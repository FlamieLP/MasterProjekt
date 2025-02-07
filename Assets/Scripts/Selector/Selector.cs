using System.Collections.Generic;
using UnityEngine;

public abstract class Selector : MonoBehaviour
{
    [SerializeField] private LayerMask selectables, blocker;
    [SerializeField] private bool isRayVisible = false;

    public virtual List<Selection> GetSelectionList(float radius, float distance)
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

    public virtual bool TryGetBestSelection(float radius, float distance, out Selection bestSelection)
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
    
    public virtual bool TryGetBestSelection(List<Selection> selectionList, out Selection bestSelection)
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

    public virtual float GetAccuracy(Vector3 point)
    {
        var ray = GetRay();
        var dir = (point - GetRay().origin).normalized;
        var accuracy = Vector3.Dot(ray.direction, dir);
        return accuracy;
    }

    public abstract Ray GetRay();

    public void OnDrawGizmos()
    {
        if (!isRayVisible) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(GetRay().origin, GetRay().direction * 10);
    }
    
    [System.Serializable]
    public class Selection
    {
        public Selectable selectable;
        public Vector3 point;
        public float accuracy;

        public Selection(Selectable selectable, Vector3 point, float accuracy)
        {   
            this.selectable = selectable;
            this.point = point; 
            this.accuracy = accuracy;
        }

        public override string ToString()
        {
            return $"Selectable: {selectable}, Point: {point}, Accuracy: {accuracy}";
        }
    }
}
