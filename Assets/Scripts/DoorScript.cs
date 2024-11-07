using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Collider2D doorColider;

    public Sprite closedDoorSprite;
    public Sprite openDoorSprite;
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
}
