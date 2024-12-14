using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GunScript : MonoBehaviour
{
    public BulletPool bulletPool;
    public GunData gunData;
    public int currentAmmo;
    private float lastFiredTime;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    MSManagerScript _manager;
    public int ammoRemaining;
    PlayerScript playerScript;
    ammoCrateScript crateScript;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = gunData.magSize;
        _manager = FindAnyObjectByType<MSManagerScript>();
        ammoRemaining = gunData.maxAmmo;
        _manager.UpdateAmmoDisplay();
        playerScript = FindObjectOfType<PlayerScript>();
        crateScript = FindObjectOfType<ammoCrateScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //_manager.UpdateAmmoDisplay();
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
        if (Time.time - lastFiredTime >= gunData.fireRate && currentAmmo > 0)
        {
            Vector3 bulletDirection = (shootPoint.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            if (bulletScript != null)
            {
                if (playerScript.hasDoubleTap == true)
                {
                    bulletScript.Initialize(bulletDirection, gunData.damage * 2);
                }
                else
                {
                    bulletScript.Initialize(bulletDirection, gunData.damage);
                }
            }

            currentAmmo--;
            lastFiredTime = Time.time + 0.01f;
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
    public void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine() {
        _manager.reloadGunSound();
        yield return new WaitForSeconds(gunData.reloadTime); // wait for 1 second
        if (ammoRemaining > 0) {
            if (ammoRemaining >= gunData.magSize) {
                int ammoInMag = currentAmmo;
                currentAmmo = gunData.magSize;
                ammoRemaining -= gunData.magSize-ammoInMag;
                _manager.UpdateAmmoDisplay();
            }
            else {
                currentAmmo = ammoRemaining;
                ammoRemaining = 0;
                _manager.UpdateAmmoDisplay();
            }
        }
    }
    public void freeMag()
    {
        ammoRemaining = gunData.magSize;
    }

    public void fullAmmo()
    {
        ammoRemaining = gunData.maxAmmo;
        _manager.playerScore -= crateScript.ammoCost;
    }
}
