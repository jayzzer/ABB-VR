using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics deviceCharacteristics;

    private InputDevice _targetDevice;
    private GameObject _spawnedHandModel;
    private Animator _handAnimator;

    private void Awake()
    {
        _spawnedHandModel = transform.GetChild(0).gameObject;
        _handAnimator = _spawnedHandModel.GetComponent<Animator>();
        _spawnedHandModel.SetActive(false);
    }
    
    private void Start()
    {
        TryInitializeHand();
    } 

    private void TryInitializeHand()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(deviceCharacteristics, devices);

        if (devices.Count <= 0) return;
        _targetDevice = devices[0];
        _spawnedHandModel.SetActive(true);
    }

    private void Update()
    {
        if (!_targetDevice.isValid)
        {
            TryInitializeHand();
            return;
        }

        UpdateHandAnimation();
    }

    private void UpdateHandAnimation()
    {
        if (_targetDevice.TryGetFeatureValue(CommonUsages.trigger, out var triggerValue))
        {
            _handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            _handAnimator.SetFloat("Trigger", 0);
        }

        if (_targetDevice.TryGetFeatureValue(CommonUsages.grip, out var gripValue))
        {
            _handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            _handAnimator.SetFloat("Grip", 0);
        }
    }
}
