using UnityEngine;

public abstract class Selectable : MonoBehaviour
{

    // Called once when Selectable ist selected
    public abstract void Select();
    
    // Called every frame on the hovered Selectable
    public abstract void Hover();
}
