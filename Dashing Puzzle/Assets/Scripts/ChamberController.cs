﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ChamberController : MonoBehaviour
{
    public Chambers[] ChambersInGame;

    public static ChamberController instance;

    [Space (10)]
    public TileBase OpenDoorAssetLeft;
    public TileBase OpenDoorAssetRight;
    public TileBase OpenDoorAssetUp;
    public TileBase OpenDoorAssetDown;
    public GameObject Cam;
    public Text LevelText;
    public Text DeathCounterText;

    [HideInInspector] public bool doorIsOpen = false;
    [HideInInspector] public int currentChamberNumber = 0;
    [HideInInspector] public Tilemap currentGroundTilemap;
    [HideInInspector] public Tilemap currentObstaclesTilemap;
    [HideInInspector] public Tilemap currentDoorTilemap;
    [HideInInspector] public GameObject currentEnemies;
    [HideInInspector] public GameObject currentSpawn;
    [HideInInspector] public GameObject currentCamera;

    private List<Tilemap> GroundTilemaps = new List<Tilemap>();
    private List<Tilemap> ObstaclesTilemaps = new List<Tilemap>();
    private List<Tilemap> DoorsTilemaps = new List<Tilemap>();
    private List<GameObject> Enemies = new List<GameObject>();
    private List<GameObject> Spawns = new List<GameObject>();
    private List<GameObject> Camera = new List<GameObject>();

    private AudioSource AudioOpenDoor;
    private AudioSource AudioChangeLevel;
    public ParticleSystem OpenDoorAnim;
    public ParticleSystem OpenDoorAnim2;
    public ParticleSystem OpenDoorAnim3;
    public ParticleSystem OpenDoorAnim4;

    [HideInInspector] public int DeathCounter = 0;

    private bool JumpAllLevels = false;

    [HideInInspector] public bool WinGame = false; 


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
        JumpAllLevels = false;
        currentChamberNumber = 0;
        WinGame = false;

        // if ChambersInGame == 0 -> está na tela de menu
        if (ChambersInGame.Length != 0)
        {
            for (int i = 0; i < ChambersInGame.Length; i++)
            {
                GroundTilemaps.Add(ChambersInGame[i].ChamberGrid.transform.Find("Tilemap-Ground").GetComponent<Tilemap>());
                ObstaclesTilemaps.Add(ChambersInGame[i].ChamberGrid.transform.Find("Tilemap-Obstacles").GetComponent<Tilemap>());
                DoorsTilemaps.Add(ChambersInGame[i].ChamberGrid.transform.Find("Tilemap-Doors").GetComponent<Tilemap>());
                Enemies.Add(ChambersInGame[i].Enemies);
                Spawns.Add(ChambersInGame[i].Spawn);
                Camera.Add(ChambersInGame[i].Camera);

            }

            currentGroundTilemap = GroundTilemaps[currentChamberNumber];
            currentObstaclesTilemap = ObstaclesTilemaps[currentChamberNumber];
            currentDoorTilemap = DoorsTilemaps[currentChamberNumber];
            currentEnemies = Enemies[currentChamberNumber];
            currentSpawn = Spawns[currentChamberNumber];
            currentCamera = Camera[currentChamberNumber];

            LevelText.text = "Level " + (currentChamberNumber + 1) + "/ " + (ChambersInGame.Length);
            DeathCounterText.text = "Death Counter: " + DeathCounter;
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            JumpAllLevels = true;
        }
    }

    // Troca a camera ativa, posiciona o player no próximo spawn
    // Muda todo o current Chamber

    public void ChangeChamber()
    {
        // Atualiza o current Chamber
        if (!JumpAllLevels)
        {
            currentChamberNumber += 1;
        }
        else
        {
            currentChamberNumber = ChambersInGame.Length - 2;
            JumpAllLevels = false;
        }

        if (currentChamberNumber + 1 > ChambersInGame.Length)
        {
            SceneController.Instance.LoadWinScene();
            WinGame = true;
            return;
        }

        currentGroundTilemap = GroundTilemaps[currentChamberNumber];
        currentObstaclesTilemap = ObstaclesTilemaps[currentChamberNumber];
        currentDoorTilemap = DoorsTilemaps[currentChamberNumber];
        currentEnemies = Enemies[currentChamberNumber];
        currentSpawn = Spawns[currentChamberNumber];
        currentCamera = Camera[currentChamberNumber];

        Cam.transform.position = currentCamera.transform.position;

        AudioOpenDoor = GameObject.Find("Change Level Audio").GetComponent<AudioSource>();
        AudioOpenDoor.PlayOneShot(AudioOpenDoor.clip,AudioOpenDoor.volume);

        LevelText.text = "Level " + (currentChamberNumber + 1) + "/ " + (ChambersInGame.Length);

        // coloca o pin de posicão do player
        //TimeRushController.Instance.PlayerCompletionPosition();
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

        doorIsOpen = canOpenDoor;
    }

    private void OpenDoor() {

        // Abre as portas que estavam fechadas 

        for (int n = DoorsTilemaps[currentChamberNumber].cellBounds.xMin; n < DoorsTilemaps[currentChamberNumber].cellBounds.xMax; n++)
        {
            for (int p = DoorsTilemaps[currentChamberNumber].cellBounds.yMin; p < DoorsTilemaps[currentChamberNumber].cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, 0));

                if (DoorsTilemaps[currentChamberNumber].HasTile(localPlace))
                {
                    OpenDoorAnim.transform.position = DoorsTilemaps[currentChamberNumber].GetCellCenterWorld(localPlace);
                    OpenDoorAnim.Play();

                    OpenDoorAnim2.transform.position = DoorsTilemaps[currentChamberNumber].GetCellCenterWorld(localPlace);
                    OpenDoorAnim2.Play();

                    OpenDoorAnim3.transform.position = DoorsTilemaps[currentChamberNumber].GetCellCenterWorld(localPlace);
                    OpenDoorAnim3.Play();

                    OpenDoorAnim4.transform.position = DoorsTilemaps[currentChamberNumber].GetCellCenterWorld(localPlace);
                    OpenDoorAnim4.Play();

                    AudioOpenDoor = GameObject.Find("Open Door Audio").GetComponent<AudioSource>();
                    AudioOpenDoor.PlayOneShot(AudioOpenDoor.clip,AudioOpenDoor.volume);
            if (DoorsTilemaps[currentChamberNumber].GetTile(localPlace).name == "Tiles-Porta-Fechado-1") {  // esquerda
                        DoorsTilemaps[currentChamberNumber].SetTile(localPlace, OpenDoorAssetLeft);
                    } else if (DoorsTilemaps[currentChamberNumber].GetTile(localPlace).name == "Tiles-Porta-Fechado-3") {   // direita
                        DoorsTilemaps[currentChamberNumber].SetTile(localPlace, OpenDoorAssetRight);
                    } else if (DoorsTilemaps[currentChamberNumber].GetTile(localPlace).name == "Tiles-Porta-Fechado-2") {   // cima 
                        DoorsTilemaps[currentChamberNumber].SetTile(localPlace, OpenDoorAssetUp);
                    } else if (DoorsTilemaps[currentChamberNumber].GetTile(localPlace).name == "Tiles-Porta-Fechado-4") {   // baixo
                        DoorsTilemaps[currentChamberNumber].SetTile(localPlace, OpenDoorAssetDown);
                    }
                }
            }
        }

    }

}
