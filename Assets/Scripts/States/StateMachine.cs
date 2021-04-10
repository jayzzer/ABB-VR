using UnityEngine;

namespace States
{
    public abstract class StateMachine<T> : MonoBehaviour where T : IState
    {
        protected T state;

        public void SetState(T newState)
        {
            Debug.Log("changed State to " + newState);
            state = newState;
            StartCoroutine(state.Start());
        }
    }
}