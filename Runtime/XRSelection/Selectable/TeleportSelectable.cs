using UnityEngine;
using UnityEngine.Assertions;

namespace XRSelection.Selectable
{
    public class TeleportSelectable : TintSelectable
    {
        [Header("Teleport")]
        [SerializeField] private Transform obj;
        [SerializeField] private Transform target;


        new void Start()
        {
            base.Start();
            Assert.IsNotNull(obj);
            Assert.IsNotNull(target);
        }

        public override void Select()
        {
            obj.position = target.position;
        }
    }
}
