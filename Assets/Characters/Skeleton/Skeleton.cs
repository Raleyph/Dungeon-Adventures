using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour {
    public Vector3 Zentr;
    private GameObject Sam;
    
    public float EnemySpeed;
    public int health = 10;
    public bool isActive;

    private Animator Anim;

    private void Start() {
        Sam = GameObject.FindWithTag("Player");
        Anim = GetComponent<Animator>();
    }

    void FixedUpdate() {
        if (isActive) {
            float dist = Vector3.Distance(Sam.transform.position, transform.position);

            if (dist <= 20) {
                Zentr = Sam.transform.position;
                Zentr.y = 0;
                Vector3 EnemyMove = Vector3.MoveTowards(transform.position, Zentr, EnemySpeed * Time.deltaTime);

                transform.position = new Vector3(EnemyMove.x, EnemyMove.y, EnemyMove.z);
                transform.LookAt(Zentr);
                Anim.SetBool("Move", true);
            } else {
                Anim.SetBool("Move", false);
            }
        }
    }
    
    private void Death() {
        Destroy(gameObject);
    }
}
