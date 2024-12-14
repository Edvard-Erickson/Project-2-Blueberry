using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpScript : MonoBehaviour
{

    public bool isNuc;
    public bool isMaxAmmo;
    public bool isDoublePoints;
    public bool isInstantKill;
    public GameObject powerUpPrefab;
    private string type;

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
        
        int random = Random.Range(0, 3);
        if (random == 0)
        {
            isNuc = true;
            type = "nuc";
        }
        else if(random == 1)
        {
            isMaxAmmo = true;
            type = "ammo";
        }
        else if (random == 2)
        {
            isDoublePoints = true;
            type = "points";
        }
        else if (random == 3)
        {
            isInstantKill = true;
            type = "kill";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnPowerup(Vector3 position)
    {
        int spawnChance = Random.Range(1, 5);
        if (spawnChance < 6) 
        {
            Instantiate(powerUpPrefab, position, Quaternion.identity);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log(type);
            handlePowerup();
            Destroy(gameObject);

        }
    }

    public void handlePowerup()
    {
        if (isNuc)
        {
            GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
            foreach (GameObject zombie in zombies)
            {
                Destroy(zombie);
            }
        }
        else if (isMaxAmmo)
        {
            GunScript gun = FindObjectOfType<GunScript>();
            gun.ammoRemaining = gun.gunData.maxAmmo;
        }
        else if (isDoublePoints)
        {
           StartCoroutine(handleDoublePoints());
        }
        else if (isInstantKill)
        {
            handleInstantKill();
        }
    }

    private IEnumerator handleDoublePoints()
    {
        MSManagerScript msm = new MSManagerScript();
        GameObject gameObject = GameObject.Find("MSmanager");
        msm = gameObject.GetComponent<MSManagerScript>();
        msm.isDoublePoints = true;
        yield return new WaitForSeconds(30);
        msm.isDoublePoints = false;
    }

    private IEnumerator handleInstantKill()
    {
        GunScript gun = FindObjectOfType<GunScript>();
        int damage = gun.gunData.damage;
        gun.gunData.damage = 100000000;
        yield return new WaitForSeconds(30);
        gun.gunData.damage = damage;
    }
}
