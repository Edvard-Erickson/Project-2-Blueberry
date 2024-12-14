using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class storeScript : MonoBehaviour
{

    public PlayerScript _playerScript;
    public MSManagerScript _sceneScript;
    public TMP_Text AutoPistol;
    //public Button healthButton;
    public TMP_Text rifleText;
    //public Button rifleButton;
    public TMP_Text shotgunText;
    //public Button shotgunButton;
    public TMP_Text SMG;
    public TMP_Text AutoShotgun;
    public TMP_Text Sniper;
    public TMP_Text DMR;
    public TMP_Text LMG;    
    public GameObject _shopPanel;

    public GunData assaultRifle;
    public GunData smg;
    public GunData dmr;
    public GunData sniperRifle;
    public GunData pistol;
    public GunData autoPistol;
    public GunData shotgun;
    public GunData lmg;
    public GunData autoShotgun;


    // Start is called before the first frame update
    void Start()
    {
        if (_playerScript == null)
        {
            _playerScript = FindObjectOfType<PlayerScript>();
            Debug.Log($"PlayerScript automatically assigned: {_playerScript}");
        }

        assaultRifle.isPurchased = false;
        smg.isPurchased = false;
        dmr.isPurchased = false;
        sniperRifle.isPurchased = false;
        pistol.isPurchased = false;
        autoPistol.isPurchased = false;
        shotgun.isPurchased = false;
        lmg.isPurchased = false;
        autoShotgun.isPurchased = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void boughtRifle() {
        if (!assaultRifle.isPurchased && _sceneScript.playerScore >= assaultRifle.price)
        {
            assaultRifle.isPurchased = true;

            _playerScript.PurchasedWeapon(assaultRifle);
            assaultRifle.isUpgraded = false;

            _sceneScript.playerScore -= assaultRifle.price;
            rifleText.text = "Purchased";
            UpdateShop();
        }
    }

    public void boughtShotgun() {
        if (!shotgun.isPurchased && _sceneScript.playerScore >= shotgun.price) {
            shotgun.isPurchased = true;

            //set the player's weapon with the gunData
            _playerScript.PurchasedWeapon(shotgun);
            shotgun.isUpgraded = false;

            _sceneScript.playerScore -= shotgun.price;
            shotgunText.text = "Purchased";
            UpdateShop();
        }
    }
    public void boughtAutoPistol() {
        if (!autoPistol.isPurchased && _sceneScript.playerScore >= autoPistol.price) {
            autoPistol.isPurchased = true;

            //set the player's weapon with the gunData
            _playerScript.PurchasedWeapon(autoPistol);
            autoPistol.isUpgraded = false;

            _sceneScript.playerScore -= autoPistol.price;
            AutoPistol.text = "Purchased";
            UpdateShop();
        }
    }
    public void boughtAutoShotgun() {
        if (!autoShotgun.isPurchased && _sceneScript.playerScore >= autoShotgun.price) {
            autoShotgun.isPurchased = true;

            //set the player's weapon with the gunData
            _playerScript.PurchasedWeapon(autoShotgun);
            autoShotgun.isUpgraded = false;

            _sceneScript.playerScore -= autoShotgun.price;
            AutoShotgun.text = "Purchased";
            UpdateShop();
        }
    }
    public void hasSniper() {
        if (!sniperRifle.isPurchased && _sceneScript.playerScore >= sniperRifle.price) {
            sniperRifle.isPurchased = true;

            //set the player's weapon with the gunData
            _playerScript.PurchasedWeapon(sniperRifle);
            sniperRifle.isUpgraded = false;

            _sceneScript.playerScore -= sniperRifle.price;
            Sniper.text = "Purchased";
            UpdateShop();
        }
    }
    public void boughtLMG() {
        if (!lmg.isPurchased && _sceneScript.playerScore >= lmg.price) {
            lmg.isPurchased = true;

            //set the player's weapon with the gunData
            _playerScript.PurchasedWeapon(lmg);
            lmg.isUpgraded = false;

            _sceneScript.playerScore -= lmg.price;
            LMG.text = "Purchased";
            UpdateShop();
        }
    }
    public void boughtDMR() {
        if (!dmr.isPurchased && _sceneScript.playerScore >= dmr.price) {
            smg.isPurchased = true;

            //set the player's weapon with the gunData
            _playerScript.PurchasedWeapon(dmr);
            dmr.isUpgraded = false;

            _sceneScript.playerScore -= dmr.price;
            DMR.text = "Purchased";
            UpdateShop();
        }
    }
    public void boughtSMG() {
        if (!smg.isPurchased && _sceneScript.playerScore >= smg.price) {
            smg.isPurchased = true;

            //set the player's weapon with the gunData
            _playerScript.PurchasedWeapon(smg);
            smg.isUpgraded = false;

            _sceneScript.playerScore -= smg.price;
            SMG.text = "Purchased";
            UpdateShop();
        }
    }


    public void UpdateShop()
    {
        if (!smg.isPurchased) { SMG.text = smg.price.ToString(); }
        if (!assaultRifle.isPurchased) { rifleText.text = assaultRifle.price.ToString(); }
        if (!shotgun.isPurchased) { shotgunText.text = shotgun.price.ToString(); }
        if (!autoPistol.isPurchased) { AutoPistol.text = autoPistol.price.ToString(); }
        if (!autoShotgun.isPurchased) { AutoShotgun.text = autoShotgun.price.ToString(); }
        if (!sniperRifle.isPurchased) { Sniper.text = sniperRifle.price.ToString(); }
        if (!dmr.isPurchased) { DMR.text = dmr.price.ToString(); }
        if (!lmg.isPurchased) { LMG.text = lmg.price.ToString(); }
    }
}
