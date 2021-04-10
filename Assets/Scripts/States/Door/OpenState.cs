using System.Collections;
using UnityEngine;

namespace States.Door
{
    public class OpenState : DoorState
    {
        public OpenState(DoorStateSystem doorStateSystemRef) : base(doorStateSystemRef)
        {
        }

        public override IEnumerator Start()
        {
            doorStateSystem.door.GetComponent<Rigidbody>().isKinematic = false;

            yield break;
        }

        public override IEnumerator CloseDoor()
        {
            var doorJoint = doorStateSystem.door.GetComponent<HingeJoint>();
            if (doorJoint.angle <= doorJoint.limits.min + 0.1f)
                doorStateSystem.SetState(new CanOpenState(doorStateSystem));

            yield break;
        }
    }
}