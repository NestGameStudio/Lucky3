using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChamberController : MonoBehaviour
{
    public Chambers[] ChambersInGame;

    public static ChamberController instance;

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

    // Update is called once per frame
    void Update()
    {
        
    }

}
