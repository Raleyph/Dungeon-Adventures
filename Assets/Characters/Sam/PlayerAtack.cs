using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtack : MonoBehaviour {
    private Inventory IV;
    private float timer;
    public float TimeBtwAtack;

    public Transform AtackPos;
    public LayerMask Enemy;
    public float atackRange;
    public float damage;
    public Animator Anim;

    private void Start() {
        IV = GameObject.FindWithTag("GameController").GetComponent<Inventory>();
    }

    private void Update() {
        if (Time.timeScale == 1) {
            if (timer <= 0) {
                if (Input.GetMouseButtonDown(0)) {
                    Anim.SetTrigger("Atack");

                    for (int i = 0; i < IV.Cells.Length; i++) {
                        if (IV.Cells[i].busy && IV.Cells[i].itemType == "Weapon") {
                            damage = IV.Cells[i].value;
                        }
                    }
                    timer = TimeBtwAtack;
                }
            } else {
                timer -= Time.deltaTime;
            }
        }
    }

    public void Atack() {
        Collider[] enemies = Physics.OverlapSphere(AtackPos.position, atackRange, Enemy);
        for (int i = 0; i < enemies.Length; i++) {
            enemies[i].GetComponent<Enemy>().Damage(damage);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(AtackPos.position, atackRange);
    }
}
