using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunScript : MonoBehaviour
{
    public GunData gunData;
    private int currentAmmo;
    private float lastFiredTime;
    public Transform shootPoint;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = gunData.maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        if (gunData.isShotgun)
        {
            FireShotgun();
        }
        else
        {
            FireSingleBullet();
        }
    }

    public void FireSingleBullet()
    {
        if(Time.time - lastFiredTime >= gunData.fireRate && currentAmmo > 0)
        {
            Vector3 bulletDirection = (shootPoint.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(bulletDirection, gunData.damage);
            }

            currentAmmo--;
            lastFiredTime = Time.time;
        }
    }

    public void FireShotgun()
    {
        // Ensure there is ammo and respect the fire rate cooldown
        if (Time.time - lastFiredTime >= gunData.fireRate && currentAmmo > 0)
        {
            for (int i = 0; i < gunData.pelletCount; i++)
            {
                // Randomize spread within the spread angle
                float spread = Random.Range(-gunData.spreadAngle / 2, gunData.spreadAngle / 2);

                // Calculate rotation with spread
                Quaternion pelletRotation = shootPoint.rotation * Quaternion.Euler(0, 0, spread + 90);

                // Instantiate pellet
                GameObject pellet = Instantiate(bulletPrefab, shootPoint.position, pelletRotation);
                BulletScript bulletScript = pellet.GetComponent<BulletScript>();

                // Optionally, initialize pellet with damage or other properties
                if (bulletScript != null)
                {
                    bulletScript.Initialize(pellet.transform.right, gunData.damage); // Divide damage for each pellet
                }
            }

            // Decrease ammo and reset the fire time
            currentAmmo--;
            lastFiredTime = Time.time;
        }
    }


    public void Reload()
    {
        currentAmmo = gunData.maxAmmo;
    }
}
