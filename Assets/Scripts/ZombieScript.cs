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
    public ParticleSystem ps;

    public int health;
    public int damage;
    public int speed;

    public LayerMask collisionLayerMask;
    public float rotationSpeed;

    public float collisionAvoidanceDistance;
    public MSManagerScript MSMScript;

    public float hitCooldown;
    private float lastHitTime;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
        MSMScript = FindAnyObjectByType<MSManagerScript>();
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
            Instantiate(ps, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void MoveTowardsTarget()
{
    Vector2 directionToPlayer = (target.position - transform.position).normalized;
    //float bestAngle = 0f;
    float closestDistance = Mathf.Infinity;
    Vector2 bestDirection = directionToPlayer;

    // Perform multiple raycasts around the direction towards the player
    int numRaycasts = 8;  // Increase this for more precision
    float angleIncrement = 360f / numRaycasts;

    for (int i = 0; i < numRaycasts; i++)
    {
        float angle = i * angleIncrement;
        Vector2 rayDirection = RotateVector(directionToPlayer, angle);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, collisionAvoidanceDistance, collisionLayerMask);

        if (hit.collider == null)
        {
            // If no obstacle, measure distance to player
            float distanceToPlayer = Vector2.Distance(transform.position + (Vector3)rayDirection, target.position);

            // Keep track of the closest path
            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                bestDirection = rayDirection;
            }
        }
    }

    // Move in the best direction found
    targetDirection = Vector2.Lerp(targetDirection, bestDirection, rotationSpeed * Time.deltaTime);
    rbody.velocity = targetDirection * speed;

    // Smooth rotation to face movement direction
    float angleToFace = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angleToFace), rotationSpeed * Time.deltaTime);
}

Vector2 RotateVector(Vector2 v, float angle)
{
    float rad = angle * Mathf.Deg2Rad;
    float cos = Mathf.Cos(rad);
    float sin = Mathf.Sin(rad);
    return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastHitTime + hitCooldown) // Check if cooldown has passed
            {
                lastHitTime = Time.time; // Update the last hit time
                PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
                if (player != null)
                {
                    player.TakeDamage();
                }
            }
        }
    }

}

