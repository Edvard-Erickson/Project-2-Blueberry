using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MSManagerScript : MonoBehaviour
{
    public GameObject zombiePrefab;
    public GameObject[] spawnPoints;
    public int zombiesPerRound;
    public float spawnDuration;

    private int zombiesLeft;
    private bool roundInProgress;
    private int zombiesAlive;

    private int currentRound;

    private int playerScore;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI roundNumber;
    public TextMeshProUGUI healthIndicator;
    public TextMeshProUGUI doorText;

    public Transform playerPosition;

    public DoorScript door;
    public KeyCode openDoorKey = KeyCode.E;

    // Start is called before the first frame update
    void Start()
    {
        currentRound = 0;
        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        if (roundInProgress)
        {
            if(zombiesAlive <= 0)
            {
                roundInProgress = false;
            }
        }
        else
        {
            StartRound();
        }

        //checks if player is nearby door to display text
        DoorScript door = findNearestDoor();
        if (door != null && door.doorColider.enabled)
        {
            doorText.text = "Press 'E' to open door [cost 1000]";
        }
        else
        {
            doorText.text = "";
        }
        
        // Check if the 'E' key is pressed and if a door is nearby
        if (Input.GetKeyDown(openDoorKey))
        { 
            DoorScript nearestDoor = findNearestDoor();
            //checks if door is nearby
            if (nearestDoor != null)
            {
                door = nearestDoor;
                openDoor();        
            }
        }
    }

    void StartRound()
    {
        playerScoreText.text = "Score " + playerScore;
        currentRound += 1;
        if(currentRound % 5 == 0)
        {
            zombiesPerRound += 10;
        } else
        {
            zombiesPerRound += 3;
        }
        spawnDuration =  zombiesPerRound * 2;
        zombiesLeft = zombiesPerRound;
        zombiesAlive = zombiesPerRound;
        spawnZombies();
        roundInProgress = true;
        roundNumber.text = "" + currentRound;
    }

    void spawnZombies()
    {
        if (zombiesLeft > 0)
        {
            // Track how much time has passed to manage the spawn rate
            float spawnInterval = spawnDuration / zombiesPerRound;

            // Start a coroutine to spawn zombies gradually over time
            StartCoroutine(SpawnZombiesOverTime(spawnInterval));
        }
    }

    IEnumerator SpawnZombiesOverTime(float spawnInterval)
    {
        // Continue spawning zombies until all are spawned
        while (zombiesLeft > 0)
        {
            GameObject spawnPoint = null;
            while(spawnPoint == null)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                if (!spawnPoint.GetComponent<ZSpawnScript>().isOpen)
                {
                    spawnPoint = null;
                }
                // Pick a random spawn point
            }
            Instantiate(zombiePrefab, spawnPoint.transform.position, Quaternion.identity); // Spawn the zombie
            zombiesLeft--; // Decrease the number of zombies left

            // Wait for the specified spawn interval before spawning the next one
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    //adds 10 to playerScore each time the player hits a zombie
    public void hitZombie()
    {
        playerScore += 10;
        playerScoreText.text = "Score " + playerScore;
    }

    //adds 100 to playerScore each time the player kills a zombie
    public void killedZombie()
    {
        playerScore += 100;
        playerScoreText.text = "Score " + playerScore;
        zombiesAlive--;
    }

    public void UpdateHealthUI(int health)
    {
        healthIndicator.text = string.Concat(Enumerable.Repeat(" *", health));
    }

    //activates when the 'O' key is pressed near a door
    void openDoor()
    {
        if (playerScore >= 1000)
        {
            door.openDoor();
            playerScore -= 1000;
            playerScoreText.text = "Score " + playerScore;
        }
    }

    private DoorScript findNearestDoor()
    {
        //puts every object with DoorScript attached to it in an array
        DoorScript[] doors = FindObjectsOfType<DoorScript>();

        //placeholder variable to keep track of the nearest door
        DoorScript nearestDoor = null;

        //placeholder variable to keep track of the 
        float minDistance = Mathf.Infinity;

        //loops through DoorScripts
        foreach (DoorScript door in doors)
        {
            //if door is active game object and the player is nearby to that door
            if (door.isActiveAndEnabled && door.isPlayerNearby())
            {
                //calculate distance between player and door
                float distance = Vector2.Distance(door.transform.position, playerPosition.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestDoor = door;
                }
            }
        }
        //returns the nearest door, or null if there is no door nearby
        return nearestDoor;
    }
}
