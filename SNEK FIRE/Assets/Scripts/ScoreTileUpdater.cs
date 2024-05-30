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

    private GameManager gameManager;

    [ContextMenu("Paint")]
    void Paint()
    {
        tilemap.SetTile(/*insertposition*/, /*insertcharacter*/);
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
}
