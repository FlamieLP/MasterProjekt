using System.Collections.Generic;
using UnityEngine;

public abstract class Selector : MonoBehaviour
{

    public abstract List<Selection> GetSelectionList(float radius, float distance);

    public abstract bool TryGetBestSelection(float radius, float distance, out Selection bestSelection);

    public abstract bool TryGetBestSelection(List<Selection> selectionList, out Selection bestSelection);

    public abstract float GetAccuracy(Vector3 point);

    public abstract Ray GetRay();
    
    [System.Serializable]
    public class Selection
    {
        public Selectable selectable;
        public Vector3 point;
        public float accuracy;

        public Selection(Selectable selectable, Vector3 point, float accuracy)
        {   
            this.selectable = selectable;
            this.point = point; 
            this.accuracy = accuracy;
        }

        public override string ToString()
        {
            return $"Selectable: {selectable}, Point: {point}, Accuracy: {accuracy}";
        }
    }
}
