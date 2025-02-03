using UnityEngine;

public class BluePlayerMovement : MonoBehaviour
{
    public GameObject trail;
    public float spawnRate = 2;
    private float timer = 0;
    public float playerSpeed = 2;
    public Rigidbody2D myRigidBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            myRigidBody.linearVelocityY = playerSpeed;
            myRigidBody.linearVelocityX = 0;
            transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            myRigidBody.linearVelocityY = 0;
            myRigidBody.linearVelocityX = playerSpeed;
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            myRigidBody.linearVelocityY = 0;
            myRigidBody.linearVelocityX = -playerSpeed;
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            myRigidBody.linearVelocityY = -playerSpeed;
            myRigidBody.linearVelocityX = 0;
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            spawnTrail();
        }
    }

    void spawnTrail()
    {
        Instantiate(trail, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
    }
}
