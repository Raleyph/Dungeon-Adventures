using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour {
    public Vector3 Zentr;
    private GameObject Sam;
    
    public float EnemySpeed;
    public float SpeedBoost;
    public int health = 10;

    private void Start() {
        StartCoroutine(CheckPlayer());
    }

    IEnumerator CheckPlayer() {
        yield return new WaitForSeconds(1f);
        Sam = GameObject.FindWithTag("Player");
    }

    void FixedUpdate() {
        float dist = Vector3.Distance(Sam.transform.position, transform.position);

        if (dist <= 10) {
            Zentr = Sam.transform.position;
            Zentr.y = 0;
            Vector3 EnemyMove = Vector3.MoveTowards(transform.position, Zentr, EnemySpeed * SpeedBoost * Time.deltaTime);
        
            transform.position = new Vector3(EnemyMove.x, EnemyMove.y, EnemyMove.z);
            transform.LookAt(Zentr);
        }
    }
    
    private void Death() {
        Destroy(gameObject);
    }
}
