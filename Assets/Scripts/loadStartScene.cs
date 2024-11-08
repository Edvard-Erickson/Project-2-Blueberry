using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadStartScene : MonoBehaviour
{

    public TextMeshProUGUI endGameText;
    
    // Start is called before the first frame update
    void Start()
    {
        int currentRound = PlayerPrefs.GetInt("currentRound");
        float timeElasped = PlayerPrefs.GetFloat("timeElapsed");
        endGameText.text = "Game Over!\n You survived " + currentRound + " rounds!\n You survived for " + formatTime(timeElasped);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startScene()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("startScene");
    }

    string formatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60); 
        int seconds = Mathf.FloorToInt(time % 60); 
        return string.Format("{0}:{1:00}", minutes, seconds);
    }


}
