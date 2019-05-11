using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum EnemyMovementDirection { None, Right, Left, Up, Down };

public class EnemyBehaviour : MonoBehaviour
{
    public bool hasMovement = false;
    public EnemyMovementDirection Orientation;
    public float Velocity;

    private Tilemap Ground;
    private Vector3Int currentEnemyPositionInCell;

    private Vector3Int nextPosition;
    private bool isInDesiredPosition = false;

    // Start is called before the first frame update
    void Start()
    {
        // Ajeita a posição dos inimigos pro centro da celula
        Ground = ChamberController.Instance.currentGroundTilemap;

        currentEnemyPositionInCell = Ground.WorldToCell(this.transform.position);
        //this.transform.position = Ground.GetCellCenterLocal(currentEnemyPositionInCell);
        this.transform.position = Ground.GetCellCenterWorld(currentEnemyPositionInCell);
    }


    public void DoEnemyMovement()
    {
        if (hasMovement)
        {
            Vector3Int direction = Vector3Int.zero;
            nextPosition = currentEnemyPositionInCell;

            switch (Orientation)
            {
                case EnemyMovementDirection.Right:
                    direction = Vector3Int.right;
                    break;
                case EnemyMovementDirection.Left:
                    direction = Vector3Int.left;
                    break;
                case EnemyMovementDirection.Up:
                    direction = Vector3Int.up;
                    break;
                case EnemyMovementDirection.Down:
                    direction = Vector3Int.down;
                    break;
                default:
                    Debug.Log("Nenhuma direção indicada na movimentação do inimigo");
                    break;
            }

            // Por enquanto não está detectando se tem colisões com as paredes ou não

            if (!isInDesiredPosition) {
                nextPosition += direction;
                isInDesiredPosition = true;
            } else {
                nextPosition -= direction;
                isInDesiredPosition = false;
            }

            currentEnemyPositionInCell = nextPosition;
            this.transform.position = Vector3.MoveTowards(this.transform.position, Ground.GetCellCenterWorld(currentEnemyPositionInCell), Velocity * Time.deltaTime);

        }

        Debug.Log("Move bichin");
    }

}
