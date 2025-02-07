using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Actor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Selector gazeSelector, handSelector, scopeSelector;
    
    [Header("Inputs")]
    [SerializeField] private InputActionReference select;
    
    [Header("Settings")]
    [SerializeField] private float radius, distance, weight = 0.4f;
    [Range(0, 180)]
    [SerializeField] private float maxHeadAngle = 45f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Hover();
        OnSelection(selection => selection.selectable.Hover());
        if (select.action.WasPerformedThisFrame())
        {
            //Select();
            OnSelection(selection => selection.selectable.Select());
        }
    }

    public void Select()
    {
        var handSelection = handSelector.GetSelectionList(radius, distance);
        List<Selector.Selection> selections = UseGazeRay() ? handSelection.Select(selection => new Selector.Selection(
            selection.selectable, 
            selection.point,
            Mathf.Lerp(selection.accuracy, gazeSelector.GetAccuracy(selection.point), weight))
        ).ToList() : handSelection;

        if (handSelector.TryGetBestSelection(selections, out var bestSelection))
        {
            bestSelection.selectable.Select();
        }
    }
    
    public void OnSelection(Action<Selector.Selection> action)
    {
        var gazeSelection = gazeSelector.GetSelectionList(radius, distance);
        var handSelection = handSelector.GetSelectionList(radius, distance);
        gazeSelection = gazeSelection.Select(selection => new Selector.Selection(
            selection.selectable, 
            selection.point,
            Mathf.Lerp(selection.accuracy, handSelector.GetAccuracy(selection.point), weight))
        ).ToList();
        handSelection = handSelection.Select(selection => new Selector.Selection(
            selection.selectable, 
            selection.point,
            Mathf.Lerp(selection.accuracy, gazeSelector.GetAccuracy(selection.point), 1-weight))
        ).ToList();

        if (gazeSelector.TryGetBestSelection(gazeSelection.Concat(handSelection).ToList(), out var bestSelection))
        {
            action(bestSelection);
        }
    }
    
    public void Hover()
    {
        var handSelection = handSelector.GetSelectionList(radius, distance);
        List<Selector.Selection> selections = UseGazeRay() ? handSelection.Select(selection => new Selector.Selection(
            selection.selectable, 
            selection.point,
            Mathf.Lerp(selection.accuracy, gazeSelector.GetAccuracy(selection.point), weight))
        ).ToList() : handSelection;

        if (handSelector.TryGetBestSelection(selections, out var bestSelection))
        {
            bestSelection.selectable.Hover();
        }
    }
    
    public void Select2()
    {
        if (scopeSelector.TryGetBestSelection(radius, distance, out var bestSelection))
        {
            bestSelection.selectable.Select();
        }
    }

    private bool UseGazeRay()
    {
        var gazeRay = gazeSelector.GetRay();
        var handRay = handSelector.GetRay();
        
        var angle = Vector3.Angle(gazeRay.direction, handRay.direction);
        return angle < maxHeadAngle;
    } 
}
