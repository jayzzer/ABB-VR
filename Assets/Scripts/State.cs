using System.Collections;

public abstract class State
{
    protected DoorStateSystem doorStateSystem;

    public State(DoorStateSystem doorStateSystemRef)
    {
        doorStateSystem = doorStateSystemRef;
    }

    public virtual IEnumerator Start()
    {
        yield break;
    }

    public virtual IEnumerator OpenDoor()
    {
        yield break;
    }
    
    public virtual IEnumerator CloseDoor()
    {
        yield break;
    }

    public virtual IEnumerator LockDoor()
    {
        yield break;
    }

    public virtual IEnumerator UnlockDoor()
    {
        yield break;
    }
}