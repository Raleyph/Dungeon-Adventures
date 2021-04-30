using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform Target;
    private Vector3 Velocity;
    private Vector3 Offset = new Vector3(0f, 12f, -6f);
    private Vector3 StartOffset = new Vector3(3f, 1.5f, 3f);

    private void Awake()
    {
        transform.position = StartOffset;
        transform.rotation = Quaternion.Euler(0f, -135f, 0f);
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, Target.position + Offset, ref Velocity, 0.18f, 40);
        
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Offset = new Vector3(2f, 0.5f, -4f);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        } else if (Input.GetKeyUp(KeyCode.E))
        {
            Offset = new Vector3(0f, 12f, -6f);
            transform.rotation = Quaternion.Euler(60f, 0f, 0f);
        }
    }
}
