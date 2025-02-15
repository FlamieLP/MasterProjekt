using RaySelection.Selectable;
using UnityEngine;

namespace RaySelection.Selector
{
    [System.Serializable]
    public class Selection
    {
        public readonly ISelectable selectable;
        public readonly Vector3 point;
        public readonly float accuracy;

        public Selection(ISelectable selectable, Vector3 point, float accuracy)
        {   
            this.selectable = selectable;
            this.point = point; 
            this.accuracy = accuracy;
        }

        public int CompareTo(Selection other)
        {
            if (other == null) return 1;
            
            return selectable.GetIdentifier().CompareTo(other.selectable.GetIdentifier());
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Selection)) return false;
            
            return obj.GetHashCode().Equals(GetHashCode());
        }

        public override int GetHashCode()
        {
            return selectable.GetIdentifier();
        }

        public override string ToString()
        {
            return $"Selectable: {selectable}, Point: {point}, Accuracy: {accuracy}";
        }
    }
}
