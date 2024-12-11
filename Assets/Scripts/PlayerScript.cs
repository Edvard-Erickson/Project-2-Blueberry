using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Camera mainCam;
 
    
    Rigidbody2D rbody;
    

    private float timer;

    public bool canFire;
    public float timeBetweenFiring;

    public float speed;

    public float cameraSmoothSpeed;

    public int health;
    public int maxHealth;
    public float regenDelay;
    public float regenRate;
    private Coroutine regenCoroutine;

    public MSManagerScript MSMScript;
    
    public TMP_Text _playerHealth;

    //gun storage and variables
    public GunData[] loadout = new GunData[2];
    public int currentSlotIndex = 0;
    private GameObject activeWeapon;
    public Transform weaponHolder;
    public GunScript activeGunScript;

    private Vector2 aimTarget;

    private bool isFiring;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rbody = GetComponent<Rigidbody2D>();
        health = maxHealth;
        MSMScript = FindAnyObjectByType<MSManagerScript>();

        loadout[0].isPurchased = true;
        EquipWeapon(currentSlotIndex);
        loadout[1] = null;
    }

    // Update is called once per frame
    void Update()
    {
        //update camera to player
        Vector3 desiredPosition = new Vector3(transform.position.x, transform.position.y, -10);
        Vector3 smoothedPosition = Vector3.Lerp(mainCam.transform.position, desiredPosition, cameraSmoothSpeed);
        mainCam.transform.position = smoothedPosition;

        //rotating and aiming
        rbody.rotation = GetAngle(aimTarget);

        
        //handle semi and automatic fire
        if (isFiring && activeGunScript != null)
        {
            
            if (activeGunScript.gunData.gunType == GunData.GunType.Automatic)
            {
                Shoot();
            }
            else if (activeGunScript.gunData.gunType == GunData.GunType.SemiAutomatic)
            {
                // Semi-automatic fire (handled in OnFire directly)
                if (isFiring)
                {
                    
                    Shoot();
                    isFiring = false; // Prevent holding down for semi-automatic
                }
            }
        }


    }

    void OnMove(InputValue value)
    {
        Vector2 moveVal = value.Get<Vector2>();
        rbody.velocity = moveVal * speed;
    }

    void OnAim(InputValue value)
    {
        aimTarget = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());


    }

    float GetAngle(Vector2 target)
    {
        Vector2 difference = target - rbody.position;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        return angle;
    }

    void OnFire(InputValue value)
    {
        isFiring = value.isPressed;
    }


    void OnReload(InputValue value)
    {
        if (activeWeapon != null)
        {
            GunScript gunScript = activeWeapon.GetComponent<GunScript>();
            if (gunScript != null)
            {
                gunScript.Reload(); // Call the fire method of the current gun
            }
        }
    }

    void OnInteract(InputValue value)
    {

    }

    void OnSwapWeapon()
    {
        
        if (currentSlotIndex == 0 && loadout[1] != null)
        {
            currentSlotIndex = 1;
            EquipWeapon(1);
            Debug.Log("swap to weapon 2");
        }
        else if (currentSlotIndex == 1)
        {
            currentSlotIndex = 0;
            EquipWeapon(0);
            Debug.Log("swap to weapon 1");
        }
    }

    public void PurchasedWeapon(GunData newGun)
    {
        // Find the current slot to replace
        if (loadout[1] == null)
        {
            loadout[1] = newGun;
            newGun.isPurchased = true;
            OnSwapWeapon();
            Debug.Log("Equipped gun to slot 2");
        }
        else
        {
            // Remove the gun currently in the slot
            RemoveGunFromLoadout(currentSlotIndex);

            // Add the new gun to the loadout
            loadout[currentSlotIndex] = newGun;
            newGun.isPurchased = true;
            EquipWeapon(currentSlotIndex);
            Debug.Log("Replaced gun in current slot");
        }
    }


    public void RemoveGunFromLoadout(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < loadout.Length && loadout[slotIndex] != null)
        {
            // Get the gun being removed
            GunData removedGun = loadout[slotIndex];

            // Reset the gun's availability (optional: use a dictionary or flag to track availability in the store)
            removedGun.isPurchased = false;

            // Remove the gun from the loadout
            loadout[slotIndex] = null;
            Debug.Log($"Gun {removedGun.gunName} is now purchasable again.");
        }
    }

    public void EquipWeapon(int slotIndex)
    {
        // Validate the slot index and ensure it's not empty
        if (slotIndex < 0 || slotIndex >= loadout.Length || loadout[slotIndex] == null)
        {
            Debug.LogWarning("Invalid weapon slot or empty slot!");
            return;
        }

        // Destroy the currently equipped weapon
        if (activeWeapon != null)
        {
            Destroy(activeWeapon);
        }

        // Equip the new weapon
        GunData selectedGun = loadout[slotIndex];
        activeWeapon = Instantiate(selectedGun.gunPrefab, weaponHolder);

        // Align weapon position and rotation with the holder
        activeWeapon.transform.localPosition = Vector3.zero;
        activeWeapon.transform.localRotation = Quaternion.identity;

        // Assign the GunData to the GunScript
        activeGunScript = activeWeapon.GetComponent<GunScript>();
        if (activeGunScript != null)
        {
            activeGunScript.gunData = selectedGun;
        }

        // Update the current slot index
        currentSlotIndex = slotIndex;

        Debug.Log($"Equipped weapon: {selectedGun.gunName}");
    }

    public void Shoot()
    {
        if (activeWeapon != null)
        {
            if (activeGunScript != null)
            {
                activeGunScript.Fire(); // Call the fire method of the current gun
            }
        }
    }


    public void TakeDamage()
    {
        health -= 1;
        MSMScript.UpdateHealthUI(health);
        MSMScript.playerHurtSound();
        if (health <= 0)
        {
            //die
            Debug.Log("Player has died");
            if(regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
                regenCoroutine = null;
            }
            MSMScript.GameOver();
            loadEndGameScene();
        }
        else
        {
            if(regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
            }
            regenCoroutine = StartCoroutine(RegenerateHealth());
        }
    }

    void loadEndGameScene()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("endGameScene");
    }


    IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(regenDelay);

        while(health < maxHealth)
        {
            health++;
            MSMScript.UpdateHealthUI(health);
            Debug.Log("health now " + health);
            yield return new WaitForSeconds(regenRate);
        }

        regenCoroutine = null;
    }

    // call if player purchases more health in the shop
    public void gainHealth() {
        _playerHealth.text += " *";
        maxHealth++;
    }

    // bought health script from store
    /* public void boughtHealth() {
        if (_playerScript.maxHealth < 5 && _sceneScript.playerScore >= 2500) {
            _playerScript.gainHealth();
            _sceneScript.playerScore -= 2500;
            if (_playerScript.maxHealth == 5) {
                healthText.text = "Max Health Reached";
            }
        }
        else if (_playerScript.maxHealth < 5 && _sceneScript.playerScore < 2500) {
            // tell player they don't have enough money

        }
        
    }
    */
}
