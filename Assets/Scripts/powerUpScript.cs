using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpScript : MonoBehaviour
{

    public bool isNuc;
    public bool isMaxAmmo;
    public bool isDoublePoints;
    public bool isInstantKill;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnPowerup()
    {

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