using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public GameObject Target;
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
        Target = GameObject.FindWithTag("Player");

        if (Target)
        {
            transform.position = Vector3.SmoothDamp(transform.position, Target.transform.position + Offset, ref Velocity, 0.18f, 40);
        }
    }
}
