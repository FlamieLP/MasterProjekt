using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Method1 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Selector gazeSelector;
    [SerializeField] private Transform hand;
    
    [Header("Inputs")]
    [SerializeField] private InputActionReference preSelect;
    [SerializeField] private InputActionReference select;

    [Header("Settings")] [SerializeField] private float radius;
    [SerializeField] private float distance, objectSpacing = 0.1f;
    
    [SerializeField] private List<Selector.Selection> preSelections = new List<Selector.Selection>();

    private Vector3 selectOrigin, selectXAxis;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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

    void PreSelect()
    {
        var ray = gazeSelector.GetRay();
        preSelections = gazeSelector.GetSelectionList(radius, distance).OrderBy(selection =>
        {
            var right = Vector3.Cross(Vector3.up, ray.direction ).normalized;
            var dir = selection.selectable.transform.position - ray.origin;
            return Vector3.Dot(dir, right);
        }).ToList();
        
        selectOrigin = hand.position;
        selectXAxis = hand.right;
    }
    
    void SelectMode(Action<Selector.Selection> action)
    {
        var relPos = hand.position - selectOrigin;
        var offset = Vector3.Dot(selectXAxis, relPos);
        var scale = preSelections.Count * objectSpacing;
        var index = Mathf.RoundToInt(Mathf.Lerp(0, preSelections.Count - 1,((offset + (scale / 2)) / scale)));

        action(preSelections[index]);

    }
}
