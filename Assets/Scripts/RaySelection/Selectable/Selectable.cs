using UnityEngine;

namespace RaySelection.Selectable
{
    public abstract class Selectable : MonoBehaviour, ISelectable
    {
        public virtual int GetIdentifier()
        {
            return gameObject.GetHashCode();
        }

        /// <summary>
        /// Called once when Selectable ist selected
        /// </summary>
        public abstract void Select();
    
        /// <summary>
        /// Called every frame on the hovered Selectable
        /// </summary>
        public abstract void Hover();
    }
}
