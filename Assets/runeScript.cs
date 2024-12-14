using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class runeScript : MonoBehaviour
{
    public MSManagerScript MSMScript;
    public bool isActivated;

    public bool invisibleAtFirst;

    // Start is called before the first frame update
    void Start()
    {
        isActivated = false;
        MSMScript = FindAnyObjectByType<MSManagerScript>();

        if (invisibleAtFirst)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActivated = true;
            gameObject.SetActive(false);
        }
    }
}
