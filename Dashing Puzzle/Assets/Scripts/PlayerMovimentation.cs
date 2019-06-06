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
    private Vector3Int locationInfloorPlayerCellPosition;
    private TileBase currentPlayerTileBase;

    private Rigidbody2D playerRB;

    private bool playerDied = false;
    private bool playerChangedLevel = false;

    private Vector3Int nextPosition;
    private bool playerCanWalk = true;

    public ParticleSystem dashParticle;
    public ParticleSystem floorParticle;

    public Animator playerWalkAnimator;
    private GameObject playerGraphic;

    public AudioSource AudioDash;
    public AudioSource AudioEnemyDeath;
    public AudioSource AudioObstacle;

    public TrailRenderer DashTrail;
    public ParticleSystem DashParticle;

    private bool MoveEnemyOnce = false;
    private bool PlayParticleOnce = false;
    private bool FazUmaVezSoCarai = false;

    // Start is called before the first frame update
    void Start()
    { 
        Ground = ChamberController.Instance.currentGroundTilemap;
        Obstacles = ChamberController.Instance.currentObstaclesTilemap;
        Doors = ChamberController.Instance.currentDoorTilemap;
        Enemies = ChamberController.Instance.currentEnemies;
        Spawn = ChamberController.Instance.currentSpawn;

        playerRB = this.GetComponent<Rigidbody2D>();
        playerGraphic = this.transform.Find("Graphics").gameObject;

        // Inicia o jogador na posição de spawn
        spawnCellPosition = Ground.WorldToCell(Spawn.transform.position);
        this.transform.position = Ground.GetCellCenterWorld(spawnCellPosition);
        currentPlayerCellPosition = Ground.WorldToCell(this.transform.position);
        locationInfloorPlayerCellPosition = currentPlayerCellPosition;

        ChamberController.Instance.CheckIfCanOpenDoor();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.R)) {
            this.GetComponent<PlayerLifeControl>().KillPlayer();
        }
    }

    // Place enemy in spawn
    public void RespawnPlayerAfterDeath() {

        this.transform.position = Ground.GetCellCenterWorld(spawnCellPosition);
        currentPlayerCellPosition = Ground.WorldToCell(this.transform.position);

        playerDied = true;
        
        //reativa todos os inimigos mortos
        foreach (Transform DeadEnemy in Enemies.GetComponentInChildren<Transform>(true)) {

            if (!DeadEnemy.gameObject.activeSelf) {
                DeadEnemy.gameObject.SetActive(true);
            }

            if (DeadEnemy.GetComponent<EnemyBehaviour>().hasMovement) {
                if (DeadEnemy.GetComponent<EnemyBehaviour>().isInDesiredPosition)  {
                    DeadEnemy.GetComponent<EnemyBehaviour>().DoEnemyMovement();
                }

            }
        }
    }

    private void PlayerMovement() {

        if (Ground.WorldToCell(this.transform.position) == nextPosition || playerChangedLevel || playerDied)
        {
            playerCanWalk = true;
        }

        Vector3Int newPos = Vector3Int.zero;

        if (playerCanWalk)
        {
            nextPosition = currentPlayerCellPosition;

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                newPos = Vector3Int.up;
                nextPosition += new Vector3Int(0, CanWalkSpaces(Vector3Int.up), 0);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                newPos = Vector3Int.down;
                nextPosition -= new Vector3Int(0, CanWalkSpaces(Vector3Int.down), 0);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                newPos = Vector3Int.left;
                nextPosition -= new Vector3Int(CanWalkSpaces(Vector3Int.left), 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                newPos = Vector3Int.right;
                nextPosition += new Vector3Int(CanWalkSpaces(Vector3Int.right), 0, 0);
            }

            playerCanWalk = false;
        }

        // can walk - loop
        if (!playerDied && !playerChangedLevel) {

            // ele anda triga durante o caminho e no final quando termina de andar ele brilha onde tinha que ter brilhado antes
            if (PlayParticleOnce) {

                if (playerWalkAnimator)
                {
                    playerWalkAnimator.SetTrigger("Trigger");

                    SpriteRenderer spriteR = playerGraphic.GetComponent<SpriteRenderer>();

                    if (newPos == Vector3Int.left) {
                        spriteR.flipX = true;
                    } else if (newPos == Vector3Int.right) {
                        spriteR.flipX = false;
                    }
                }

                dashParticle.Play();
                if (AudioDash)
                    AudioDash.PlayOneShot(AudioDash.clip, AudioDash.volume);
                PlayParticleOnce = false;
            }

            // Move Enemy
            if (MoveEnemyOnce) { 
                foreach (Transform enemy in Enemies.transform) {
                    if (enemy.GetComponent<EnemyBehaviour>().hasMovement) {
                        enemy.GetComponent<EnemyBehaviour>().DoEnemyMovement();
                    }
                }
                MoveEnemyOnce = false;
            }

            // Move Player
            currentPlayerCellPosition = nextPosition;
            currentPlayerTileBase = Ground.GetTile(nextPosition);
            this.transform.position = Vector3.MoveTowards(this.transform.position, Ground.GetCellCenterWorld(currentPlayerCellPosition), Velocity * Time.deltaTime);

            if (FazUmaVezSoCarai)
            {
                if (!CheckIfEnemyKilledPlayer(Vector3.zero, true)) {
                    // tem que pegar a direção dessa desgraça
                    CheckIfPlayerKilledEnemy(newPos, true);
                }
                //CheckIfEnemyKilledPlayer(Vector3.zero, false);
                FazUmaVezSoCarai = false;
            }
           
        }
    }

    // Check how many spaces can he walk (work for 2 right now)
    // Return 2 if the next two spaces in the desire direction are free or if there's a enemy in the first tile and nothing on the second
    // Return 1 if the second tile there's an obstacle and nothing on the first one
    // return 0 if there's a enemy in the second tile or there's an obstacle in the second tile and an enemy on the first tile
    private int CanWalkSpaces(Vector3Int dir) {

        FazUmaVezSoCarai = true;
        PlayParticleOnce = true;
        MoveEnemyOnce = true;
        playerDied = false;
        playerChangedLevel = false;

        if (Obstacles.HasTile(currentPlayerCellPosition + dir)) {                      // tem obstaculos 1 tiles a frente
            if (AudioObstacle)
                AudioObstacle.Play();
            return 0;
        } else if (Obstacles.HasTile(currentPlayerCellPosition + dir + dir)) {         // tem obstaculos 1 tiles a frente

            /*if (CheckIfEnemyKilledPlayer(dir, false)) {   // tem inimigo 1 tile a frente - dead
                return 0;
            }*/

            if (Doors.HasTile(currentPlayerCellPosition + dir) && ChamberController.Instance.doorIsOpen)
            {
                if (ChamberController.Instance.doorIsOpen) {
                    ChamberController.Instance.ChangeChamber();
                    if (!ChamberController.Instance.WinGame)
                        updateChamber();
                    playerChangedLevel = true;
                }

                return 0;
            }

            if (AudioObstacle)
                AudioObstacle.Play();

            return 1;

        } else {                                                                        // Não tem obstaculos a frente

            if (CheckIfPlayerKilledEnemy(dir, false)) {   // tem inimigo 1 tile a frente - kill
                // Kill
                if(AudioEnemyDeath)
                    AudioEnemyDeath.PlayOneShot(AudioEnemyDeath.clip,AudioEnemyDeath.volume);
            } /*else if (CheckIfEnemyKilledPlayer(dir + dir, false))  {   // tem inimigo 1 tile a frente - dead
                return 0;
            }*/

            if (Doors.HasTile(currentPlayerCellPosition + dir))
            {
                if (ChamberController.Instance.doorIsOpen) {
                    ChamberController.Instance.ChangeChamber();
                    if (!ChamberController.Instance.WinGame)
                        updateChamber();
                    //StartCoroutine(DashGone());
                    playerChangedLevel = true;
                }

                return 0;

            } else if (Doors.HasTile(currentPlayerCellPosition + dir + dir))
            {
                if (ChamberController.Instance.doorIsOpen) {
                    ChamberController.Instance.ChangeChamber();
                    if (!ChamberController.Instance.WinGame)
                        updateChamber();
                    playerChangedLevel = true;

                }

                return 1;
            }

            return 2;
        }
    }

    private bool CheckIfEnemyKilledPlayer(Vector3 direction, bool movingEnemies)
    {
        foreach (Transform enemy in Enemies.GetComponentInChildren<Transform>())
        {
            if ((Ground.WorldToCell(enemy.position) == currentPlayerCellPosition + direction) && enemy.gameObject.activeSelf) {        // tem inimigo 1 tile a frente - dead
                if (!enemy.GetComponent<EnemyBehaviour>().hasMovement || (enemy.GetComponent<EnemyBehaviour>().hasMovement && movingEnemies)) {
                    StartCoroutine(WaitUntilKillPlayer());
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckIfPlayerKilledEnemy(Vector3Int direction, bool movingEnemies)
    {
        foreach (Transform enemy in Enemies.GetComponentInChildren<Transform>())
        {
            if ((Ground.WorldToCell(enemy.position) == currentPlayerCellPosition + direction) && enemy.gameObject.activeSelf && !enemy.GetComponent<EnemyBehaviour>().hasMovement && !movingEnemies) {        // tem inimigo 1 tile a frente - kill

                enemy.GetComponent<EnemyBehaviour>().InitiateEnemyDieAnimation();
                ChamberController.Instance.CheckIfCanOpenDoor();
                return true;

            } else if (enemy.gameObject.activeSelf && enemy.GetComponent<EnemyBehaviour>().hasMovement && movingEnemies)
            {
                if ((Ground.WorldToCell(enemy.position) == currentPlayerCellPosition - direction))
                {
                    enemy.GetComponent<EnemyBehaviour>().InitiateEnemyDieAnimation();
                    ChamberController.Instance.CheckIfCanOpenDoor();
                    return true;
                }
            }
        }

        return false;
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

    IEnumerator WaitUntilKillPlayer()
    {
        while (Ground.WorldToCell(this.transform.position) != currentPlayerCellPosition) {
            yield return new WaitForEndOfFrame();
        }
        ChamberController.instance.DeathCounter += 1;
        ChamberController.instance.DeathCounterText.text = "Death Counter: " + ChamberController.instance.DeathCounter;
        this.GetComponent<PlayerLifeControl>().KillPlayer();
    }
    IEnumerator DashGone() {
        DashParticle.Clear();
        DashTrail.enabled = false;
        yield return new WaitForSeconds(0.4f);
        DashTrail.enabled = true;
    }
}
