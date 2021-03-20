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
    private Transform _currentPhalanx;
    private Coroutine _squeezing;

    private void Awake()
    {
        UpdatePhalanges();
    }

    public void UpdatePhalanges()
    {
        phalanges.Clear();
        var childColliders = GetComponentsInChildren<Collider>();
        foreach (var childCollider in childColliders)
        {
            phalanges.Add(childCollider.transform);
            _initialPhalanxTransforms.Add(childCollider.transform.localRotation);

            childCollider.gameObject.AddComponent<CollisionNotificator>();
        }

        _currentPhalanx = phalanges[0];
    }

    public void StartSqueeze()
    {
        _squeezing = StartCoroutine(Squeezing());
    }

    public void StopSqueeze()
    {
        StopCoroutine(_squeezing);
        _currentPhalanx = phalanges[0];
    }

    private IEnumerator Squeezing()
    {
        while (true)
        {
            _currentPhalanx.Rotate(0, 0, -squeezeSpeed * Time.deltaTime);

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
        if (collidedPhalanxInd == phalanges.Count - 1)
        {
            StopSqueeze();
        }
        else
        {
            _currentPhalanx = phalanges[collidedPhalanxInd + 1];
        }
    }
}