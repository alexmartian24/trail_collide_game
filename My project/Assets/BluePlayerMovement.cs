using UnityEngine;
using System.Collections;

public class BluePlayerMovement : MonoBehaviour
{
    public GameObject trail;
    public float spawnRate = 0.1f;
    public float playerSpeed = 2;
    private float start_speed;
    public Rigidbody2D myRigidBody;
    private Vector2 lastDirection;
    private Vector3 startPosition;
    private bool canSpawnTrail = true;
    public AudioSource death;
    public FuelBoostScript fuel;
    
    private bool isHandlingCollision = false; // 🔹 Prevent multiple calls

    public float minX = -10000f, maxX = 10000f, minY = -2000f, maxY = 2000f;

    void Start()
    {
        startPosition = transform.position;
        start_speed = playerSpeed;
        lastDirection = Vector2.right;
        myRigidBody.linearVelocity = lastDirection * playerSpeed;
        transform.rotation = Quaternion.Euler(0, 0, -90);

        SpawnTrail();
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
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            lastDirection = Vector2.left;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            lastDirection = Vector2.down;
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        if (transform.position.x < minX || transform.position.x > maxX ||
            transform.position.y < minY || transform.position.y > maxY)
        {
            if (!isHandlingCollision) StartCoroutine(HandleCollision());
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (fuel.boost())
            {
                playerSpeed = start_speed * 2;
                spawnRate = 0.2f;
            }
            else
            {
                playerSpeed = start_speed;
                spawnRate = 0.1f;
            }
        }
        else
        {
            playerSpeed = start_speed;
            spawnRate = 0.1f;
            fuel.boosting = false;
        }
    }

    void FixedUpdate()
    {
        myRigidBody.linearVelocity = lastDirection * playerSpeed;
    }

    void SpawnTrail()
    {
        if (!canSpawnTrail) return;

        Vector3 trailPosition = transform.position - (Vector3)(lastDirection * 1.0f);
        GameObject spawnedTrail = Instantiate(trail, trailPosition, transform.rotation);
        spawnedTrail.tag = "Trail";
        spawnedTrail.AddComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trail") && !isHandlingCollision) // 🔹 Prevent multiple triggers
        {
            StartCoroutine(HandleCollision());
        }
    }

    private IEnumerator HandleCollision()
    {
        if (isHandlingCollision) yield break; // 🔹 Exit if already handling collision
        isHandlingCollision = true; // 🔹 Lock function

        Debug.Log("Collision detected! Pausing game...");
        ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();

        // Stop game by pausing time
        Time.timeScale = 0;

        // Destroy all existing trails
        GameObject[] trails = GameObject.FindGameObjectsWithTag("Trail");
        foreach (GameObject t in trails)
        {
            Destroy(t);
        }

        GameObject bluePlayerObject = GameObject.FindWithTag("BluePlayer");
        GameObject redPlayerObject = GameObject.FindWithTag("RedPlayer");

        if (bluePlayerObject != null)
            bluePlayerObject.GetComponent<BluePlayerMovement>().ResetPosition();

        if (redPlayerObject != null)
            redPlayerObject.GetComponent<RedPlayerMovement>().ResetPosition();

        scoreManager.AddScore(2, 1);

        // Wait for 2 seconds before resuming game
        yield return new WaitForSecondsRealtime(1f);

        Debug.Log("Resuming game...");
        Time.timeScale = 1; // Resume game
        isHandlingCollision = false; // 🔹 Unlock function for next collision
    }

    public void ResetPosition()
    {
        death.Play();
        fuel.refuel();
        transform.position = startPosition;
        lastDirection = Vector2.right;
        myRigidBody.linearVelocity = lastDirection * playerSpeed;
        transform.rotation = Quaternion.Euler(0, 0, -90);
        canSpawnTrail = false;
        Invoke(nameof(EnableTrailSpawning), 0.5f);
        SpawnTrail();
    }

    void EnableTrailSpawning()
    {
        canSpawnTrail = true;
    }
}