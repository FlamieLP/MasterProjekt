using System;
using RaySelection.Selector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace RaySelection.Methods
{
    public class Method3 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RaySelector selector;
    
        [Header("Inputs")]
        [SerializeField] private InputActionReference select;

        [Header("Settings")] 
        [SerializeField] private float radius;
        [SerializeField] private float distance;
    
        void Start()
        {
            Assert.IsNotNull(selector);
            Assert.IsNotNull(select);
        }

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
            if (selector.TryGetBestSelection(radius, distance, out var bestSelection))
            {
                action(bestSelection);
            }
        }
    }
}
