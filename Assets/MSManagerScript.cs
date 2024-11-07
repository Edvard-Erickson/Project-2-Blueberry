using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MSManagerScript : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform[] spawnPoints;
    public int zombiesPerRound;
    public float spawnDuration;

    private int zombiesLeft;
    private bool roundInProgress;
    private int zombiesAlive;

    private int currentRound;

    private int playerScore;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI roundNumber;

    public Transform playerPosition;

    public DoorScript door;
    public KeyCode openDoorKey = KeyCode.O;

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
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // Pick a random spawn point
            Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity); // Spawn the zombie
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
            //if door is active game object and the player is nearby to that door, think this is bad check as of now
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
        return nearestDoor;
    }
}
