using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public AudioSource hurtSound;

    public TextMeshProUGUI healthIndicator;
    public TextMeshProUGUI doorText;

    public Transform playerPosition;

    public DoorScript door;
    public KeyCode openDoorKey = KeyCode.E;

    private float timeHighscore;
    private float startTime;
    private int roundHighscore;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        timeHighscore = PlayerPrefs.GetFloat("timeHighscore", 0);
        startTime = Time.time;
        roundHighscore = PlayerPrefs.GetInt("roundHighscore", 0);
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
        playerScoreText.text = "Score " + playerScore;


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
            while (spawnPoint == null)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // Pick a random spawn point
                if (!spawnPoint.GetComponent<ZSpawnScript>().isOpen)
                {
                    spawnPoint = null;
                }
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

    public void playerHurtSound() {
        hurtSound.Play();
    }

    public void GameOver()
    {
        // Calculate elapsed time
        float elapsedTime = Time.time - startTime;

        // Save current round and elapsed time to PlayerPrefs for display in the game over screen
        PlayerPrefs.SetInt("currentRound", currentRound);
        PlayerPrefs.SetFloat("timeElapsed", elapsedTime);

        // Check and update round high score
        int savedRoundHighscore = PlayerPrefs.GetInt("roundHighscore", 0);
        if (currentRound > savedRoundHighscore)
        {
            PlayerPrefs.SetInt("roundHighscore", currentRound);
        }

        // Check and update time high score
        float savedTimeHighscore = PlayerPrefs.GetFloat("timeHighscore", 0);
        if (elapsedTime > savedTimeHighscore)
        {
            PlayerPrefs.SetFloat("timeHighscore", elapsedTime);
        }

        
        PlayerPrefs.Save();

        
        SceneManager.LoadScene("endGameScene"); 
    }
}

