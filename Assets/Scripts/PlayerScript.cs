using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;

    public GameObject Projectile;
    public Transform endOfGun;
    public Transform centerOfGun;
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
    public bool hasRifle;
    public bool hasShotgun;
    public bool hasAutoPistol;
    public bool hasAutoShotgun;
    public bool hasDMR;
    public bool hasSniper;
    public bool hasLMG;
    public bool hasSMG;
    public TMP_Text _playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rbody = GetComponent<Rigidbody2D>();
        health = maxHealth;
        MSMScript = FindAnyObjectByType<MSManagerScript>();
        hasRifle = false;
        hasShotgun = false;
        hasAutoPistol = false;
        hasAutoShotgun = false;
        hasSniper = false;
        hasLMG = false;
        hasSMG = false;
        hasDMR = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPosition = new Vector3(transform.position.x, transform.position.y, -10);
        Vector3 smoothedPosition = Vector3.Lerp(mainCam.transform.position, desiredPosition, cameraSmoothSpeed);
        mainCam.transform.position = smoothedPosition;

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        Vector3 direction = (mousePos - transform.position).normalized;

        if (!canFire)
        {
            timer += Time.deltaTime;
            if(timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            Vector3 bulletDirection = (endOfGun.position - centerOfGun.position).normalized;
            GameObject bullet = Instantiate(Projectile, endOfGun.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection;

        }
    }

    void OnMove(InputValue value)
    {
        Vector2 moveVal = value.Get<Vector2>();
        rbody.velocity = moveVal * speed;
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
