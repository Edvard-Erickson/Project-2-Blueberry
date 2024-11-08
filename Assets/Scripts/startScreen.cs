using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startScreen : MonoBehaviour
{

    public TextMeshProUGUI roundHighscoreText;
    public TextMeshProUGUI timeHighScore;
    // Start is called before the first frame update
    void Start()
    {
        int roundHighscore = PlayerPrefs.GetInt("roundHighscore",0);
        roundHighscoreText.text = " Round: " + roundHighscore;
        float time = PlayerPrefs.GetFloat("timeHighscore", 0);
        timeHighScore.text = " Time: " + formatTime(time);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadGameScene()
    {
        
        
        SceneManager.LoadScene("SampleScene");
    }

    public void loadSmallMapScene()
    {
        
        SceneManager.LoadScene("smallMap");
    }

    string formatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0}:{1:00}", minutes, seconds);
    }
}
