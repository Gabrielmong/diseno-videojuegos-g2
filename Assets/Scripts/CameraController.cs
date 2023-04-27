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

    [SerializeField]
    private float maxZoom = 25f;

    [SerializeField]
    private float minZoom = 6f;

    private bool isChangingPerspective = false;
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

    void Update()
    {
        handleCameraPan();

        if (!isChangingPerspective)
        {
            if (Input.GetKey(KeyCode.Keypad5))
            {
                ChangePerspective();
                isChangingPerspective = true;
                StartCoroutine(ChangePerspectiveCoroutine());
            }
        }
    }

    private void handleCameraPan()
    {
        if (Input.GetKey(KeyCode.Keypad8))
        {
            transform.RotateAround(target.position, transform.right, 1f);
        }
        else if (Input.GetKey(KeyCode.Keypad2))
        {
            transform.RotateAround(target.position, transform.right, -5f);
        }
        else if (Input.GetKey(KeyCode.Keypad4))
        {
            transform.RotateAround(target.position, transform.up, -7f);

            transform.RotateAround(target.position, transform.right, -9f);

        }
        else if (Input.GetKey(KeyCode.Keypad6))
        {
            transform.RotateAround(target.position, transform.up, 7f);

            transform.RotateAround(target.position, transform.right, -9f);
        }

        // zoom in and out
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            if (offset.magnitude > minZoom)
            {
                offset = offset.normalized * (offset.magnitude - 0.1f);
            }
        }
        else if (Input.GetKey(KeyCode.KeypadMinus))
        {
            if (offset.magnitude < maxZoom)
            {
                offset = offset.normalized * (offset.magnitude + 0.1f);
            }
        }
    }

    public void ChangePerspective()
    {
        if (Camera.main.orthographic)
        {
            Camera.main.orthographic = false;
            Camera.main.fieldOfView = 60;
        }
        else
        {
            offset = offset.normalized * maxZoom;

            Camera.main.orthographic = true;
            Camera.main.orthographicSize = 10;
        }
    }

    IEnumerator ChangePerspectiveCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        isChangingPerspective = false;
    }
}
