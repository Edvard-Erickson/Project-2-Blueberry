using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Guns/Gun")]
public class GunData : ScriptableObject
{
    public string gunName;
    public int damage;
    public int magSize;
    public int maxAmmo;
    public float fireRate;
    public GunType gunType; // Enum for gun firing mode (Automatic/Semi-Automatic)
    public GameObject gunPrefab;

    public bool isUpgraded;

    //for shotgun
    public bool isShotgun;
    public int pelletCount;
    public int spreadAngle;
    public float reloadTime;
    //for shop
    public bool isPurchased;
    public int price;

    public enum GunType { SemiAutomatic, Automatic }

    public void activateDoubleTap()
    {
        damage *= damage;
    }

    public void activateSpeedCola()
    {
        reloadTime *= .75f;
    }
}