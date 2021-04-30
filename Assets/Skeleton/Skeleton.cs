using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public GameObject EnemyObject;
    public Vector3 Zentr;
    
    public PlayerController Sam;
    
    public float EnemySpeed;
    public float SpeedBoost;
    public int health = 10;

    private void Start()
    {
        Sam = PlayerController.FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            Death();
        }
    }

    void FixedUpdate() {
        Move();
    }
    
    void Move()
    {
        float dist = Vector3.Distance(Sam.transform.position, transform.position);

        if (dist <= 10)
        {
            Zentr = Sam.transform.position;
            Zentr.y = 0;
            EnemyObject.GetComponent<Animation>().Play("walk");
            Vector3 EnemyMove = Vector3.MoveTowards(transform.position, Zentr, EnemySpeed * SpeedBoost * Time.deltaTime);
        
            transform.position = new Vector3(EnemyMove.x, EnemyMove.y, EnemyMove.z);
            transform.LookAt(Zentr);
        }
    }
/*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Sam.health -= 0.08f;
        } else if (other.tag == "Default Sword")
        {
            health -= 5;
        }
    }
*/
    private void Death()
    {
        Destroy(gameObject);
    }
}
