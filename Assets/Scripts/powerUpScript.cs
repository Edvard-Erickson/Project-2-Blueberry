using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpScript : MonoBehaviour
{

    public bool isNuc;
    public bool isMaxAmmo;
    public bool isDoublePoints;
    public bool isInstantKill;
    public GameObject nukeDrop;
    public GameObject ammoDrop;
    public GameObject pointsDrop;
    public GameObject instaDrop;

    // Start is called before the first frame update
    void Start()
    {
        isNuc = false;
        isMaxAmmo = false;
        isDoublePoints = false;
        isInstantKill = false;
    }
    private void Awake()
    {
        int random = Random.Range(0, 4);
        if (random == 0)
        {
            isNuc = true;
        }
        else if(random == 1)
        {
            isMaxAmmo = true;
        }
        else if (random == 2)
        {
            isDoublePoints = true;
        }
        else if (random == 3)
        {
            isInstantKill = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnPowerup(Vector3 position)
    {
        int spawnChance = Random.Range(1, 100);
        if (spawnChance < 6) 
        {
            GameObject drop = randomPowerUp();
            Instantiate(drop, position, Quaternion.identity);
        }

    }

    private GameObject randomPowerUp() {
        GameObject[] powerUps = { nukeDrop, ammoDrop, pointsDrop, instaDrop };
        return powerUps[Random.Range(0, powerUps.Length)];
    }

    // public void handlePowerup(string dropName)
    // {
    //     if (dropName == "nukeDrop")
    //     {
    //         GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
    //         foreach (GameObject zombie in zombies)
    //         {
    //             Destroy(zombie);
    //         }
    //     }
    //     else if (dropName == "ammoDrop")
    //     {
    //         GunScript gun = FindObjectOfType<GunScript>();
    //         gun.ammoRemaining = gun.gunData.maxAmmo;
    //     }
    //     else if (dropName == "pointsDrop")
    //     {
    //        StartCoroutine(handleDoublePoints());
    //     }
    //     else if (dropName == "instaDrop")
    //     {
    //         handleInstantKill();
    //     }
    // }
    
}

