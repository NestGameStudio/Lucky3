using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestMov : MonoBehaviour
{
    public Tilemap Ground;
    public GameObject Spawn;

    Vector3Int spawnCellPosition;
    Vector3Int currentPlayerCellPosition;
    TileBase currentPlayerPositionTileBase;

    Rigidbody2D playerRB;

    Vector3Int nextPosition;


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
        nextPosition = spawnCellPosition;

        if (Input.GetKeyDown(KeyCode.W))
        {          
            nextPosition += new Vector3Int(0, 2, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            nextPosition -= new Vector3Int(0, 2, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            nextPosition -= new Vector3Int(2, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            nextPosition += new Vector3Int(2, 0, 0);
        }

        // Checa se é um tile acessível para movimentação
        TileBase nextTile = Ground.GetTile(nextPosition);
        Debug.Log(nextTile + " KD");
        Debug.Log(nextPosition);

        currentPlayerCellPosition += nextPosition;
        currentPlayerPositionTileBase = nextTile;
        this.transform.position = Ground.GetCellCenterWorld(currentPlayerCellPosition);

        
    }

}
