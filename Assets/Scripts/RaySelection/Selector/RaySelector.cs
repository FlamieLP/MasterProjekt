using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RaySelection.Selector
{
    public abstract class RaySelector : MonoBehaviour
    {
        private const float MAX_SPHERECAST_RADIUS = 3;
    
        [SerializeField] private LayerMask selectables, blocker;
        [SerializeField] private bool isRayVisible = false;

        protected IEnumerable<Selection> GetRaycastSelections(float radius, float distance, Ray ray, float offset)
        {
            var offsetRay = new Ray(ray.origin + ray.direction * offset, ray.direction);
            var hits = Physics.SphereCastAll(offsetRay, radius, distance, selectables);
        
            List<Selection> selection = new List<Selection>();
            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out Selectable.Selectable selectable))
                {
                    var confirmationRayToPoint = new Ray(ray.origin, (hit.point - ray.origin).normalized);
                    var confirmationRayToCenter = new Ray(ray.origin, (hit.transform.position - ray.origin).normalized);
                    var dist = (hit.point - ray.origin).magnitude;
                    var isBlockedToPoint = hit.point == Vector3.zero || !Physics.Raycast(confirmationRayToPoint, dist, blocker);
                    var isBlockedToCenter = !Physics.Raycast(confirmationRayToCenter, dist, blocker);
                    if (isBlockedToCenter || isBlockedToPoint)
                    {
                        selection.Add(new Selection(selectable, hit.point, GetAccuracy(hit.point)));
                    }
                }
            }
            return selection;
        }
        
        public virtual IEnumerable<Selection> GetSelectionList(float angle, float distance)
        {
            var ray = GetRay();
            var radius = 0.1f;
            var dist = distance;
            HashSet<Selection> selection = new HashSet<Selection>();
            while (radius <= MAX_SPHERECAST_RADIUS && dist > 0)
            {
                var offset = radius/Mathf.Tan(angle * Mathf.Deg2Rad);
                dist = distance - offset;
                var selections = GetRaycastSelections(radius, dist, ray, offset);
                selection.AddRange(selections);
                radius *= 2;
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
