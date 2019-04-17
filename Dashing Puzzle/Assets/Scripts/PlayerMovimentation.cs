using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovimentation : MonoBehaviour
{   
    // quantidade de tiles que o player anda por vez - não faz nada por enquanto
    public int WalkSpaces = 2;

    private Tilemap Ground;
    private Tilemap Obstacles;
    private Tilemap Doors;
    private GameObject Enemies;
    private GameObject Spawn;

    private Vector3Int spawnCellPosition;
    private Vector3Int currentPlayerCellPosition;
    private TileBase currentPlayerTileBase;

    private Rigidbody2D playerRB;

    private bool playerDied = false;

    // Start is called before the first frame update
    void Start()
    {
        Ground = ChamberController.Instance.currentGroundTilemap;
        Obstacles = ChamberController.Instance.currentObstaclesTilemap;
        Doors = ChamberController.Instance.currentDoorTilemap;
        Enemies = ChamberController.Instance.currentEnemies;
        Spawn = ChamberController.Instance.currentSpawn;

        playerRB = this.GetComponent<Rigidbody2D>();

        // Inicia o jogador na posição de spawn
        spawnCellPosition = Ground.WorldToCell(Spawn.transform.position);
        this.transform.position = Ground.GetCellCenterWorld(spawnCellPosition);

        currentPlayerCellPosition = Ground.WorldToCell(this.transform.position);
        
        ChamberController.Instance.CheckIfCanOpenDoor();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

// Place enemy in spawn
public void RespawnPlayer() {
        this.transform.position = Ground.GetCellCenterWorld(spawnCellPosition);
        currentPlayerCellPosition = Ground.WorldToCell(this.transform.position);

        //reativa todos os inimigos mortos
        foreach (Transform DeadEnemy in Enemies.GetComponentInChildren<Transform>(true)) {

            if (!DeadEnemy.gameObject.activeSelf) {
                DeadEnemy.gameObject.SetActive(true);
            }
        }
    }

    private void PlayerMovement() {

        Vector3Int nextPosition = currentPlayerCellPosition;

        if (Input.GetKeyDown(KeyCode.W)) {
            nextPosition += new Vector3Int( 0, CanWalkSpaces(Vector3Int.up), 0);

        } else if (Input.GetKeyDown(KeyCode.S)) {
            nextPosition -= new Vector3Int( 0, CanWalkSpaces(Vector3Int.down), 0);

        } else if (Input.GetKeyDown(KeyCode.A)) {
            nextPosition -= new Vector3Int( CanWalkSpaces(Vector3Int.left), 0, 0);

        } else if (Input.GetKeyDown(KeyCode.D)) {
            nextPosition += new Vector3Int( CanWalkSpaces(Vector3Int.right), 0, 0);
        }

        if (!playerDied) {
            currentPlayerCellPosition = nextPosition;
            currentPlayerTileBase = Ground.GetTile(nextPosition);
            this.transform.position = Ground.GetCellCenterWorld(currentPlayerCellPosition);
        }
    }

    // Check how many spaces can he walk (work for 2 right now)
    // Return 2 if the next two spaces in the desire direction are free or if there's a enemy in the first tile and nothing on the second
    // Return 1 if the second tile there's an obstacle and nothing on the first one
    // return 0 if there's a enemy in the second tile or there's an obstacle in the second tile and an enemy on the first tile
    private int CanWalkSpaces(Vector3Int dir) {

        playerDied = false;

        if (Obstacles.HasTile(currentPlayerCellPosition + dir)) {                      // tem obstaculos 1 tiles a frente
            return 0;
        } else if (Obstacles.HasTile(currentPlayerCellPosition + dir + dir)) {         // tem obstaculos 1 tiles a frente

            foreach (Transform enemy in Enemies.GetComponentInChildren<Transform>()) {
                if (Ground.WorldToCell(enemy.position) == currentPlayerCellPosition + dir) {        // tem inimigo 1 tile a frente - dead

                    this.GetComponent<PlayerLifeControl>().KillPlayer();
                    playerDied = true;

                    return 0;
                }
            }

            if (Doors.HasTile(currentPlayerCellPosition + dir))
            {
                ChamberController.Instance.ChangeChamber();
                updateChamber(); 
                return 0;
            }

            return 1;

        } else {                                                                        // Não tem obstaculos a frente

            foreach (Transform enemy in Enemies.GetComponentInChildren<Transform>()) {
                if (Ground.WorldToCell(enemy.position) == currentPlayerCellPosition + dir) {        // tem inimigo 1 tile a frente - kill

                    enemy.gameObject.SetActive(false);
                    ChamberController.Instance.CheckIfCanOpenDoor();

                } else if (Ground.WorldToCell(enemy.position) == currentPlayerCellPosition + dir + dir) {   // tem inimigo 2 tiles a frente - dead

                    this.GetComponent<PlayerLifeControl>().KillPlayer();
                    playerDied = true;

                    return 0;
                }
            }

            if (Doors.HasTile(currentPlayerCellPosition + dir))
            {
                ChamberController.Instance.ChangeChamber();
                updateChamber();
                return 0;

            } else if (Doors.HasTile(currentPlayerCellPosition + dir + dir))
            {
                ChamberController.Instance.ChangeChamber();
                updateChamber();
                return 0;
            }

            return 2;
        }
    }

    private void updateChamber()
    {
        Ground = ChamberController.Instance.currentGroundTilemap;
        Obstacles = ChamberController.Instance.currentObstaclesTilemap;
        Doors = ChamberController.Instance.currentDoorTilemap;
        Enemies = ChamberController.Instance.currentEnemies;
        Spawn = ChamberController.Instance.currentSpawn;

        // Inicia o jogador na posição de spawn
        spawnCellPosition = Ground.WorldToCell(Spawn.transform.position);
        this.transform.position = Ground.GetCellCenterWorld(spawnCellPosition);
        currentPlayerCellPosition = Ground.WorldToCell(this.transform.position);

        ChamberController.Instance.CheckIfCanOpenDoor();
    }

}
