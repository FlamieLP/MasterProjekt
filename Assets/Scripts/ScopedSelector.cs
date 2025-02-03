using System.Collections.Generic;
using UnityEngine;

public class ScopedSelector : RaySelector
{
    [SerializeField] private Transform scope;

    public override Ray GetRay()
    {
        return new Ray(origin.position, (scope.position - origin.position).normalized);
    }
}
