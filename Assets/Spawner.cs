using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject[] Enemy;
    public int SpawnerType;

    IEnumerator Spawn() {
        switch (SpawnerType) {
            case 1:
                Instantiate(Enemy[0]);
                break;
            case 2:
                Instantiate(Enemy[1]);
                break;
            case 3:
                Instantiate(Enemy[2]);
                break;
        }
        yield return new WaitForSeconds(1f);
    }
}
