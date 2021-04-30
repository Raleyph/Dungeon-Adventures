using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomPlacer : MonoBehaviour {
    public Room[] RoomPrefabs;
    public Room StartingRoom;
    public int layer;

    public Room[,] spawnedRooms;

    public GameObject PlayerPrefab;

    private void Start()
    {
        SetRooms();
    }

    public void SetRooms()
    {
        Instantiate(StartingRoom, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(PlayerPrefab, new Vector3(0f, 1.6f, 0f), Quaternion.identity);
        layer = 1;
        spawnedRooms = new Room[15, 15];
        spawnedRooms[5, 5] = StartingRoom;

        for (int i = 0; i < 25; i++)
        {
            PlaceOneRoom();
        }
    }

    public void PlaceOneRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        vacantPlaces.Clear();
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }

        Room newRoom = Instantiate(GetRandomChunk());

        int limit = 700;
        while (limit-- > 0)
        {
            Vector2Int position = vacantPlaces.ElementAt(UnityEngine.Random.Range(0, vacantPlaces.Count));
            newRoom.RotateRandomly();

            if (ConnectToSomething(newRoom, position))
            {
                newRoom.transform.position = new Vector3(position.x - 5, 0, position.y - 5) * 15;
                spawnedRooms[position.x, position.y] = newRoom;
                return;
            }
        }

        Destroy(newRoom.gameObject);
    }

    private bool ConnectToSomething(Room room, Vector2Int p)
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (room.DoorU != null && p.y < maxY && spawnedRooms[p.x, p.y + 1]?.DoorD != null) neighbours.Add(Vector2Int.up);
        if (room.DoorD != null && p.y > 0 && spawnedRooms[p.x, p.y - 1]?.DoorU != null) neighbours.Add(Vector2Int.down);
        if (room.DoorR != null && p.x < maxX && spawnedRooms[p.x + 1, p.y]?.DoorL != null) neighbours.Add(Vector2Int.right);
        if (room.DoorL != null && p.x > 0 && spawnedRooms[p.x - 1, p.y]?.DoorR != null) neighbours.Add(Vector2Int.left);

        if (neighbours.Count == 0) return false;

        Vector2Int selectedDirection = neighbours[UnityEngine.Random.Range(0, neighbours.Count)];
        Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];

        if(selectedDirection == Vector2Int.up)
        {
            room.DoorU.SetActive(false);
            selectedRoom.DoorD.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.down)
        {
            room.DoorD.SetActive(false);
            selectedRoom.DoorU.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.right)
        {
            room.DoorR.SetActive(false);
            selectedRoom.DoorL.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.left)
        {
            room.DoorL.SetActive(false);
            selectedRoom.DoorR.SetActive(false);
        }

        return true;
    }
    
    public void StartRefreshNewLayer()
    {
        StartCoroutine(FuckingSetRooms());
    }

    public void StopRefreshNewLayer()
    {
        StopCoroutine(FuckingSetRooms());
    }

    public IEnumerator FuckingSetRooms()
    {
        var obj = GameObject.FindGameObjectsWithTag("Room");

        for (int i = 0; i < obj.Length; i++) {
            Destroy(obj[i]);
        }

        yield return new WaitForSeconds(0.2f);
        SetRooms();
        layer++;
        StopRefreshNewLayer();
    }

    private Room GetRandomChunk()
    {
        List<float> chances = new List<float>();
        for (int i = 0; i < RoomPrefabs.Length; i++)
        {
            chances.Add(RoomPrefabs[i].SpawnChange.Evaluate(layer));
        }

        float value = Random.Range(0, chances.Sum());
        float sum = 0;
        
        for (int i = 0; i < chances.Count; i++)
        {
            sum += chances[i];
            if (value < sum)
            {
                return RoomPrefabs[i];
            }
        }
        
        return RoomPrefabs[RoomPrefabs.Length-1];
    }
}
