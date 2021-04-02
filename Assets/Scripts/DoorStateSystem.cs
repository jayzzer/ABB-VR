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
        SetState(new CanOpenState(this));
    }

    private void OnDoorHandleReachedEnd()
    {
        SetState(new OpenState(this));
    }

    private void OnDoorHandleUnreachedEnd()
    {
        SetState(new CanOpenState(this));
    }
}