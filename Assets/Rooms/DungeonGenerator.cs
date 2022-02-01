using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour {
    public class Cell {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Rule {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;

        public bool obligatory;

        public int ProbabilityOfSpawning(int x, int y) {
            if (x>= minPosition.x && x<=maxPosition.x && y >= minPosition.y && y <= maxPosition.y) {
                return obligatory ? 2 : 1;
            }
            return 0;
        }
    }

    public Vector2Int size;
    public int startPos = 0;
    public Rule[] rooms;
    public Vector2 offset;

    public int Level;
    
    public GameObject PlayerPrefab;

    List<Cell> board;

    void Start() {
        MazeGenerator();
        Instantiate(PlayerPrefab, new Vector3(0f, 1.527f, 0f), Quaternion.identity);
        Level = 0;
    }

    void GenerateDungeon() {
        for (int i = 0; i < size.x; i++) {
            for (int j = 0; j < size.y; j++) {
                Cell currentCell = board[(i + j * size.x)];
                
                if (currentCell.visited) {
                    var newRoom = Instantiate(rooms[Random.Range(0, rooms.Length)].room, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(currentCell.status);
                    newRoom.name += " " + i + "-" + j;
                }
            }
        }
    }

    void MazeGenerator() {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++) {
            for (int j = 0; j < size.y; j++) {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while (k<1000) {
            k++;
            board[currentCell].visited = true;

            if(currentCell == board.Count - 1) {
                break;
            }

            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0) {
                if (path.Count == 0) {
                    break;
                } else {
                    currentCell = path.Pop();
                }
            } else {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell) {
                    if (newCell - 1 == currentCell) {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    } else {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                } else {
                    if (newCell + 1 == currentCell) {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    } else {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }
        GenerateDungeon();
    }

    List<int> CheckNeighbors(int cell) {
        List<int> neighbors = new List<int>();

        if (cell - size.x >= 0 && !board[(cell-size.x)].visited) {
            neighbors.Add((cell - size.x));
        }

        if (cell + size.x < board.Count && !board[(cell + size.x)].visited) {
            neighbors.Add((cell + size.x));
        }

        if ((cell+1) % size.x != 0 && !board[(cell +1)].visited) {
            neighbors.Add((cell +1));
        }
        
        if (cell % size.x != 0 && !board[(cell - 1)].visited) {
            neighbors.Add((cell -1));
        }

        return neighbors;
    }

    public void NewLayer() {
        MazeGenerator();
        Level++;
        Instantiate(PlayerPrefab, new Vector3(0f, 1.6f, 0f), Quaternion.identity);
    }
}
