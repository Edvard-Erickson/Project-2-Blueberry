using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    public Sprite closedDoorSprite;
    public Sprite openDoorSprite;
    public float detectionRange = 3f;
    public LayerMask playerLayer;
    private MSManagerScript MSManagerScript;

    public Transform playerTransform;

    public bool isOpen;



    public SpriteRenderer spriteRenderer;
    public Collider2D doorColider;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer.sprite = closedDoorSprite;
        doorColider.enabled = true;
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // opens the door by changing the SpriteRenderer and turning the colider of
    public void openDoor()
    {
        Debug.Log("opening door");
        spriteRenderer.sprite = openDoorSprite;
        doorColider.enabled = false;
    }

    public bool isPlayerNearby()
    {
    
        
        // Direction from door to player
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, playerLayer);

        // Return true if the ray hits the player
        return hit.collider != null && hit.collider.CompareTag("Player");   
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}
