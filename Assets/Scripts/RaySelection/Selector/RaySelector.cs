using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RaySelection.Selector
{
    public abstract class RaySelector : MonoBehaviour
    {
        private const int SPEHERECAST_STEPS = 3;
    
        [SerializeField] private LayerMask selectables, blocker;
        [SerializeField] private bool isRayVisible = false;

        protected IEnumerable<Selection> GetRaycastSelections(float radius, float distance, Ray ray)
        {
            var hits = Physics.SphereCastAll(ray, radius, distance, selectables | blocker);
        
            List<Selection> selection = new List<Selection>();
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out Selectable.Selectable selectable))
                {
                    selection.Add(new Selection(selectable, hit.point, GetAccuracy(hit.point)));
                }
            }
            return selection;
        }
    
        public virtual IEnumerable<Selection> GetSelectionList(float radius, float distance)
        {
            var ray = GetRay();
            HashSet<Selection> selection = new HashSet<Selection>();
            for (int n = 1; n <= SPEHERECAST_STEPS; n++)
            {
                var radiusFrac = radius * (n / (float)SPEHERECAST_STEPS); 
                var selections = GetRaycastSelections(radiusFrac, distance, ray);
                selection.AddRange(selections);
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
    
        public virtual bool TryGetBestSelection(IEnumerable<Selection> selectionList, out Selection bestSelection)
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
    }
}
