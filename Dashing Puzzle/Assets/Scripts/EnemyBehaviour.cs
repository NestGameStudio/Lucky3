using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum EnemyMovementDirection { None, Right, Left, Up, Down };

public class EnemyBehaviour : MonoBehaviour
{
    public bool hasMovement = false;
    public EnemyMovementDirection Orientation;
    public float Velocity = 30f;

    private Tilemap Ground;
    private Vector3Int currentEnemyPositionInCell;

    public Animator enemyDieAnimator;

    private Vector3Int nextPosition;
    [HideInInspector] public bool isInDesiredPosition = false;

    // Start is called before the first frame update
    void Start()
    {

        enemyDieAnimator = this.GetComponent<Animator>();

        foreach (Chambers chamber in ChamberController.Instance.ChambersInGame) {

            Tilemap levelGround = chamber.ChamberGrid.transform.Find("Tilemap-Ground").GetComponent<Tilemap>();

            // Adjust the enemy in the Ground position relative to it's level
            foreach (Transform enemy in chamber.Enemies.transform) {

                if (enemy == this.transform)
                {
                    currentEnemyPositionInCell = levelGround.WorldToCell(this.transform.position);
                    this.transform.position = levelGround.GetCellCenterWorld(currentEnemyPositionInCell);
                    Ground = levelGround;
                    break;
                }
            }
        }

    }

    public void InitiateEnemyDieAnimation()
    {
        enemyDieAnimator.SetTrigger("Death");
        this.gameObject.SetActive(false);
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
            if (this.isActiveAndEnabled) {
                StartCoroutine(MoveEnemy());
            }
        }

    }

    IEnumerator MoveEnemy()
    {
        while (this.transform.position != Ground.GetCellCenterWorld(currentEnemyPositionInCell))
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, Ground.GetCellCenterWorld(currentEnemyPositionInCell), Velocity * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

}
