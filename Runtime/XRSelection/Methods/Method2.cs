using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using XRSelection.Selector;

namespace XRSelection.Methods
{
    public class Method2 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RaySelector gazeSelector, handSelector;
    
        [Header("Inputs")]
        [SerializeField] private InputActionReference select;

        [Header("Settings")] 
        [SerializeField] private float angle;
        [SerializeField] private float distance;
        [SerializeField] 
        [Range(0,1)] 
        [Tooltip("Determines the weighing of the selections. 1 means only hand matters, 0 means only gaze matters.")]
        private float handWeight = 0.4f;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Assert.IsNotNull(gazeSelector);
            Assert.IsNotNull(handSelector);
            Assert.IsNotNull(select);
        }

        // Update is called once per frame
        void Update()
        {
            //Hover
            OnSelection(selection => selection.selectable.Hover());
            if (select.action.WasPerformedThisFrame())
            {
                //Select
                OnSelection(selection => selection.selectable.Select());
            }
        }
    
        private void OnSelection(Action<Selection> action)
        {
            var gazeSelection = gazeSelector.GetSelectionList(angle, distance);
            var handSelection = handSelector.GetSelectionList(angle, distance);
            gazeSelection = gazeSelection.Select(selection => new Selection(
                selection.selectable, 
                selection.point,
                Mathf.Lerp(selection.accuracy, handSelector.GetAccuracy(selection.point), handWeight))
            ).ToList();
            handSelection = handSelection.Select(selection => new Selection(
                selection.selectable, 
                selection.point,
                Mathf.Lerp(selection.accuracy, gazeSelector.GetAccuracy(selection.point), 1-handWeight))
            ).ToList();

            if (gazeSelector.TryGetBestSelection(gazeSelection.Concat(handSelection).ToList(), out var bestSelection))
            {
                action(bestSelection);
            }
        }
    }
}
