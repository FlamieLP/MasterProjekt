using UnityEngine;
using UnityEngine.Assertions;

namespace RaySelection.Selectable
{
    public class TeleportPlayerSelectable : SampleSelectable
    {
        [Header("Teleport")]
        [SerializeField] private Transform player;
        [SerializeField] private Transform target;

        new void Start()
        {
            base.Start();
            Assert.IsNotNull(player);
            Assert.IsNotNull(target);
        }

        public override void Select()
        {
            player.position = target.position;
        }
    }
}
