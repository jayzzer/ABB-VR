using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace States.Door
{
    public class CanOpenState : DoorState
    {
        public CanOpenState(DoorStateSystem doorStateSystemRef) : base(doorStateSystemRef)
        {
        }

        public override IEnumerator Start()
        {
            doorStateSystem.door.transform.localRotation = quaternion.identity;
            doorStateSystem.door.GetComponent<Rigidbody>().isKinematic = true;

            doorStateSystem.doorHandle.GetComponent<ConfigurableJoint>().highAngularXLimit =
                new SoftJointLimit {limit = 15};

            yield break;
        }

        public override IEnumerator LockDoor()
        {
            doorStateSystem.SetState(new ClosedState(doorStateSystem));

            yield break;
        }

        public override IEnumerator OpenDoor()
        {
            doorStateSystem.SetState(new OpenState(doorStateSystem));

            yield break;
        }
    }
}