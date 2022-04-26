using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtack : MonoBehaviour
{
    private float atackTime;
    public float startTimeAtack;

    public Transform AtackPos;
    public LayerMask Enemy;
    public float atackRange;
    public int damage;
    public Animator Anim;

    private void Update() {
        if (Time.timeScale == 1) {
            if (startTimeAtack <= 0) {
                if (Input.GetMouseButtonDown(0)) {
                    Anim.SetTrigger("Atack");
                
                    Collider[] enemies = Physics.OverlapSphere(AtackPos.position, atackRange, Enemy);
                    for (int i = 0; i < enemies.Length; i++) {
                        enemies[i].GetComponent<Enemy>().Damage(1);
                    }
                }
                startTimeAtack = atackTime;
            } else {
                startTimeAtack -= Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(AtackPos.position, atackRange);
    }
}
