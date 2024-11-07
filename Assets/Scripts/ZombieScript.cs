using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    private Rigidbody2D rbody;
    public GameObject player;
    public Transform target;
    public Vector2 targetDirection;

    public int health;
    public int damage;
    public int speed;

    public LayerMask collisionLayerMask;
    public float rotationSpeed;

    public float collisionAvoidanceDistance;
    public MSManagerScript MSMScript;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();
        HandleHealth();
    }

    void HandleHealth()
    {
        if(health <= 0)
        {
            MSMScript.killedZombie();
            //play death animations/particle
            Destroy(gameObject);
        }
    }

    void MoveTowardsTarget()
    {
        // Determine direction towards the player
        Vector2 directionToPlayer = (target.position - transform.position).normalized;

        // Check for obstacles directly in the path to the player
        RaycastHit2D obstacleHit = Physics2D.Raycast(transform.position, directionToPlayer, collisionAvoidanceDistance, collisionLayerMask);

        if (obstacleHit.collider != null)
        {
            // If an obstacle is detected, find a perpendicular direction to avoid it
            Vector2 avoidanceDirection = Vector2.Perpendicular(obstacleHit.normal).normalized;

            // Adjust the movement direction to avoid the obstacle
            targetDirection = Vector2.Lerp(targetDirection, avoidanceDirection, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // If no obstacle is detected, move directly towards the player
            targetDirection = Vector2.Lerp(targetDirection, directionToPlayer, rotationSpeed * Time.deltaTime);
        }

        // Apply movement to the Rigidbody2D
        rbody.velocity = targetDirection * speed;

        // Rotate the enemy smoothly to face the direction of movement
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
    }
}
