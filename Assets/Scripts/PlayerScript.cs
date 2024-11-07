using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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


    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rbody = GetComponent<Rigidbody2D>();
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
}
