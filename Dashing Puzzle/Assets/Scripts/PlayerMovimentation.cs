using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovimentation : MonoBehaviour
{   
    // quantidade de tiles que o player anda por vez - não faz nada por enquanto
    public int WalkSpaces = 2;
    public float Velocity = 2;

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
    private bool playerChangedLevel = false;

    private Vector3Int nextPosition;
    private bool playerCanWalk = true;

    public ParticleSystem dashParticle;

    public AudioSource AudioDash;
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

        if (Input.GetKeyDown(KeyCode.R)) {
            RespawnPlayerAfterDeath();
        }
    }

    // Place enemy in spawn
    public void RespawnPlayerAfterDeath() {

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

        if (Ground.WorldToCell(this.transform.position) == nextPosition || playerChangedLevel || playerDied)
        {
            playerCanWalk = true;
        }

        if (playerCanWalk)
        {
            nextPosition = currentPlayerCellPosition;

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                nextPosition += new Vector3Int(0, CanWalkSpaces(Vector3Int.up), 0);
                dashParticle.Play();
                AudioDash.PlayOneShot(AudioDash.clip, AudioDash.volume);

            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                nextPosition -= new Vector3Int(0, CanWalkSpaces(Vector3Int.down), 0);
                dashParticle.Play();
                AudioDash.PlayOneShot(AudioDash.clip, AudioDash.volume);


            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                nextPosition -= new Vector3Int(CanWalkSpaces(Vector3Int.left), 0, 0);
                dashParticle.Play();
                AudioDash.PlayOneShot(AudioDash.clip, AudioDash.volume);


            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                nextPosition += new Vector3Int(CanWalkSpaces(Vector3Int.right), 0, 0);
                dashParticle.Play();
                AudioDash.PlayOneShot(AudioDash.clip,AudioDash.volume);

            }

            playerCanWalk = false;
        }


        // -------------------------------------------------------------------------------- Colocar animação do glow onde anda

        // can walk
        if (!playerDied && !playerChangedLevel) {

            currentPlayerCellPosition = nextPosition;
            currentPlayerTileBase = Ground.GetTile(nextPosition);
            this.transform.position = Vector3.MoveTowards(this.transform.position, Ground.GetCellCenterWorld(currentPlayerCellPosition), Velocity * Time.deltaTime);
        }
    }

    // Check how many spaces can he walk (work for 2 right now)
    // Return 2 if the next two spaces in the desire direction are free or if there's a enemy in the first tile and nothing on the second
    // Return 1 if the second tile there's an obstacle and nothing on the first one
    // return 0 if there's a enemy in the second tile or there's an obstacle in the second tile and an enemy on the first tile
    private int CanWalkSpaces(Vector3Int dir) {


        playerDied = false;
        playerChangedLevel = false;

        if (Obstacles.HasTile(currentPlayerCellPosition + dir)) {                      // tem obstaculos 1 tiles a frente
            return 0;
        } else if (Obstacles.HasTile(currentPlayerCellPosition + dir + dir)) {         // tem obstaculos 1 tiles a frente

            foreach (Transform enemy in Enemies.GetComponentInChildren<Transform>()) {
                if ((Ground.WorldToCell(enemy.position) == currentPlayerCellPosition + dir) && enemy.gameObject.activeSelf) {        // tem inimigo 1 tile a frente - dead

                    this.GetComponent<PlayerLifeControl>().KillPlayer();
                    playerDied = true;

                    return 0;
                }
            }

            if (Doors.HasTile(currentPlayerCellPosition + dir) && ChamberController.Instance.doorIsOpen)
            {
                if (ChamberController.Instance.doorIsOpen) {
                    ChamberController.Instance.ChangeChamber();
                    updateChamber();
                    playerChangedLevel = true;
                }

                return 0;
            }

            return 1;

        } else {                                                                        // Não tem obstaculos a frente

            foreach (Transform enemy in Enemies.GetComponentInChildren<Transform>()) {
                if ((Ground.WorldToCell(enemy.position) == currentPlayerCellPosition + dir) && enemy.gameObject.activeSelf) {        // tem inimigo 1 tile a frente - kill

                    enemy.gameObject.SetActive(false);
                    ChamberController.Instance.CheckIfCanOpenDoor();

                } else if ((Ground.WorldToCell(enemy.position) == currentPlayerCellPosition + dir + dir) && enemy.gameObject.activeSelf) {   // tem inimigo 2 tiles a frente - dead

                    this.GetComponent<PlayerLifeControl>().KillPlayer();
                    playerDied = true;

                    return 0;
                }
            }

            if (Doors.HasTile(currentPlayerCellPosition + dir))
            {
                if (ChamberController.Instance.doorIsOpen) {
                    ChamberController.Instance.ChangeChamber();
                    updateChamber();
                    playerChangedLevel = true;
                }

                return 0;

            } else if (Doors.HasTile(currentPlayerCellPosition + dir + dir))
            {
                if (ChamberController.Instance.doorIsOpen) {
                    ChamberController.Instance.ChangeChamber();
                    updateChamber();
                    playerChangedLevel = true;

                }

                return 1;
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
