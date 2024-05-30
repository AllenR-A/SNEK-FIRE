using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private int scoreMultiplier;
    [SerializeField] private int length;
    [SerializeField] private int level;
    [SerializeField] private int fireBullets;
    [SerializeField] private int fireBulletsMax = 24;
    [SerializeField] private int specialBullets;
    [SerializeField] private int specialBulletsMax = 7;
    [SerializeField] private bool randomTP = false;
    [SerializeField] private bool life = true;
    public GameObject pauseMenu;
    public GameObject titleScreen;
    public GameObject gameOverMenu;

    private Snake snakeScript;
    private ScoreManager scoreScript;

    //================ Encapsulation ================
    public int GetScore() { return score; }
    public void SetScore(int n) { score = n; }
    public int GetScoreMultiplier() { return scoreMultiplier; }
    public void SetScoreMultiplier(int n) { scoreMultiplier = n; }
    public int GetLength() { return length; }
    public void SetLength(int n) { length = n; }
    public int GetLevel() { return level; }
    public void SetLevel(int n) { level = n; }
    public int GetFireBullets() { return fireBullets; }
    public void SetFireBullets(int n) { fireBullets = n; }
    public int GetFireBulletsMax() { return fireBulletsMax; }
    public int GetSpecialBullets() { return specialBullets; }
    public void SetSpecialBullets(int n) { specialBullets = n; }
    public int GetSpecialBulletsMax() { return specialBulletsMax; }
    public bool IsTeleportRandom() { return randomTP; }
    public void IsTeleportRandom(bool n) { randomTP = n; }
    public bool IsAlive() { return life; }
    public void IsAlive(bool n) { life = n; }
    //================================================

    // Start is called before the first frame update
    void Start()
    {
        snakeScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Snake>();
        scoreScript = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        titleScreen.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        length = snakeScript.GetBodyPartCount();
        score = scoreScript.currentScore;

        if (snakeScript.IsAlive()) {
        life = true;
        }
        else {
            life = false;
            //GameOver();
        }
    }

    private void StartGame(){

    }
    private void PauseMenu(){
        pauseMenu.SetActive(true);
    }

    private void GameOver(){
        gameOverMenu.SetActive(true);
    }

}
