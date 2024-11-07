using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Rigidbody2D rbody;
    public int bulletSpeed;
    public LayerMask collisionLayers;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.velocity = rbody.velocity * bulletSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rbody.velocity.normalized, bulletSpeed * Time.deltaTime, collisionLayers);

        if (hit.collider != null && !hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Bullet"))
        {
            if (hit.collider.CompareTag("Zombie"))
            {
                hit.collider.GetComponent<ZombieScript>().health -= damage;
            }
            Destroy(gameObject);
        }
    }
}
