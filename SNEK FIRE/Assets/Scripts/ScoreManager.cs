using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int currentScore;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public TMP_Text highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: \n" + currentScore.ToString("D7"); ;
    }

    public void UpdateScore(int points)
    {
        currentScore += points;
        scoreText.text = "Score: \n" + currentScore.ToString("D7"); ;
    }

    public void HighScoreUpdate()
    {
        if (PlayerPrefs.HasKey("SavedHighScore"))
        {
            if (currentScore > PlayerPrefs.GetInt("SavedHighScore"))
            {
                PlayerPrefs.SetInt("SavedHighScore", currentScore);
            } else {
                PlayerPrefs.SetInt("SavedHighScore", currentScore);
            }

            finalScoreText.text = currentScore.ToString("D7");
            highScoreText.text = PlayerPrefs.GetInt("SavedHighScore").ToString();
        }
    }
}
