using System.Collections;
using UnityEngine;

public class OpenState : State
{
    public OpenState(DoorStateSystem doorStateSystemRef) : base(doorStateSystemRef)
    {
    }

    public override IEnumerator Start()
    {
        doorStateSystem.door.GetComponent<Rigidbody>().isKinematic = false;

        yield break;
    }
}