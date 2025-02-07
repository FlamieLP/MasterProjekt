using System.Collections.Generic;
using UnityEngine;

public class ScopedSelector : Selector
{
    [SerializeField] private Transform origin, scope;

    public override Ray GetRay()
    {
        return new Ray(origin.position, (scope.position - origin.position).normalized);
    }
}
