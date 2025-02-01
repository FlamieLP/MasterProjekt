using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Actor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RaySelector selector;
    
    [Header("Inputs")]
    [SerializeField] private InputActionReference select;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (selector.TryGetBestSelection(1, 10, out RaySelector.Selection selection))
        {
            print("Move");
            selection.selectable.gameObject.transform.Translate(Vector3.up);
        }
    }
}
