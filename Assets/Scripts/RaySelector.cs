using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaySelector : MonoBehaviour
{
    [SerializeField]
    private Transform origin;

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

    public List<Selection> GetSelectionList(float radius, float distance)
    {
        var ray = GetRay();
        var hits = Physics.SphereCastAll(ray, radius, distance, selectables | blocker);
        
        print($"i got {hits.Length} hits");
        List<Selection> selection = new List<Selection>();
        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out Selectable selectable))
            {
                var dir = (selectable.transform.position - origin.position).normalized;
                var accuracy = Vector3.Dot(ray.direction, dir);
                selection.Add(new Selection(selectable, accuracy));
            }
        }

        return selection;
    }
    
    public bool TryGetBestSelection(float radius, float distance, out Selection bestSelection)
    {
        print("Start Selection");
        var selectionList = GetSelectionList(radius, distance);
        bestSelection = null;
        
        float bestSelectionScore = -1;
        foreach (var selection in selectionList)
        {
            if (selection.selectionScore > bestSelectionScore)
            {
                bestSelectionScore = selection.selectionScore;
                bestSelection = selection;
            }
        }
        print($"best got {bestSelectionScore}");

        return bestSelection != null;
    }
    
    private Ray GetRay()
    {
        var start = origin.position;
        var dir = origin.forward;
        return new Ray(start, dir);
    }

    public class Selection
    {
        public Selectable selectable;
        public float selectionScore;

        public Selection(Selectable selectable, float selectionScore)
        {   
            this.selectable = selectable;
            this.selectionScore = selectionScore;
        }
    }
}
