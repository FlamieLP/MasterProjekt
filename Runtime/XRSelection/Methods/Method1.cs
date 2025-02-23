using System;
using System.Collections.Generic;
using System.Linq;
using XRSelection.Selector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace XRSelection.Methods
{
    public class Method1 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GazeSelector gazeSelector;
        [SerializeField] private Transform hand;
    
        [Header("Inputs")]
        [SerializeField] private InputActionReference preSelect;
        [SerializeField] private InputActionReference select;

        [Header("Settings")] [SerializeField] private float angle;
        [SerializeField] private float distance, objectSpacing = 0.1f;
    
        [SerializeField] private List<Selection> preSelections = new List<Selection>();

        private Vector3 selectOrigin, selectXAxis;
        private int startIndex;

        void Update()
        {
            if (preSelections.Count == 0 && preSelect.action.WasPerformedThisFrame())
            {
                PreSelect();
            } else if (preSelections.Count > 0)
            {
                SelectMode(selection => selection.selectable.Hover());
                if (select.action.WasPerformedThisFrame())
                {
                    SelectMode(selection => selection.selectable.Select());
                    preSelections.Clear();
                }
            }
        }

        private void PreSelect()
        {
            var ray = gazeSelector.GetRay();
            startIndex = 0;
            preSelections = gazeSelector.GetSelectionList(angle, distance).OrderBy(selection =>
            {
                var right = Vector3.Cross(Vector3.up, ray.direction ).normalized;
                var dir = selection.point - ray.origin;
                var offset = Vector3.Dot(dir, right);
                if (offset < 0)
                {
                    startIndex += 1;
                }
                return offset;
            }).ToList();
        
            selectOrigin = hand.position;
            selectXAxis = hand.right;
        }
    
        private void SelectMode(Action<Selection> action)
        {
            var relPos = hand.position - selectOrigin;
            var offset = Vector3.Dot(selectXAxis, relPos);
            var scale = preSelections.Count * objectSpacing;
            var index = Mathf.RoundToInt(Mathf.Lerp(0, preSelections.Count - 1,((offset + (startIndex * objectSpacing)) / scale)));

            action(preSelections[index]);

        }
    }
}
