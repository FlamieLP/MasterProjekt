using UnityEngine;

public class Selectable : MonoBehaviour
{
    public Renderer rend;
    private bool selected = false;
    
    public void Select()
    {
        selected = !selected;
        rend.material.color = selected ? Color.red : Color.white;
    }
}
