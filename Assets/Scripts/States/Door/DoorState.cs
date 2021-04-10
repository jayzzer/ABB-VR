using System.Collections;

namespace States.Door
{
    public abstract class DoorState : IState
    {
        protected DoorStateSystem doorStateSystem;

        public DoorState(DoorStateSystem doorStateSystemRef)
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
}