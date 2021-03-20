using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollisionNotificator : MonoBehaviour
{
    public ThumbPosing receiver;

    private void Awake()
    {
        if (!receiver)
        {
            receiver = GetComponentInParent<ThumbPosing>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        receiver.OnCollision(other, GetComponent<Collider>());
    }
}