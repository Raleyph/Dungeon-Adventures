using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
    [Range(0, 1)] public float Change = 0.5f;

    private void Start()
    {
        if (UnityEngine.Random.value > Change) Destroy(gameObject);
    }
}
