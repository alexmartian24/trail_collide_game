using UnityEngine;

public class RedPlayerMovement : MonoBehaviour
{
    public GameObject trail;
    public float playerSpeed = 2;
    public Rigidbody2D myRigidBody;
    private Vector3 lastTrailPosition;
    public float trailSpacing = 1f; 
    private Vector2Int currentGridPosition;
    private bool isMoving = false;
    
    void Start()
    {
        lastTrailPosition = transform.position;
        SnapToGrid();
        spawnTrail();
    }

    void SnapToGrid()
    {
        Vector2Int gridPos = GridManager.WorldToGridPosition(transform.position);
        transform.position = GridManager.GridToWorldPosition(gridPos);
        currentGridPosition = gridPos;
        myRigidBody.linearVelocity = Vector2.zero;
        isMoving = false;
    }

    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveToNextCell(Vector2Int.up);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveToNextCell(Vector2Int.right);
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveToNextCell(Vector2Int.left);
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveToNextCell(Vector2Int.down);
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
        else
        {
            Vector3 targetPos = GridManager.GridToWorldPosition(currentGridPosition);
            if (Vector2.Distance(myRigidBody.position, targetPos) < 0.1f)
            {
                SnapToGrid();
            }
        }

        if (isMoving)
        {
            float distanceMoved = Vector3.Distance(transform.position, lastTrailPosition);
            if (distanceMoved >= trailSpacing)
            {
                spawnTrail();
                lastTrailPosition = transform.position;
            }
        }
    }

    void MoveToNextCell(Vector2Int direction)
    {
        Vector2Int targetPos = currentGridPosition + direction;
        
        if (targetPos.x >= 0 && targetPos.x < GridManager.GRID_SIZE &&
            targetPos.y >= 0 && targetPos.y < GridManager.GRID_SIZE)
        {
            currentGridPosition = targetPos;
            Vector3 worldDirection = GridManager.GridToWorldPosition(targetPos) - transform.position;
            worldDirection.Normalize();
            myRigidBody.linearVelocity = worldDirection * playerSpeed;
            isMoving = true;
        }
    }

    void spawnTrail()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, 1);
        Instantiate(trail, spawnPosition, transform.rotation);
    }
}
