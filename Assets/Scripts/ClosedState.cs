using System.Collections;
using UnityEngine;

public class ClosedState : State
{
    public ClosedState(DoorStateSystem doorStateSystemRef) : base(doorStateSystemRef)
    {
    }

    public override IEnumerator Start()
    {
        // Делаем невозможным открыть дверь
        doorStateSystem.door.transform.localRotation = Quaternion.identity;
        doorStateSystem.door.GetComponent<Rigidbody>().isKinematic = true;

        // Также убираем возможность пользоваться ручкой на двери
        doorStateSystem.doorHandle.GetComponent<Rigidbody>().isKinematic = true;

        yield break;
    }

    public override IEnumerator UnlockDoor()
    {
        doorStateSystem.SetState(new CanOpenState(doorStateSystem));

        yield break;
    }
}