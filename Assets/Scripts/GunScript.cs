using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GunScript : MonoBehaviour
{
    public GunData gunData;
    public int currentAmmo;
    private float lastFiredTime;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    MSManagerScript _manager;
    public int ammoRemaining;
    PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = gunData.magSize;
        _manager = FindAnyObjectByType<MSManagerScript>();
        ammoRemaining = gunData.maxAmmo;
        _manager.UpdateAmmoDisplay();
        playerScript=FindObjectOfType<PlayerScript>();
        crateScript=FindObjectOfType<ammoCrateScript>();
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
        _manager.UpdateAmmoDisplay();
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
                if (playerScript.hasDoubleTap == true)
                {
                    bulletScript.Initialize(bulletDirection, gunData.damage*2);
                }
                else
                {
                    bulletScript.Initialize(bulletDirection, gunData.damage);
                }
            }

            currentAmmo--;
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
                    if (playerScript.hasDoubleTap == true)
                    {
                        bulletScript.Initialize(pellet.transform.right, gunData.damage * 2);
                    }
                    else
                    {
                        bulletScript.Initialize(pellet.transform.right, gunData.damage); // Divide damage for each pellet
                    }
                }
            }

            // Decrease ammo and reset the fire time
            currentAmmo--;
        }
    }
    public IEnumerator Reload()
    {
        Debug.Log("reloading");
        playerScript.canFire = false;
        if (playerScript.hasSpeedCola)
        {
            yield return new WaitForSeconds(gunData.reloadTime / 2);
        }
        else
        {
            yield return new WaitForSeconds(gunData.reloadTime);
        }
        int bulletsNeeded = gunData.maxMag - currentAmmo;
        if (bulletsNeeded > 0)
        {
            if (gunData.reserveBulletSize >= bulletsNeeded)
            {
                gunData.reserveBulletSize -= bulletsNeeded;
                currentAmmo = gunData.maxMag;
            }
        }
        else
        {
            currentAmmo += gunData.reserveBulletSize;
            gunData.reserveBulletSize = 0;
        }
        playerScript.canFire = true;
        ammoText.text = currentAmmo + "/" + gunData.reserveBulletSize;   
    }

    public void freeMag() {
        ammoRemaining = gunData.magSize;
    }

    public void fullAmmo() {
        ammoRemaining = gunData.maxAmmo;
        _manager.playerScore -= crateScript.ammoCost;
    }
}
