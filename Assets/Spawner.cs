using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour {
    public GameObject[] Enemy;
    public Transform[] SpawnPos;

    private void OnTriggerStay(Collider other)
    {
        StartCoroutine(Spawn());
    }

    private void OnTriggerExit(Collider other)
    {
        StopCoroutine(Spawn());
    }

    void Repeat()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(15f);
        Instantiate(Enemy[Random.Range(0, Enemy.Length)], SpawnPos[Random.Range(0, SpawnPos.Length)].position, Quaternion.identity);
        yield return new WaitForSeconds(15f);
        Repeat();
    }
}
