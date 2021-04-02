using System.Collections;
using UnityEngine;

public class CanOpenState : State
{
    public CanOpenState(DoorStateSystem doorStateSystemRef) : base(doorStateSystemRef)
    {
    }

    public override IEnumerator Start()
    {
        doorStateSystem.doorHandle.GetComponent<Rigidbody>().isKinematic = false;

        yield break;
    }

    public override IEnumerator LockDoor()
    {
        doorStateSystem.SetState(new ClosedState(doorStateSystem));

        yield break;
    }
}