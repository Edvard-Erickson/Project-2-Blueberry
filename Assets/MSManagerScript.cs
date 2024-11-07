using System.Collections;
using System.Collections.Generic;
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

    private int currentRound = 0;

    // Start is called before the first frame update
    void Start()
    {
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
        currentRound += 1;
        roundInProgress = true;
        spawnZombies();
        if(currentRound % 5 == 0)
        {
            zombiesPerRound += 10;
        } else if(currentRound % 2 == 0)
        {
            zombiesPerRound += 3;
        }
        spawnDuration =  zombiesPerRound / 2;
        zombiesLeft = zombiesPerRound;
        zombiesAlive = zombiesPerRound;
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
            Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation); // Spawn the zombie
            zombiesLeft--; // Decrease the number of zombies left

            // Wait for the specified spawn interval before spawning the next one
            yield return new WaitForSeconds(spawnInterval);
        }
    }

}
