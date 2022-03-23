using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public GameObject Target;

    public Vector3[] Offset;
    public Vector3[] Quat;
    
    private Vector3 Velocity;
    //private Vector3 Offset = new Vector3(0f, 12f, -6f);
    private Vector3 StartOffset = new Vector3(3f, 1.5f, 3f);
    
    public int zoomType;

    private void Awake() {
        transform.position = StartOffset;
        transform.rotation = Quaternion.Euler(0f, -135f, 0f);
    }

    private void Start() {
        FindPlayer();
        zoomType = 0;
    }

    private void FindPlayer() {
        Target = GameObject.FindWithTag("Player");
    }

    private void Update() {
        if (Time.timeScale == 1) {
            if (Input.GetKeyDown(KeyCode.Q) && zoomType >= 0) {
                if (zoomType <= 1) {
                    zoomType++;
                } else {
                    zoomType = 0;
                }
            }

            if (Target) {
                transform.position = Vector3.SmoothDamp(transform.position, Target.transform.position + Offset[zoomType], ref Velocity, 0.18f, 40);
                transform.rotation = Quaternion.Euler(Quat[zoomType]);
            } else {
                FindPlayer();
            }
        }
    }
}
