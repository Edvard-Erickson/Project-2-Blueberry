
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isOpen;
    public GameObject[] doors;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen)
        {
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i].GetComponent<DoorScript>().isOpen)
                {
                    isOpen = true;
                }
            }
        }
    }
}
