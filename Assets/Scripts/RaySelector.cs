using System;
using System.Collections.Generic;
using UnityEngine;

public class RaySelector : Selector
{
    [SerializeField]
    protected Transform origin;

    [SerializeField] private LayerMask selectables, blocker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (origin == null)
        {
            origin = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public override List<Selection> GetSelectionList(float radius, float distance)
    {
        var ray = GetRay();
        var hits = Physics.SphereCastAll(ray, radius, distance, selectables | blocker);
        
        List<Selection> selection = new List<Selection>();
        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out Selectable selectable))
            {
                selection.Add(new Selection(selectable, hit.point, GetAccuracy(hit.point)));
            }
        }

        return selection;
    }

    public override bool TryGetBestSelection(float radius, float distance, out Selection bestSelection)
    {
        var selectionList = GetSelectionList(radius, distance);
        bestSelection = null;
        
        float bestSelectionScore = -1;
        foreach (var selection in selectionList)
        {
            if (selection.accuracy > bestSelectionScore)
            {
                bestSelectionScore = selection.accuracy;
                bestSelection = selection;
            }
        }

        return bestSelection != null;
    }
    
    public override bool TryGetBestSelection(List<Selection> selectionList, out Selection bestSelection)
    {
        bestSelection = null;
        
        float bestSelectionScore = -1;
        foreach (var selection in selectionList)
        {
            if (selection.accuracy > bestSelectionScore)
            {
                bestSelectionScore = selection.accuracy;
                bestSelection = selection;
            }
        }

        return bestSelection != null;
    }

    public override float GetAccuracy(Vector3 point)
    {
        var ray = GetRay();
        var dir = (point - GetRay().origin).normalized;
        var accuracy = Vector3.Dot(ray.direction, dir);
        return accuracy;
    }
    
    public override Ray GetRay()
    {
        var start = origin.position;
        var dir = origin.forward;
        return new Ray(start, dir);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(GetRay().origin, GetRay().direction * 10);
    }
}
