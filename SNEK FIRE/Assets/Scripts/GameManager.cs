using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool isGameActive;
    public GameObject pauseMenu;
    public GameObject titleScreen;
    public GameObject gameOverMenu;

    private Snake snakeScript;

    // Start is called before the first frame update
    void Start()
    {
        snakeScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Snake>();
        titleScreen.SetActive(true);
        isGameActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive == true){
        if (snakeScript.IsAlive()) {
        life = true;
        }
        else {
        life = false;
        gameOverMenu.SetActive(true);
        }
        }
    }

    public void StartGame(){
        titleScreen.SetActive(false);
        isGameActive = true;
    }
    public void PauseMenu(){
        pauseMenu.SetActive(true);
    }

    public void GameOver(){
        gameOverMenu.SetActive(true);
    }

    public void EndGame(){
        Application.Quit();
    }

    public void MainMenu(){
        titleScreen.SetActive(true);
        gameOverMenu.SetActive(false);
    }
}
