using UnityEngine;

namespace RaySelection.Selector
{
    public class SimpleSelector : RaySelector
    {
        [SerializeField]
        protected Transform origin;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (origin == null)
            {
                origin = transform;
            }
        }
    
        public override Ray GetRay()
        {
            var start = origin.position;
            var dir = origin.forward;
            return new Ray(start, dir);
        }
    }
}
