using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class perkScript : MonoBehaviour
{
    // Perk identifiers
    public bool isJuggernog;
    public bool isDoubleTap;
    public bool isSpeedCola;
    public bool isStaminup;

    // Perk costs
    public int juggernogCost = 2500;
    public int doubleTapCost = 2000;
    public int speedColaCost = 3000;
    public int staminupCost = 2000;

    // Reference to the player
    public GameObject player;

    GunData gunData;

    void Start() {
        gunData = GetComponent<GunData>();
    }

    // Method to apply the perk
    public void UsePerk()
    {
        PlayerScript playerScript = player.GetComponent<PlayerScript>();
            if (isJuggernog)
            {
                ApplyJuggernog(playerScript);
            }
            else if (isDoubleTap)
            {
                ApplyDoubleTap(playerScript);
            }
            else if (isSpeedCola)
            {
                ApplySpeedCola(playerScript);
            }
            else if (isStaminup)
            {
                ApplyStaminup(playerScript);
            }
    }

    // Method to get the cost of the perk
    public int GetPerkCost()
    {
        if (isJuggernog) 
        { 
            return juggernogCost;
        }
        if (isDoubleTap)
        {
            return doubleTapCost;
        }
        if (isSpeedCola)
        {
            return speedColaCost;
        }
        if (isStaminup)
        {
            return staminupCost;
        }
        return 0;
    }

    // Method to check if the perk is already owned
    public bool IsPerkAlreadyOwned(PlayerScript playerScript)
    {
        if (isJuggernog)
        {
            return playerScript.hasJuggernog;
        }
        if (isDoubleTap)
        {
            return playerScript.hasDoubleTap;
        }
        if (isSpeedCola)
        {
            return playerScript.hasSpeedCola;
        }
        if (isStaminup)
        {
            return playerScript.hasStaminup;
        }
        return false;
    }

    public void SetPerkOwned(PlayerScript playerScript)
    {
        if (isJuggernog)
        {
            playerScript.hasJuggernog = true;
        }
        if (isDoubleTap)
        {
            playerScript.hasDoubleTap = true;
        }
        if (isSpeedCola)
        {
            playerScript.hasSpeedCola = true;
        }
        if (isStaminup)
        {
            playerScript.hasStaminup = true;
        }
    }

    public string getPerkName()
    {
        if (isJuggernog)
        {
            return "Juggernog";
        }
        else if (isDoubleTap) 
        {
            return "Double Tap";
        }
        else if (isSpeedCola)
        {
            return "Speed Cola";
        }
        else if (isStaminup)
        {
            return "Staminup";
        }
        else
        {
            return "";
        }
    }

    private void ApplyJuggernog(PlayerScript playerScript)
    {
        playerScript.maxHealth += 2;
        playerScript.health += 2;
    }

    private void ApplyDoubleTap(PlayerScript playerScript)
    {

    }

    private void ApplySpeedCola(PlayerScript playerScript)
    {
        gunData.reloadTime *= 0.75f;

    }

    private void ApplyStaminup(PlayerScript playerScript)
    {
        playerScript.speed = playerScript.speed * 1.5f;
    }
}
