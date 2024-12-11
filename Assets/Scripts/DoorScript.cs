

using JetBrains.Annotations;
using UnityEngine;

public class DoorScript : MonoBehaviour
{ 
    public Sprite closedDoorSprite;
    public Sprite openDoorSprite;
    public int doorCost;

    public bool isOpen;

    public SpriteRenderer spriteRenderer;
    public Collider2D doorColider;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = closedDoorSprite;
        doorColider.enabled = true;
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // opens the door by changing the SpriteRenderer and turning the colider of
  


    public void Toggle()
    {
        spriteRenderer.sprite = openDoorSprite;
        doorColider.enabled = false;
        isOpen = true;
    }
}
