using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject[] Enemy;
    public int SpawnerType;

    public void DoSpawn() {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn() {
        switch (SpawnerType) {
            case 1:
                Instantiate(Enemy[0]);
                break;
            case 2:
                Instantiate(Enemy[1]);
                break;
            case 3:
                Instantiate(Enemy[Random.Range(1, 2)]);
                break;
        }
        yield return new WaitForSeconds(5f);
    }
}
