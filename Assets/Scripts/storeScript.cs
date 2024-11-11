using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class storeScript : MonoBehaviour
{

    public PlayerScript _playerScript;
    public MSManagerScript _sceneScript;
    public TMP_Text healthText;
    //public Button healthButton;
    public TMP_Text rifleText;
    //public Button rifleButton;
    public TMP_Text shotgunText;
    //public Button shotgunButton;
    public GameObject _shopPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void boughtHealth() {
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

    public void boughtRifle() {
        if (!_playerScript.hasRifle && _sceneScript.playerScore >= 1500) {
            _playerScript.hasRifle = true;
            rifleText.text = "Rifle Purchased";
        }
        else if (!_playerScript.hasRifle && _sceneScript.playerScore < 1500) {
            // tell player they don't have enough money

        }
    }

    public void boughtShotgun() {
        if (!_playerScript.hasShotgun && _sceneScript.playerScore >= 2000) {
            _playerScript.hasShotgun = true;
            shotgunText.text = "Shotgun Purchased";
        }
        else if (!_playerScript.hasShotgun && _sceneScript.playerScore < 2000) {
            // tell player they don't have enough money
            
        }
    }
}
