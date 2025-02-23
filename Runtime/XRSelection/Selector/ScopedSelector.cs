using UnityEngine;

namespace XRSelection.Selector
{
    public class ScopedSelector : RaySelector
    {
        [SerializeField] private Transform origin, scope;

        public override Ray GetRay()
        {
            return new Ray(origin.position, (scope.position - origin.position).normalized);
        }
    }
}
