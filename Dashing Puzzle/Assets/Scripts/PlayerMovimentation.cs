﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovimentation : MonoBehaviour
{
    // No futuro criar um array de grids para cada nivel, talvez em um script diferente
    public Grid TilemapGrid;
    public Tilemap Ground;
    public Tilemap Obstaculos;
    public Tilemap Enemies;

    public GameObject Spawn;

    // quantidade de tiles que o player anda por vez - não faz nada por enquanto
    public int WalkSpaces = 2;

    private Vector3Int spawnCellPosition;
    private Vector3Int currentPlayerCellPosition;
    private TileBase currentPlayerTileBase;

    private Rigidbody2D playerRB;


    // --------------------------- To do ------------------------
    // Criar um script com as grid que separa a grid atual
    // Inimigos morrerem ou matarem
    // Portinha




    // Start is called before the first frame update
    void Start()
    {
        playerRB = this.GetComponent<Rigidbody2D>();

        // Inicia o jogador na posição de spawn
        spawnCellPosition = Ground.WorldToCell(Spawn.transform.position);
        this.transform.position = Ground.GetCellCenterWorld(spawnCellPosition);

        currentPlayerCellPosition = Ground.WorldToCell(this.transform.position);     
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    // Place enemy in spawn
    public void RespawnPlayer() {
        this.transform.position = Ground.GetCellCenterWorld(spawnCellPosition);
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

        // Check if tile is possible to walk and do the walk
        if (Ground.HasTile(nextPosition)) {
            currentPlayerCellPosition = nextPosition;
            currentPlayerTileBase = Ground.GetTile(nextPosition);
            this.transform.position = Ground.GetCellCenterWorld(currentPlayerCellPosition);
        } else {
            Debug.Log("falsiane");
        }
    }

    // Check how many spaces can he walk (work for 2 right now)
    // Return 2 if the next two spaces in the desire direction are free or if there's a enemy in the first tile and nothing on the second
    // Return 1 if the second tile there's an obstacle and nothing on the first one
    // return 0 if there's a enemy in the second tile or there's an obstacle in the second tile and an enemy on the first tile
    private int CanWalkSpaces(Vector3Int dir) {

        if (Obstaculos.HasTile(currentPlayerCellPosition + dir)) {        // tem obstaculos 2 tiles a frente
            //if (Enemies.HasTile(currentPlayerCellPosition + dir)) {                 // tem inimigo 1 tile a frente - dead
                // kill player
               // this.GetComponent<PlayerLifeControl>().KillPlayer();
                //return 0;
            //} else {
                return 0;
           // }
        } else if (Obstaculos.HasTile(currentPlayerCellPosition + dir + dir)) {       // tem obstaculos 1 tiles a frente
            return 1;
        } else {                                                                // Não tem obstaculos a frente
            return 2;
        }

    }

    // mata 2 inimigos mas 1 mata ele, reinicia todos
}
