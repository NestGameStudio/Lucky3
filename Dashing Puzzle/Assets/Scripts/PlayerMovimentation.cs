using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovimentation : MonoBehaviour
{
    public Tilemap Ground;
    public GameObject Spawn;

    Vector3Int spawnCellPosition;
    Vector3Int currentPlayerCellPosition;
    TileBase currentPlayerPositionTileBase;

    Rigidbody2D playerRB;

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
        // Movimentação
        if (Input.anyKeyDown)
        {
            PlayerMovement();
        }
    }


    // Movimentação
    void PlayerMovement()
    {
        Vector3Int nextPosition = currentPlayerCellPosition;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            nextPosition += new Vector3Int(0, 1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            nextPosition -= new Vector3Int(0, 1, 0);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nextPosition -= new Vector3Int(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextPosition += new Vector3Int(1, 0, 0);
        }

        // Checa se é um tile acessível para movimentação
        TileBase nextTile = Ground.GetTile(nextPosition);
        Debug.Log(nextTile + " KD");
        Debug.Log(nextPosition);

        if (Ground.HasTile(nextPosition))
        {
            Debug.Log("A");
            currentPlayerCellPosition += nextPosition;
            currentPlayerPositionTileBase = nextTile;
            this.transform.position = Ground.GetCellCenterWorld(currentPlayerCellPosition);
        }
        else
        {
            Debug.Log("falsiane");
        }
    }

}
