using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ThumbPosing : MonoBehaviour
{
    public List<Transform> phalanges = new List<Transform>();
    public float squeezeSpeed = 10f;

    private List<Quaternion> _initialPhalanxTransforms = new List<Quaternion>();
    private int _currentPhalanxInd;
    private Coroutine _squeezing;

    private void Awake()
    {
        UpdatePhalanges();
    }

    private void UpdatePhalanges()
    {
        phalanges.Clear();
        var childColliders = GetComponentsInChildren<Collider>();
        foreach (var childCollider in childColliders)
        {
            phalanges.Add(childCollider.transform);
            _initialPhalanxTransforms.Add(childCollider.transform.localRotation);

            childCollider.gameObject.AddComponent<CollisionNotificator>();
        }

        _currentPhalanxInd = 0;
    }

    public void StartSqueeze()
    {
        _squeezing = StartCoroutine(Squeezing());
    }

    public void StopSqueeze()
    {
        StopCoroutine(_squeezing);
        _currentPhalanxInd = 0;
    }

    private IEnumerator Squeezing()
    {
        while (true)
        {
            var turnAngle = _initialPhalanxTransforms[_currentPhalanxInd] * Quaternion.Euler(0f, 0f, -70f);
            phalanges[_currentPhalanxInd].localRotation = Quaternion.Lerp(phalanges[_currentPhalanxInd].localRotation, turnAngle,
                squeezeSpeed * Time.deltaTime);

            var difference = Quaternion.Angle(phalanges[_currentPhalanxInd].localRotation, turnAngle);
            if (difference <= 0.5f)
            {
                SetNextPhalanx(_currentPhalanxInd);
            }
            // phalanges[_currentPhalanxInd].Rotate(0, 0, -squeezeSpeed * Time.deltaTime);

            // if (phalanges[_currentPhalanxInd].localRotation.eulerAngles.z >=
            //     _initialPhalanxTransforms[_currentPhalanxInd].eulerAngles.z - 70)
            // {
            //     SetNextPhalanx(_currentPhalanxInd + 1);
            // }

            yield return null;
        }
    }

    public void ResetThumb()
    {
        for (var i = 0; i < phalanges.Count; i++)
        {
            phalanges[i].localRotation = _initialPhalanxTransforms[i];
        }
    }

    public void OnCollision(Collider other, Collider source)
    {
        var colliderGameObject = other.gameObject;
        if (!colliderGameObject.layer.Equals(6)) return;

        var collidedPhalanxInd = int.Parse(source.name.Substring(source.name.Length - 1, 1)) - 1;
        SetNextPhalanx(collidedPhalanxInd);
    }

    private void SetNextPhalanx(int newInd)
    {
        if (newInd == phalanges.Count - 1)
        {
            StopSqueeze();
        }
        else
        {
            _currentPhalanxInd = newInd + 1;
        }
    }
}