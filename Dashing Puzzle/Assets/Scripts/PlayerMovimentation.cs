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
    TileBase currentPlayerTileBase;

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
        PlayerMovement();
    }

    void PlayerMovement() {

        Vector3Int nextPosition = currentPlayerCellPosition;

        if (Input.GetKeyDown(KeyCode.W)) {
            nextPosition += new Vector3Int(0, 1, 0);
        } else if (Input.GetKeyDown(KeyCode.S)) {
            nextPosition -= new Vector3Int(0, 1, 0);
        } else if (Input.GetKeyDown(KeyCode.A)) {
            nextPosition -= new Vector3Int(1, 0, 0);
        } else if (Input.GetKeyDown(KeyCode.D)) {
            nextPosition += new Vector3Int(1, 0, 0);
        }

        if (Ground.HasTile(nextPosition)) {
            currentPlayerCellPosition = nextPosition;
            currentPlayerTileBase = Ground.GetTile(nextPosition);
            this.transform.position = Ground.GetCellCenterWorld(currentPlayerCellPosition);
        } else {
            Debug.Log("falsiane");
        }
    }

}
