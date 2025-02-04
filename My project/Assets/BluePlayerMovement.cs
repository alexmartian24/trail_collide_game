using UnityEngine;

public class BluePlayerMovement : MonoBehaviour
{
    public GameObject trail;
    public float spawnRate = 0.1f; // Controls how often trails spawn
    public float playerSpeed = 2;
    public Rigidbody2D myRigidBody;
    private Vector2 lastDirection;
    private Vector3 startPosition;
    private bool canSpawnTrail = true;

    void Start()
    {
        startPosition = transform.position;
        lastDirection = Vector2.up;
        myRigidBody.linearVelocity = lastDirection * playerSpeed;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Immediately spawn the first trail
        SpawnTrail();

        // Continue spawning trails at regular intervals
        InvokeRepeating(nameof(SpawnTrail), spawnRate, spawnRate);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            lastDirection = Vector2.up;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            lastDirection = Vector2.right;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            lastDirection = Vector2.left;
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            lastDirection = Vector2.down;
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    void FixedUpdate()
    {
        myRigidBody.linearVelocity = lastDirection * playerSpeed;
    }

    void SpawnTrail()
    {
        if (!canSpawnTrail) return; // Ensure trails only spawn when allowed

        // Offset the trail so it doesn't spawn on the player
        Vector3 trailPosition = transform.position - (Vector3)(lastDirection * 1.0f);
        GameObject spawnedTrail = Instantiate(trail, trailPosition, transform.rotation);
        
        spawnedTrail.tag = "Trail"; 
        spawnedTrail.AddComponent<BoxCollider2D>(); // Ensure trail has a collider
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trail"))
        {
            Debug.Log("Collision detected with trail! Resetting game...");
            ResetGame();
        }
    }

    void ResetGame()
    {
        GameObject[] trails = GameObject.FindGameObjectsWithTag("Trail");
        foreach (GameObject t in trails)
        {
            Destroy(t);
        }

        transform.position = startPosition;
        lastDirection = Vector2.up;
        myRigidBody.linearVelocity = lastDirection * playerSpeed;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        canSpawnTrail = false;

        // Ensure trails spawn correctly after reset
        Invoke(nameof(EnableTrailSpawning), 0.5f);
        SpawnTrail(); // Spawn a trail immediately after reset
    }

    void EnableTrailSpawning()
    {
        canSpawnTrail = true;
    }
}