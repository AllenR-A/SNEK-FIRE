using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isGameActive = false;         //indicates if a game is ongoing

    [SerializeField] private int score;
    [SerializeField] private int scoreMultiplier = 1;
    [SerializeField] private int scoreMultiplierMax = 64;
    public int eatNumber;
    [SerializeField] private int lastEatMilestoneSpecial = 0;   //milestone for Special Food
    [SerializeField] private int lastEatMilestoneBullet = 0;    //milestone for FireBullet
    [SerializeField] private int lastEatMilestoneBomb = 0;      //milestone for Bomb
    [SerializeField] private int lastEatMilestoneLarge = 0;     //milestone for LargeFood
    [SerializeField] private int lastEatMilestoneScore = 0;     //milestone for Score Multiplier
    [SerializeField] private int length;
    [SerializeField] private int level;
    [SerializeField] private int fireBullets;
    [SerializeField] private int fireBulletsMax = 24;
    [SerializeField] private int specialBullets;
    [SerializeField] private int specialBulletsMax = 7;
    [SerializeField] private bool randomTP = false;
    [SerializeField] private bool life = true;                   //used for the Score UI
    [SerializeField] private bool bulletRefill = false;

    public GameObject pauseMenu;
    public GameObject titleScreen;
    public GameObject gameOverMenu;

    private Snake snakeScript;
    private ScoreManager scoreScript;

    public GameObject bombPrefab;
    public GameObject foodLargePrefab;
    public GameObject specialFoodPrefab;

    //================ Encapsulation ================
    public bool IsGameActive() { return isGameActive; }
    public void IsGameActive(bool n) { isGameActive = n; }
    public int GetScore() { return score; }
    public void SetScore(int n) { score = n; }
    public int GetScoreMultiplier() { return scoreMultiplier; }
    public void SetScoreMultiplier(int n) { scoreMultiplier = n; }
    public int GetEatNumber() { return eatNumber; }
    public void SetEatNumber(int n) { eatNumber = n; }
    public int GetLength() { return length; }
    public void SetLength(int n) { length = n; }
    public int GetLevel() { return level; }
    public void SetLevel(int n) { level = n; }
    public int GetFireBullets() { return fireBullets; }
    public void SetFireBullets(int n) { fireBullets = n; }
    public void AddFireBullets(int n) { fireBullets += n; }
    public int GetFireBulletsMax() { return fireBulletsMax; }
    public int GetSpecialBullets() { return specialBullets; }
    public void SetSpecialBullets(int n) { specialBullets = n; }
    public void AddSpecialBullets(int n) { specialBullets += n; }
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
        eatNumber = snakeScript.eat;

        if (snakeScript.IsAlive()) { life = true; }
        else { life = false; }

        if (fireBullets > 0) { snakeScript.CanFireRegular(true); }      //Enable Firing once there's ammo
        else { snakeScript.CanFireRegular(false); }

        if (specialBullets > 0) { snakeScript.CanFireSpecial(true); }   //Enable special firing once there's ammo
        else { snakeScript.CanFireSpecial(false); }
    }

    public void StartGame()
    {
        isGameActive = true;
        titleScreen.gameObject.SetActive(false);            //Deactivate Title Screen
    }
    public void Leaderboard()
    {
        //Go to highscore list
    }
    public void PauseMenu()
    {
        pauseMenu.SetActive(true);
    }
    public void GameOver()
    {
        gameOverMenu.SetActive(true);
    }
    public void EndGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
    public void MainMenu()
    {
        titleScreen.SetActive(true);
        gameOverMenu.SetActive(false);
    }

    public void UpdateSpawnAndAmmo()
    {
        if (isGameActive)
        {
            Debug.Log("[UpdateSpawnAndAmmo()] Length: " + length);
            if (length == 15) { bulletRefill = true; }  //Start adding ammo once snake grows past 15 in length

            int bulletInterval = 5;     // Add 1 bullet (every 5th growth, tho, only past getting length 15)
            int scrMultiInterval = 8;   // Increase Score Multipier (every 8th growth)
            int bombInterval = 9;       // SPAWN (every 9th growth)
            int largeFoodInterval = 10; // SPAWN (every 10th growth)
            int specialInterval = 15;   // SPAWN (every 15th growth)


            if (eatNumber >= lastEatMilestoneBullet + bulletInterval && bulletRefill && fireBullets < fireBulletsMax)
            {
                lastEatMilestoneBullet += bulletInterval;                                       // Update the last milestone
                fireBullets += 1;
                Debug.Log("Got ammo!");
            }
            else if (eatNumber >= lastEatMilestoneScore + scrMultiInterval && scoreMultiplier < scoreMultiplierMax)
            {
                lastEatMilestoneScore += scrMultiInterval;                                      // Update the last milestone
                scoreMultiplier *= 2;
                Debug.Log("Increased Score Multiplier!");
            }
            else if (eatNumber >= lastEatMilestoneBomb + bombInterval)
            {
                lastEatMilestoneBomb += bombInterval;                                           // Update the last milestone
                Instantiate(bombPrefab, new Vector3(24, 24, 0), Quaternion.identity);
                Debug.Log("SPAWNED Bomb.");
            }
            else if (eatNumber >= lastEatMilestoneLarge + largeFoodInterval)
            {
                lastEatMilestoneLarge += largeFoodInterval;                                     // Update the last milestone
                Instantiate(foodLargePrefab, new Vector3(24, 24, 0), Quaternion.identity);
                Debug.Log("SPAWNED LARGE.");
            }
            else if (eatNumber >= lastEatMilestoneSpecial + specialInterval)
            {
                lastEatMilestoneSpecial += specialInterval;                                     // Update the last milestone
                Instantiate(specialFoodPrefab, new Vector3(24, 24, 0), Quaternion.identity);
                Debug.Log("SPAWNED SPECIAL.");
            }
        }

    }

}
