using UnityEngine;
using UnityEngine.Events;

namespace XRSelection.Selectable
{
    public class EventSelectable : Selectable
    {
        [SerializeField] protected UnityEvent onSelect, onDeselect, onHoverStart, onHover, onHoverEnd;

        private bool isHovered, isSelected;
        private bool wasHoveredLastFrame;

        public override void Select()
        {
            TransitionSelectedState(!isSelected);
        }

        public override void Hover()
        {
            TransitionHoverState(true);
        }
        
        public void LateUpdate()
        {
            TransitionHoverState(false);
        }

        private void TransitionHoverState(bool hovered)
        {
            switch (hovered)
            {
                case true when !wasHoveredLastFrame:
                    isHovered = true;
                    wasHoveredLastFrame = true;
                    onHoverStart.Invoke();
                    onHover.Invoke();
                    break;
                case true when wasHoveredLastFrame:
                    isHovered = true;
                    onHover.Invoke();
                    break;
                case false when isHovered:
                    isHovered = false;
                    break;
                case false when !isHovered && wasHoveredLastFrame:
                    isHovered = false;
                    wasHoveredLastFrame = false;
                    onHoverEnd.Invoke();
                    break;
            }
        }

        private void TransitionSelectedState(bool selected)
        {
            switch (selected)
            {
                case true when !isSelected:
                    isSelected = true;
                    onSelect.Invoke();
                    break;
                case false when isSelected:
                    isSelected = false;
                    onDeselect.Invoke();
                    break;
            }
        }

        public void Print(string text)
        {
            print(text);
        }
    }
}
