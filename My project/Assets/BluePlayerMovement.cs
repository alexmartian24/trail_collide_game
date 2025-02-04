using UnityEngine;

public class BluePlayerMovement : MonoBehaviour
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
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveToNextCell(Vector2Int.up);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MoveToNextCell(Vector2Int.right);
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                MoveToNextCell(Vector2Int.left);
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else if (Input.GetKeyDown(KeyCode.S))
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
        GameObject newTrail = Instantiate(trail, spawnPosition, transform.rotation);
        
        if (this.CompareTag("BluePlayer"))
        {
            newTrail.tag = "BlueTrail";
        }
        else if (this.CompareTag("RedPlayer"))
        {
            newTrail.tag = "RedTrail";
        }

        Debug.Log("Spawned Trail Tag: " + newTrail.tag);
    }

    void OnTriggerEnter2D(Collider2D other)
{
    ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>(); // Ensure we find the score manager

    if (scoreManager != null)
    {
        // Both players will reset themselves
        BluePlayerMovement bluePlayer = FindFirstObjectByType<BluePlayerMovement>();
        RedPlayerMovement redPlayer = FindFirstObjectByType<RedPlayerMovement>();

        if (CompareTag("BluePlayer") && other.CompareTag("RedTrail"))
        {
            Debug.Log("Blue Player hit Red Trail! Red gets a point.");
            scoreManager.AddScore(2, 1);

            bluePlayer.ResetPlayers();
            redPlayer.ResetPlayers();
        }
        else if (CompareTag("RedPlayer") && other.CompareTag("BlueTrail"))
        {
            Debug.Log("Red Player hit Blue Trail! Blue gets a point.");
            scoreManager.AddScore(1, 1);

            bluePlayer.ResetPlayers();
            redPlayer.ResetPlayers();
        }
    }
    else
    {
        Debug.LogError("ScoreManager not found!");
    }
}

public void ResetPlayers()
{
    // Remove all trails from the scene
    GameObject[] blueTrails = GameObject.FindGameObjectsWithTag("BlueTrail");
    foreach (GameObject trail in blueTrails)
    {
        Destroy(trail);
    }

    GameObject[] redTrails = GameObject.FindGameObjectsWithTag("RedTrail");
    foreach (GameObject trail in redTrails)
    {
        Destroy(trail);
    }

    // Reset each player's position
    if (CompareTag("BluePlayer"))
    {
        currentGridPosition = new Vector2Int(3, 8); // Update grid position
        transform.position = GridManager.GridToWorldPosition(currentGridPosition); // Move to exact world position
    }
    else if (CompareTag("RedPlayer"))
    {
        currentGridPosition = new Vector2Int(13, 8); // Update grid position
        transform.position = GridManager.GridToWorldPosition(currentGridPosition); // Move to exact world position
    }

    // Fully stop movement
    myRigidBody.linearVelocity = Vector2.zero;
    myRigidBody.angularVelocity = 0f;
    myRigidBody.Sleep(); // Ensures Rigidbody2D resets all forces

    isMoving = false; // Ensure movement doesn't continue after reset

    // Reset rotation
    transform.rotation = Quaternion.identity;
}
}
