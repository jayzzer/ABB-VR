using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State state;

    public void SetState(State newState)
    {
        Debug.Log("changed State to " + newState.ToString());
        state = newState;
        StartCoroutine(state.Start());
    }
}