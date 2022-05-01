using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour {
    [System.Serializable]
    public class Room {
        public GameObject RoomModel;
        public bool obligatory;
        public Vector2 Position;

        public void SetPosition() {
            Position = new Vector2(RoomModel.transform.position.x, RoomModel.transform.position.z);
        }
    }

    public Room[] rooms;
    public Vector2 RoomSize;
    public int minCountRooms;
    public int maxCountRooms;
    public int level;

    private void Start() {
        GenerateDungeon();
    }

    public void FirstRoom() {
        Instantiate(rooms[0].RoomModel);

        for (int i = 0; i < 4; i++) {
            if (i == Random.Range(1, 4)) {
                List<Vector2> neighbors = Neighbors(i);
                
            }
        }
    }

    public void GenerateDungeon() {
        for (int i = 0; i < Random.Range(minCountRooms, maxCountRooms); i++) {
            for (int j = 0; j < 3; j++) {
                if (i == Random.Range(0, 3)) {
                    List<Vector2> neighbors = Neighbors(i);
                    
                    Vector3 roomPosition = new Vector3((RoomSize.x + neighbors[0].x) / 2, 0, (RoomSize.y + neighbors[0].y) / 2);
                    Instantiate(rooms[Random.Range(0, rooms.Length)].RoomModel, roomPosition, Quaternion.identity);
                }
            }
        }
    }

    private List<Vector2> Neighbors(int axis) {
        List<Vector2> neighbors = new List<Vector2>();
        
        if (axis == 0) {
            neighbors.Add(new Vector2(15, 15));
        } else if (axis == 1) {
            neighbors.Add(new Vector2(-15, 15));
        } else if (axis == 2) {
            neighbors.Add(new Vector2(15, -15));
        } else if (axis == 3) {
            neighbors.Add(new Vector2(-15, -15));
        }
        
        return neighbors;
    }
}
