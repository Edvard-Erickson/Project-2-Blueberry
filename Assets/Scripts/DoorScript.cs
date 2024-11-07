using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    [SerializeField] private Sprite closedDoorSprite;
    [SerializeField] private Sprite openDoorSprite;
    [SerializeField] private float dectectionRange = 3f;
    [SerializeField] private LayerMask playerLayer;


    public SpriteRenderer spriteRenderer;
    public Collider2D doorColider;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = closedDoorSprite;
        doorColider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // opens the door by changing the SpriteRenderer and turning the colider of
    public void openDoor()
    {
        spriteRenderer.sprite = openDoorSprite;
        doorColider.enabled = false;
    }

    public bool isPlayerNearby()
    {
        //implement
        return true;
    }
}
