using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Actor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RaySelector gazeSelector, handSelector, scopeSelector;
    
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
        if (select.action.WasPerformedThisFrame())
        {
            Select2();
        }
    }

    public void Select()
    {
        var handSelection = handSelector.GetSelectionList(radius, distance);
        List<RaySelector.Selection> selections = UseGazeRay() ? handSelection.Select(selection => new RaySelector.Selection(
            selection.selectable, 
            selection.point,
            Mathf.Lerp(selection.accuracy, gazeSelector.GetAccuracy(selection.point), weight))
        ).ToList() : handSelection;

        if (handSelector.TryGetBestSelection(selections, out var bestSelection))
        {
            bestSelection.selectable.Select();
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
