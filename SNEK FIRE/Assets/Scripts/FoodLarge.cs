using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodLarge : MonoBehaviour
{
    [SerializeField] private int foodPoints = 15;   //How much this food is worth

    //For spawning
    private BoxCollider2D area;
    [SerializeField] private Vector2 spawnPosition; // Position where you want to spawn the item
    [SerializeField] private Vector2 boxSize;       // Size of the box cast
    private float boxAngle = 0f;                    // Angle of the box cast in degrees
    [SerializeField] private LayerMask layerMask;   // Layer mask to define which layers to check against

    private Food foodScript;

    //================ Encapsulation ================
    public int GetPoints() { return foodPoints; }
    //================================================

    // Start is called before the first frame update
    void Start()
    {
        foodScript = GameObject.FindGameObjectWithTag("Food").GetComponent<Food>();
        area = GameObject.FindGameObjectWithTag("SpawnArea").GetComponent<BoxCollider2D>();
        layerMask = LayerMask.GetMask("Obstacles");
        RandomLocation();
    }

    private void RandomLocation()
    {
        Bounds bounds = this.area.bounds;

        float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

        spawnPosition = new Vector2(x, y);
        boxSize = new Vector2(1, 1);

        if (foodScript.CanSpawnItem(spawnPosition, boxSize, boxAngle, layerMask)) { this.transform.position = new Vector3(x, y, 0); }
        else RandomLocation();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("BOOM.");
            Destroy(gameObject);                // Self Destruct
        }
    }
}
