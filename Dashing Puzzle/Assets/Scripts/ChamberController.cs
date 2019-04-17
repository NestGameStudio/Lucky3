using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChamberController : MonoBehaviour
{
    public Chambers[] ChambersInGame;

    public static ChamberController instance;

    public TileBase OpenDoorAsset;

    public GameObject[] chamberCameras;

    [HideInInspector] public Tilemap currentGroundTilemap;
    [HideInInspector] public Tilemap currentObstaclesTilemap;
    [HideInInspector] public Tilemap currentDoorTilemap;
    [HideInInspector] public GameObject currentEnemies;
    [HideInInspector] public GameObject currentSpawn;

    private List<Tilemap> GroundTilemaps = new List<Tilemap>();
    private List<Tilemap> ObstaclesTilemaps = new List<Tilemap>();
    private List<Tilemap> DoorsTilemaps = new List<Tilemap>();
    private List<GameObject> Enemies = new List<GameObject>();
    private List<GameObject> Spawns = new List<GameObject>();

    private int currentChamberNumber = 0;

    public static ChamberController Instance { get { return instance; } }

    private void Awake() {

        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i < ChambersInGame.Length; i++) {

            GroundTilemaps.Add(ChambersInGame[i].ChamberGrid.transform.Find("Tilemap-Ground").GetComponent<Tilemap>());
            ObstaclesTilemaps.Add(ChambersInGame[i].ChamberGrid.transform.Find("Tilemap-Obstacles").GetComponent<Tilemap>());
            DoorsTilemaps.Add(ChambersInGame[i].ChamberGrid.transform.Find("Tilemap-Doors").GetComponent<Tilemap>());
            Enemies.Add(ChambersInGame[i].Enemies);
            Spawns.Add(ChambersInGame[i].Spawn);
        }

        currentGroundTilemap = GroundTilemaps[currentChamberNumber];
        currentObstaclesTilemap = ObstaclesTilemaps[currentChamberNumber];
        currentDoorTilemap = DoorsTilemaps[currentChamberNumber];
        currentEnemies = Enemies[currentChamberNumber];
        currentSpawn = Spawns[currentChamberNumber];

    }

    // Troca a camera ativa, posiciona o player no próximo spawn
    // Muda todo o current Chamber

    public void ChangeChamber()
    {
        Debug.Log("Passou pela portinha");

        // Atualiza o current Chamber
        currentChamberNumber += 1;

        currentGroundTilemap = GroundTilemaps[currentChamberNumber];
        currentObstaclesTilemap = ObstaclesTilemaps[currentChamberNumber];
        currentDoorTilemap = DoorsTilemaps[currentChamberNumber];
        currentEnemies = Enemies[currentChamberNumber];
        currentSpawn = Spawns[currentChamberNumber];

        //NextCamera.Instance.change = true;

    }

    // Chamando toda vez que se mata um inimigo
    public void CheckIfCanOpenDoor() {

        bool canOpenDoor = true;

        foreach (Transform enemy in currentEnemies.GetComponentInChildren<Transform>()) {

            if (enemy.gameObject.activeSelf) {
                canOpenDoor = false;
                break;
            }
        }

        if (canOpenDoor) {
            OpenDoor();
        }

    }

    private void OpenDoor() {

        // Abre as portas que estavam fechadas

        foreach (TileBase tile in DoorsTilemaps[currentChamberNumber].GetTilesBlock(DoorsTilemaps[currentChamberNumber].cellBounds))
        {

            //DoorsTilemaps[currentChamberNumber].SetTile(tile)
            Debug.Log(tile + "RPG");
        }
        Debug.Log("Abriu a porta");
    }

}
