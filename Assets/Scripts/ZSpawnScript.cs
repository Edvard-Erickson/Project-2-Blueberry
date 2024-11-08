using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZSpawnScript : MonoBehaviour
{
    public bool isOpen;
    public RoomScript room;
    // Start is called before the first frame update
    void Start()
    {
        room = GetComponentInParent<RoomScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            if (room.isOpen)
            {
                isOpen = true;
            }
        }
    }
}
