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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void boughtRifle() {
        if (!_playerScript.hasRifle && _sceneScript.playerScore >= 1500) {
            _playerScript.hasRifle = true;
            _sceneScript.playerScore -= 1500;
            rifleText.text = "Purchased";
        }
    }

    public void boughtShotgun() {
        if (!_playerScript.hasShotgun && _sceneScript.playerScore >= 1500) {
            _playerScript.hasShotgun = true;
            _sceneScript.playerScore -= 1500;
            shotgunText.text = "Purchased";
        }
    }
    public void boughtAutoPistol() {
        if (!_playerScript.hasAutoPistol && _sceneScript.playerScore >= 750) {
            _playerScript.hasAutoPistol = true;
            _sceneScript.playerScore -= 750;
            AutoPistol.text = "Purchased";
        }
    }
    public void boughtAutoShotgun() {
        if (!_playerScript.hasShotgun && _sceneScript.playerScore >= 2500) {
            _playerScript.hasAutoShotgun = true;
            _sceneScript.playerScore -= 2500;
            AutoShotgun.text = "Purchased";
        }
    }
    public void hasSniper() {
        if (!_playerScript.hasSniper && _sceneScript.playerScore >= 2000) {
            _playerScript.hasSniper = true;
            _sceneScript.playerScore -= 2000;
            Sniper.text = "Purchased";
        }
    }
    public void boughtLMG() {
        if (!_playerScript.hasLMG && _sceneScript.playerScore >= 2500) {
            _playerScript.hasLMG = true;
            _sceneScript.playerScore -= 2500;
            LMG.text = "Purchased";
        }
    }
    public void boughtDMR() {
        if (!_playerScript.hasDMR && _sceneScript.playerScore >= 1500) {
            _playerScript.hasDMR = true;
            _sceneScript.playerScore -= 1500;
            DMR.text = "Purchased";
        }
    }
    public void boughtSMG() {
        if (!_playerScript.hasSMG && _sceneScript.playerScore >= 1000) {
            _playerScript.hasSMG = true;
            _sceneScript.playerScore -= 1000;
            SMG.text = "Purchased";
        }
    }
}
