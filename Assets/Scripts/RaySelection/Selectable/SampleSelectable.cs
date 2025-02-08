using UnityEngine;

namespace RaySelection.Selectable
{
    public class SampleSelectable : Selectable
    {
        public Renderer rend;
        private bool selected = false;
        private bool hovered = false;
    
        public override void Select()
        {
            selected = !selected;
        }
    
        public override void Hover()
        {
            hovered = true;
        }

        public void Start()
        {
            if (rend == null)
            {
                rend = GetComponent<Renderer>();
            }
        }

        public void LateUpdate()
        {
            Render();
            hovered = false;
        }

        private void Render()
        {
            if (hovered && selected)
            {
                rend.material.color = new Color(1,.5f,0,1);
            }
            else if (hovered)
            {
                rend.material.color = Color.yellow;
            }
            else if (selected)
            {
                rend.material.color = Color.red;
            }
            else
            {
                rend.material.color = Color.white;
            }
        }
    }
}
