using UnityEngine;

public class RedPlayerMovement : MonoBehaviour
{
    public GameObject trail;
    public float spawnRate = 0.1f; // Controls how often trails spawn
    public float playerSpeed = 2;
    public Rigidbody2D myRigidBody;
    private Vector2 lastDirection;
    private Vector3 startPosition;
    private bool canSpawnTrail = true;

    public float minX = -10000f, maxX = 10000f, minY = -2000f, maxY = 2000f;

    void Start()
    {
        startPosition = transform.position;
        lastDirection = Vector2.right;
        myRigidBody.linearVelocity = lastDirection * playerSpeed;
        transform.rotation = Quaternion.Euler(0, 0, -90);

        // Immediately spawn the first trail
        SpawnTrail();

        // Continue spawning trails at regular intervals
        InvokeRepeating(nameof(SpawnTrail), spawnRate, spawnRate);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            lastDirection = Vector2.up;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            lastDirection = Vector2.right;
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lastDirection = Vector2.left;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            lastDirection = Vector2.down;
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        if (transform.position.x < minX || transform.position.x > maxX ||
            transform.position.y < minY || transform.position.y > maxY)
        {
            ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();
            Debug.Log("Collision detected with Red Player and trail! Resetting game...");
            GameObject[] trails = GameObject.FindGameObjectsWithTag("Trail");
            scoreManager.AddScore(1, 1);
            foreach (GameObject t in trails)
            {
                Destroy(t);
            }

            GameObject bluePlayerObject = GameObject.FindWithTag("BluePlayer");
            GameObject redPlayerObject = GameObject.FindWithTag("RedPlayer");


                bluePlayerObject.GetComponent<BluePlayerMovement>().ResetPosition();
            

                redPlayerObject.GetComponent<RedPlayerMovement>().ResetPosition();
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
            ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();
            Debug.Log("Collision detected with Red Player and trail! Resetting game...");
            GameObject[] trails = GameObject.FindGameObjectsWithTag("Trail");
            foreach (GameObject t in trails)
            {
                Destroy(t);
            }

            GameObject bluePlayerObject = GameObject.FindWithTag("BluePlayer");
            GameObject redPlayerObject = GameObject.FindWithTag("RedPlayer");


                bluePlayerObject.GetComponent<BluePlayerMovement>().ResetPosition();
            

                redPlayerObject.GetComponent<RedPlayerMovement>().ResetPosition();

            scoreManager.AddScore(1, 1);
        }
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
        lastDirection = Vector2.right;
        myRigidBody.linearVelocity = lastDirection * playerSpeed; // Ensures movement resets
        transform.rotation = Quaternion.Euler(0, 0, -90);
        canSpawnTrail = false;
        Invoke(nameof(EnableTrailSpawning), 0.5f); // Delay before spawning trails again
        SpawnTrail(); // Immediately spawn a trail after reset
    }

    void EnableTrailSpawning()
    {
        canSpawnTrail = true;
    }
}