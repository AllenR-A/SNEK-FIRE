using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private int foodPoints;        //How much this food is worth

    //For spawning
    private BoxCollider2D area;
    [SerializeField] private Vector2 spawnPosition; // Position where you want to spawn the item
    [SerializeField] private Vector2 boxSize;       // Size of the box cast
    private float boxAngle = 0f;                    // Angle of the box cast in degrees
    [SerializeField] private LayerMask layerMask;   // Layer mask to define which layers to check against

    public bool CanSpawnItem(Vector2 position, Vector2 size, float angle)
    {
        // Perform the BoxCast
        RaycastHit2D hit = Physics2D.BoxCast(position, size, angle, Vector2.zero, 0f, layerMask);
        
        // Return true if no collider was hit, false otherwise
        return hit.collider == null;
    }

    // Start is called before the first frame update
    private void Start()
    {
        //layerMask = LayerMask.GetMask("Obstacles");
        area = GameObject.FindGameObjectWithTag("SpawnArea").GetComponent<BoxCollider2D>();
        RandomLocation();
    }

    private void RandomLocation()
    {
        Bounds bounds = this.area.bounds;

        float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

        spawnPosition = new Vector2(x,y);
        boxSize = new Vector2(1,1);

        if (CanSpawnItem(spawnPosition, boxSize, boxAngle)) {
            this.transform.position = new Vector3(x, y, 0);   //rounded comply within the grid
        } else {
            RandomLocation();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            RandomLocation();
        } else if (other.tag == "Wall") {       //still spawns on top of the wall sprite. might have to check on that Physics2D.BoxCast thing  (also found a "Collider2D.Cast" thing but I'm not sure how this one wors either)
            RandomLocation();
        }
    }
}
