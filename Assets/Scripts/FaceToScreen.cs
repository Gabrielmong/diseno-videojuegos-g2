using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToScreen : MonoBehaviour
{
    void Start()
    {
        transform.LookAt(Camera.main.transform);

        transform.Rotate(0, 180, 0);

        Destroy(gameObject, 2);
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 2);
    
        transform.Translate(Vector3.right * Time.deltaTime * Mathf.Sin(Time.time * 10));

        Color color = GetComponent<Renderer>().material.color;
        color.a -= Time.deltaTime / 3;
        GetComponent<Renderer>().material.color = color;  

    }
}
