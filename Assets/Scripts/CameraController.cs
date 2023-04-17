using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float smoothSpeed = 0.125f;

    [SerializeField]
    private Vector3 offset;

    void Start()
    {
        if (offset == Vector3.zero)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
