using System;
using UnityEngine;

public class DoorStateSystem : StateMachine
{
    #region References

    public GameObject door;
    public GameObject doorHandle;

    private DoorHandle _doorHandleComp;

    #endregion

    private void Awake()
    {
        _doorHandleComp = doorHandle.GetComponent<DoorHandle>();
    }

    private void OnEnable()
    {
        _doorHandleComp.onReachedEnd.AddListener(OnDoorHandleReachedEnd);
        _doorHandleComp.onUnreachedEnd.AddListener(OnDoorHandleUnreachedEnd);
    }

    private void OnDisable()
    {
        _doorHandleComp.onReachedEnd.RemoveListener(OnDoorHandleReachedEnd);
        _doorHandleComp.onUnreachedEnd.RemoveListener(OnDoorHandleUnreachedEnd);
    }

    private void Start()
    {
        SetState(new ClosedState(this));
    }

    private void OnDoorHandleReachedEnd()
    {
        StartCoroutine(state.OpenDoor());
    }

    private void OnDoorHandleUnreachedEnd()
    {
        StartCoroutine(state.CloseDoor());
    }

    public void OnDoorUnlocked()
    {
        StartCoroutine(state.UnlockDoor());
    }
    
    public void OnDoorLocked()
    {
        StartCoroutine(state.LockDoor());
    }
}