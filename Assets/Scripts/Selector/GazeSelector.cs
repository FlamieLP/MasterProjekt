using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class GazeSelector : Selector
{
    [Header("OpenXR Input Actions")]
    [SerializeField] private InputActionReference position;
    [SerializeField] private InputActionReference rotation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Assert.IsNotNull(position);
        Assert.IsNotNull(rotation);
    }

    public override Ray GetRay()
    {
        return new Ray(position.action.ReadValue<Vector3>(), rotation.action.ReadValue<Quaternion>() * Vector3.forward);
    }
}
