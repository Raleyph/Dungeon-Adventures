using System;
using UnityEngine;
using Random = System.Random;

public class Room : MonoBehaviour {
    public GameObject DoorU;
    public GameObject DoorR;
    public GameObject DoorD;
    public GameObject DoorL;

    public Mesh[] BlockMeshes;

    public AnimationCurve SpawnChange;

    private void Start()
    {
        foreach (var filter in GetComponentsInChildren<MeshFilter>())
        {
            if (filter.sharedMesh == BlockMeshes[0])
            {
                filter.sharedMesh = BlockMeshes[UnityEngine.Random.Range(0, BlockMeshes.Length)];
                filter.transform.rotation = Quaternion.Euler(-90, 0, 90 * UnityEngine.Random.Range(0, 4));
            }
        }
    }

    public void RotateRandomly()
    {
        int count = UnityEngine.Random.Range(0, 4);

        for (int i = 0; i < count; i++)
        {
            transform.Rotate(0, 90, 0);

            GameObject tmp = DoorL;
            DoorL = DoorD;
            DoorD = DoorR;
            DoorR = DoorU;
            DoorU = tmp;
        }
    }
}
