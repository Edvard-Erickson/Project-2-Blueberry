using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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

    public int currentRound;

    public int playerScore;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI roundNumber;
    public AudioSource hurtSound;
    public TextMeshProUGUI healthIndicator;
    public TextMeshProUGUI doorText;

    public Transform playerPosition;

    public GameObject doorChosen;
    private KeyCode openDoorKey = KeyCode.E;
    private KeyCode openShopKey = KeyCode.B;
    private float timeHighscore;
    private float startTime;
    private int roundHighscore;
    storeScript _storeScript;
    public GameObject _shop;
    private bool _shopOpen;
    GunScript gunScript;
    GunData gunData;
    public TMP_Text ammoText;
    public AudioSource reloadSound;
    public AudioClip reload;
    public bool isDoublePoints;
    public int generatorsOn;
    public bool allGeneratorsOn;

    /*public BulletPool bulletPool;
    public GameObject bulletPrefab;*/

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        timeHighscore = PlayerPrefs.GetFloat("timeHighscore", 0);
        startTime = Time.time;
        roundHighscore = PlayerPrefs.GetInt("roundHighscore", 0);
        currentRound = 0;
        StartRound();
        _shopOpen = false;
        ammoText = GameObject.Find("AmmoText").GetComponent<TMP_Text>();
        gunScript = GameObject.FindObjectOfType<GunScript>();
        UpdateAmmoDisplay();
        reloadSound = GameObject.Find("PistolReload").GetComponent<AudioSource>();
        hurtSound = GameObject.Find("PlayerHurt").GetComponent<AudioSource>();
        allGeneratorsOn = false;
        generatorsOn = 0;
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
        //GameObject door = findNearestDoor();
        //if (door != null && door.GetComponent<DoorScript>().doorColider.enabled)
        //{
        //    doorText.text = "Press 'E' to open door [cost 1000]";
        //}
        //else
        //{
        //    doorText.text = "";
        //}

        //if (Input.GetKeyDown(openDoorKey))
        //{
        //    Debug.Log("E key pressed");
        //}


        // Check if the 'E' key is pressed and if a door is nearby
        //if (Input.GetKeyDown(openDoorKey))
        //{ 
        //    GameObject nearestDoor = findNearestDoor();
        //    //checks if door is nearby
        //    if (nearestDoor != null)
        //    {
        //        Debug.Log("trying to open door");
        //        doorChosen = nearestDoor;
        //        openDoor();        
        //    }
        //}
        if (Input.GetKeyDown(openShopKey))
        {
            Debug.Log("B key pressed");
            if (!_shopOpen) {
                _shop.SetActive(true);
                _shopOpen = true;
            }
            else {
                _shop.SetActive(false);
                _shopOpen = false;
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
        isDoublePoints = false;
    }

    void FixedUpdate() {
        gunScript = GameObject.FindObjectOfType<GunScript>();
        UpdateAmmoDisplay();
    }
    void spawnZombies()
    {
        if (zombiesLeft > 0)
        {
            // Calculate the spawn interval based on the current round
            float spawnInterval = Mathf.Clamp(3f - currentRound * 0.1f, 0.1f, 3f); // Decreases the interval over time, but limits the minimum to 0.1 seconds

            // Start a coroutine to spawn zombies gradually over time
            StartCoroutine(SpawnZombiesOverTime(spawnInterval));
        }
    }

    IEnumerator SpawnZombiesOverTime(float spawnInterval)
    {
        // Continue spawning zombies until all are spawned
        while (zombiesLeft > 0)
        {
            // Wait for the specified spawn interval before spawning the next one
            yield return new WaitForSeconds(spawnInterval);

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
        }
    }

    //adds 10 to playerScore each time the player hits a zombie
    public void hitZombie()
    {
        if (isDoublePoints)
        {
            playerScore += 20;
            playerScoreText.text = "Score " + playerScore;
        }
        else
        {
            playerScore += 10;
            playerScoreText.text = "Score " + playerScore;
        }
    }

    //adds 100 to playerScore each time the player kills a zombie
    public void killedZombie()
    {
        if (isDoublePoints)
        {
            playerScore += 200;
            playerScoreText.text = "Score " + playerScore;
            zombiesAlive--;
        }
        playerScore += 100;
        playerScoreText.text = "Score " + playerScore;
        zombiesAlive--;
    }

    public void UpdateHealthUI(int health)
    {
        healthIndicator.text = string.Concat(Enumerable.Repeat(" *", health));
    }

    //activates when the 'O' key is pressed near a door
    //void openDoor()
    //{
    //    if (playerScore >= 1000)
    //    {
    //        doorChosen.GetComponent<DoorScript>().openDoor();
    //        playerScore -= 1000;
    //        playerScoreText.text = "Score " + playerScore;
    //    }
    //}

    //private GameObject findNearestDoor()
    //{
    //    //puts every object with DoorScript attached to it in an array
    //    GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

    //    //placeholder variable to keep track of the nearest door
    //    GameObject nearestDoor = null;

    //    //placeholder variable to keep track of the 
    //    float minDistance = Mathf.Infinity;

    //    //loops through DoorScripts
    //    foreach (GameObject door in doors)
    //    {
    //        DoorScript doorSr = door.GetComponent<DoorScript>();
    //        //if door is active game object and the player is nearby to that door
    //        if (doorSr.isActiveAndEnabled && doorSr.isPlayerNearby())
    //        {
    //            //calculate distance between player and door
    //            float distance = Vector2.Distance(door.transform.position, playerPosition.position);
    //            if (distance < minDistance)
    //            {
    //                minDistance = distance;
    //                nearestDoor = door;
    //            }
    //        }
    //    }
    //    //returns the nearest door, or null if there is no door nearby
    //    return nearestDoor;
    //}

    public void playerHurtSound() {
        
        hurtSound.Play();
    }
    
    public void reloadGunSound() {
        reloadSound.clip = reload;
        reloadSound.Play();
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

    public void UpdateAmmoDisplay()
    {
        Debug.Log("UpdateAmmoDisplay called");
        ammoText.text = $"{gunScript.currentAmmo}/{gunScript.ammoRemaining}";
    }

    public void repairGenerator() {
        generatorsOn++;
        if (generatorsOn == 6) {
            allGeneratorsOn = true;
        }
    }
}

