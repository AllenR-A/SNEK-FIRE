using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBait : MonoBehaviour
{
    //For spawning
    private BoxCollider2D area;
    [SerializeField] private Vector2 spawnPosition; // Position where you want to spawn the item
    [SerializeField] private Vector2 boxSize;       // Size of the box cast
    private float boxAngle = 0f;                    // Angle of the box cast in degrees
    [SerializeField] private LayerMask layerMask;   // Layer mask to define which layers to check against

    private Animator bombAnim;
    private Food foodScript;

    // Start is called before the first frame update
    void Start()
    {
        foodScript = GameObject.FindGameObjectWithTag("Food").GetComponent<Food>();

        bombAnim = GetComponent<Animator>();
        //layerMask = LayerMask.GetMask("Obstacles");
        area = GameObject.FindGameObjectWithTag("SpawnArea").GetComponent<BoxCollider2D>();
        RandomLocation();
    }

    private void RandomLocation()
    {
        Bounds bounds = this.area.bounds;

        float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

        spawnPosition = new Vector2(x, y);
        boxSize = new Vector2(1, 1);

        if (foodScript.CanSpawnItem(spawnPosition, boxSize, boxAngle)) {
            this.transform.position = new Vector3(x, y, 0);
            StartCoroutine(Timer(10f));                         // Start timer once spot is found
        }
        else RandomLocation();
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);                  // Time is its entire existence
        StartCoroutine(AnimateAndDestroy());                    // Start destruction
    }

    IEnumerator AnimateAndDestroy()
    {
        bombAnim.SetTrigger("explode_t");                       // PLAY EXPLOSION ANIMATION
        yield return new WaitForSeconds(.5f);                   // Wait for animation to end
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("BOOM.");
            StartCoroutine(AnimateAndDestroy());                // Start destruction
        }
    }
}
