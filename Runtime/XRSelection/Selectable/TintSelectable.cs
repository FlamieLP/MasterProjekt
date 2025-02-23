using UnityEngine;

namespace XRSelection.Selectable
{
    public class TintSelectable : Selectable
    {
        public Renderer rend;
        private bool selected = false;
        private bool hovered = false;
        
        [Header("Color")]
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color hoverColor = Color.yellow;
        [SerializeField] private Color selectCover = Color.red;
        [SerializeField] private Color hoverSelectCover = new Color(1,.5f,0,1);
    
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
                rend.material.color = hoverSelectCover;
            }
            else if (hovered)
            {
                rend.material.color = hoverColor;
            }
            else if (selected)
            {
                rend.material.color = selectCover;
            }
            else
            {
                rend.material.color = defaultColor;
            }
        }
    }
}
