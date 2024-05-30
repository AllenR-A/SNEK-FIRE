using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private int scoreMultiplier;
    [SerializeField] private int length;
    [SerializeField] private int fireBullets;
    [SerializeField] private int fireBulletsMax = 24;
    [SerializeField] private int specialBullets;
    [SerializeField] private int specialBulletsMax = 7;
    [SerializeField] private bool randomTP = false;
    [SerializeField] private bool life = true;
    //  public TextMeshProUGUI gameOverText;
     public GameObject titleScreen;
     public GameObject gameOverScreen;
     public GameObject pauseMenuScreen;
     public bool isGameActive;


    private Snake snakeScript;

    // Start is called before the first frame update
    void Start()
    {
        titleScreen.SetActive(true);
        snakeScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Snake>();
        isGameActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (snakeScript.IsAlive())
        {
            life = true;
            }
        else
        {
            life = false;
            // gameOverText.gameObject.SetActive(true);
            }

    }

    public void StartGame()
    {
        titleScreen.SetActive(false);
        isGameActive = true;
        // gameOverText.gameObject.SetActive(false);
    }

    public void GameOver(){

    }

    public void PauseMenu(){

    }
}
