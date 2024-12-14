using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Camera mainCam;
    public TextMeshProUGUI interactionText;
    Rigidbody2D rbody;

    [Header("RayCast Detection")]
    public float detectionRange;
    public int numberOfRays;
    public LayerMask interactableLayer;
    private GameObject detectedObject = null;
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
    GunScript gunScript;

    private Vector2 aimTarget;

    private bool isFiring;
    //booleans to see if player already has the perk
    public bool hasJuggernog;
    public bool hasDoubleTap;
    public bool hasSpeedCola;
    public bool hasStaminup;
    public AudioSource reload;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rbody = GetComponent<Rigidbody2D>();
        health = maxHealth;
        MSMScript = FindAnyObjectByType<MSManagerScript>();
        gunScript = GameObject.FindObjectOfType<GunScript>();

        loadout[0].isPurchased = true;
        EquipWeapon(currentSlotIndex);
        loadout[1] = null;
    }

    // Update is called once per frame
    void Update()
    { 
        //uses raycast every frame to see if something in the 'interactable' layer is in range
        DetectNearbyObjects();
        HandleInteractionText();

        //checks to see if 'e' has been pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithObjects();
        }

        Vector3 desiredPosition = new Vector3(transform.position.x, transform.position.y, -10);
        Vector3 smoothedPosition = Vector3.Lerp(mainCam.transform.position, desiredPosition, cameraSmoothSpeed);
        mainCam.transform.position = smoothedPosition;

        //rotating and aiming
        rbody.rotation = GetAngle(aimTarget);

        
        //handle semi and automatic fire
        if (isFiring && gunScript != null)
        {
            
            if (gunScript.gunData.gunType == GunData.GunType.Automatic)
            {
                Shoot();
            }
            else if (gunScript.gunData.gunType == GunData.GunType.SemiAutomatic)
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
    public void reloadSound() {
        reload.Play();
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
            MSMScript.UpdateAmmoDisplay();
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
        gunScript = activeWeapon.GetComponent<GunScript>();
        if (gunScript != null)
        {
            gunScript.gunData = selectedGun;
        }

        // Update the current slot index
        currentSlotIndex = slotIndex;

        Debug.Log($"Equipped weapon: {selectedGun.gunName}");
    }

    public void Shoot()
    {
        if (activeWeapon != null)
        {
            if (gunScript != null)
            {
                gunScript.Fire(); // Call the fire method of the current gun
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


    //method for raycasting
    private void DetectNearbyObjects()
    {
        //variable to store detected object, starts at null as if nothing is there
        detectedObject = null;
        if (detectedObject == null)
        {
            interactionText.text = "";
        }

        //creates area for raycast
        float angleStep = 360f / numberOfRays;
        for (int i = 0; i < numberOfRays; i++)
        {
            //raycasting mumbo jumbo
            float angle = i * angleStep;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, interactableLayer);
            //if something is detected, detected object is set to that object
            if (hit.collider != null)
            {
                detectedObject = hit.collider.gameObject;
                Debug.Log("Detected Object: " + detectedObject.name);
                break;
            }
        }
    }
    
    private void HandleInteractionText()
    {
        if (detectedObject != null)
        {
            if (detectedObject.CompareTag("Door"))
            {
                DoorScript door = detectedObject.GetComponent<DoorScript>();
                if (door != null)
                {
                    if (!door.isOpen)
                    {
                        interactionText.enabled = true;
                        interactionText.text = $"Press 'E' to open Door [Cost: {door.doorCost}]";
                    }
                }
            }
            else if (detectedObject.CompareTag("ammoCrate"))
            {
                if (interactionText != null) {
                    if (gunScript.ammoRemaining == 0) {
                    interactionText.enabled = true;
                    interactionText.text = $"You're out of ammo! Press 'E' to get a free mag!";
                }
                else {
                    interactionText.enabled = true;
                    interactionText.text = $"Press 'E' to fill your ammo to the max! [Cost: 500]";
                }
            }
            }
            else if (detectedObject.CompareTag("Perk"))
            {
                perkScript perk = detectedObject.GetComponent<perkScript>();
                if (perk != null)
                {
                    if (!perk.IsPerkAlreadyOwned(this))
                    {
                        interactionText.enabled = true;
                        interactionText.text = $"Press 'E' to acquire {perk.getPerkName()} [Cost: {perk.GetPerkCost()}]";
                    }
                }
            }
            else if (detectedObject.CompareTag("Generator")) {
                interactionText.enabled = true;
                interactionText.text = $"[{MSMScript.generatorsOn}/6] Press 'E' to repair Generator [Cost: [1000]]";
            }
            else if (detectedObject.CompareTag("PaP"))
            {
                interactionText.enabled = true;
                interactionText.text = $"Press 'E' to upgrade gun [Cost: [5000]]";
            }
        }
        else
        {
            interactionText.enabled = false;
        }
    }

    //method that runs when 'E' is pressed
    private void InteractWithObjects()
    {
        //checks if something is in range of the raycast
        if (detectedObject != null)
        {
            //if the object is a door, handle accordingly
            if (detectedObject.CompareTag("Door"))
            {
                DoorScript door = detectedObject.GetComponent<DoorScript>();
                if (door != null && (MSMScript.playerScore >= door.doorCost) && !door.isOpen)
                {
                    MSMScript.playerScore -= door.doorCost;
                    door.Toggle();
                }
            }
            else if (detectedObject.CompareTag("Perk"))
            {
                perkScript perk = detectedObject.GetComponent<perkScript>();
                if (perk != null && MSMScript.playerScore >= perk.GetPerkCost())
                {
                    Debug.Log("Interacted with perk");
                    if (!perk.IsPerkAlreadyOwned(this))
                    {
                        MSMScript.playerScore -= perk.GetPerkCost();
                        perk.UsePerk();
                        perk.SetPerkOwned(this);
                    }
                }
            }
            else if (detectedObject.CompareTag("ammoCrate"))
            {
                Debug.Log("Interacted with ammo crate");
                if (gunScript.ammoRemaining == 0) {
                    gunScript.freeMag();
                }
                else if (gunScript.ammoRemaining != gunScript.gunData.maxAmmo) {
                    gunScript.fullAmmo();
                }
            }
            else if (detectedObject.CompareTag("Generator")) {
                Debug.Log("Interacted with generator");
                if (MSMScript.playerScore >= 1000 && MSMScript.allGeneratorsOn == false) {
                    MSMScript.playerScore -= 1000;
                    MSMScript.repairGenerator();
                    detectedObject.layer = 9;
                }
            }
            else if (detectedObject.CompareTag("PaP"))
            {
                Debug.Log("Interacted with PaP");
                if (MSMScript.playerScore >= 5000 && MSMScript.thirdStep)
                {
                    MSMScript.playerScore -= 5000;
                    loadout[currentSlotIndex].isUpgraded = true;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "MUD") {
            speed *= 0.75f;
        }
    }
    
    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "MUD") {
            speed *= 1.25f;
        }
    }
}
