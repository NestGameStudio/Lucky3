using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBehaviour : MonoBehaviour
{
    private Tilemap Ground;

    // Start is called before the first frame update
    void Start()
    {
        // Ajeita a posição dos inimigos pro centro da celula
        Ground = ChamberController.Instance.currentGroundTilemap;

        Vector3Int enemyPositionInCell = Ground.WorldToCell(this.transform.position);
        this.transform.position = Ground.GetCellCenterWorld(enemyPositionInCell);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
