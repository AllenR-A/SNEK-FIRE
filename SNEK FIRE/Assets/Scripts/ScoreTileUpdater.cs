using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScoreTileUpdater : MonoBehaviour
{
    //Characters
    [SerializeField] private Tile specialAmmo;
    [SerializeField] private Tile heartFull;
    [SerializeField] private Tile heartEmpty;
    [SerializeField] private Tile num0;
    [SerializeField] private Tile num1;
    [SerializeField] private Tile num2;
    [SerializeField] private Tile num3;
    [SerializeField] private Tile num4;
    [SerializeField] private Tile num5;
    [SerializeField] private Tile num6;
    [SerializeField] private Tile num7;
    [SerializeField] private Tile num8;
    [SerializeField] private Tile num9;
    [SerializeField] private Tile gold0;
    [SerializeField] private Tile gold1;
    [SerializeField] private Tile gold2;
    [SerializeField] private Tile gold3;
    [SerializeField] private Tile gold4;
    [SerializeField] private Tile gold5;
    [SerializeField] private Tile gold6;
    [SerializeField] private Tile gold7;
    [SerializeField] private Tile gold8;
    [SerializeField] private Tile gold9;

    //Positions To Edit
    [SerializeField] private Vector3Int heart1;
    [SerializeField] private Vector3Int specialAmmo1;
    [SerializeField] private Vector3Int specialAmmo2;
    [SerializeField] private Vector3Int specialAmmo3;
    [SerializeField] private Vector3Int specialAmmo4;
    [SerializeField] private Vector3Int specialAmmo5;
    [SerializeField] private Vector3Int specialAmmo6;
    [SerializeField] private Vector3Int specialAmmo7;
    [SerializeField] private Vector3Int bulletCount1;
    [SerializeField] private Vector3Int bulletCount2;
    [SerializeField] private Vector3Int multiplier1;
    [SerializeField] private Vector3Int multiplier2;
    [SerializeField] private Vector3Int level1;

    //Tilemap
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        UpdateUI();                 //Keep Updating
    }

    private void UpdateUI()
    {
        UpdateHearts();             //Update Life Status
        UpdateLevel();              //Update Level
        UpdateSpecialAmmo();        //Update Special Ammo Amount
        UpdateBulletCount();        //Update Regular Ammo Amount
        UpdateMultiplier();         //Update Score Multiplier
    }


    private void UpdateHearts()
    {
        tilemap.SetTile(heart1, gameManager.IsAlive() ? heartFull : heartEmpty);
    }

    private void UpdateLevel()
    {
        int level = gameManager.GetLevel();
        SetTileForNumber(level1, level);
    }

    private void UpdateSpecialAmmo()
    {
        int specialBullets = gameManager.GetSpecialBullets();
        Vector3Int[] positions = { specialAmmo1, specialAmmo2, specialAmmo3, specialAmmo4, specialAmmo5, specialAmmo6, specialAmmo7 };
        for (int i = 0; i < positions.Length; i++)
        {
            tilemap.SetTile(positions[i], i < specialBullets ? specialAmmo : null);     //iterate through the tiles and set either an ammo idicator or null.
        }
    }

    private void UpdateBulletCount()
    {
        int bullets = gameManager.GetFireBullets();
        SetTileForNumber(bulletCount1, bullets / 10);       //get tens and set them to the correct tile using switch statements
        SetTileForNumber(bulletCount2, bullets % 10);       //get units and set them to the correct tile using switch statements
    }

    private void UpdateMultiplier()
    {
        int multiplier = gameManager.GetScoreMultiplier();
        SetTileForNumber(multiplier1, multiplier / 10);     //get tens and set them to the correct tile using switch statements
        SetTileForNumber(multiplier2, multiplier % 10);     //get units and set them to the correct tile using switch statements
    }

    private void SetTileForNumber(Vector3Int position, int number)
    {
        switch (number)
        {
            case 0: tilemap.SetTile(position, num0); break;
            case 1: tilemap.SetTile(position, num1); break;
            case 2: tilemap.SetTile(position, num2); break;
            case 3: tilemap.SetTile(position, num3); break;
            case 4: tilemap.SetTile(position, num4); break;
            case 5: tilemap.SetTile(position, num5); break;
            case 6: tilemap.SetTile(position, num6); break;
            case 7: tilemap.SetTile(position, num7); break;
            case 8: tilemap.SetTile(position, num8); break;
            case 9: tilemap.SetTile(position, num9); break;
        }
    }

}
