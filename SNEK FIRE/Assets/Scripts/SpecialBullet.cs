using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpecialBullet : MonoBehaviour
{
    [SerializeField] private float speed = 7.5f;
    [SerializeField] private float destructionDelay = 0.1f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask layerMask;                   // Layer mask set to only affect the layers that need to be destroyed

    [SerializeField] private Vector2 boxSize = new Vector2(3, 3);   // Define the size of the 3x3 area
    private Animator bulletAnim;

    // Start is called before the first frame update
    void Start()
    {
        bulletAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        // First collision
        if (collision.CompareTag("PlayerBody") || collision.CompareTag("Wall"))
        {
            Debug.Log("Collision:" + collision.name);
            //StartCoroutine(Destroy3X3After(destructionDelay));
            DetectObjectsInArea();
        }
    }

    IEnumerator Destroy3X3After(float sec)
    {
        yield return new WaitForSeconds(sec);
        // Destroy objects in a 3x3 area
        DetectObjectsInArea();
    }

    private void DetectObjectsInArea()
    {
        Debug.Log("3X3 INITIATED");

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true; // Freeze the bullet's position

        // Get all colliders in a 3x3 area
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0, layerMask);
        Debug.Log("Colliders detected: " + colliders.Length);

        StartCoroutine(AnimateAndDestroy(colliders));               //Animate then DESTROY
    }

    IEnumerator AnimateAndDestroy(Collider2D[] colliders)
    {
        bulletAnim.SetTrigger("explode_t");                         // PLAY EXPLOSION ANIMATION
        StartCoroutine(DestroyObjects(colliders));                  // Destroy Collided Objects
        yield return new WaitForSeconds(.5f);                       // Wait for animation to end
        Destroy(gameObject);
    }

    IEnumerator DestroyObjects(Collider2D[] colliders)
    {
        yield return new WaitForSeconds(0.25f);
        //Start Destruction
        foreach (Collider2D collider in colliders)
        {
            Debug.Log("Collider tag: " + collider.tag);
            if (collider.CompareTag("PlayerBody"))
            {
                Destroy(collider.gameObject);
            }
            else if (collider.CompareTag("Wall"))
            {
                Tilemap tilemap = collider.GetComponent<Tilemap>();
                if (tilemap != null)
                {
                    // Get all tiles within the 3x3 area
                    Vector3 bulletPosition = transform.position;
                    Vector3Int cellPosition = tilemap.WorldToCell(bulletPosition);

                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            Vector3Int tilePosition = new Vector3Int(cellPosition.x + x, cellPosition.y + y, cellPosition.z);
                            tilemap.SetTile(tilePosition, null);
                        }
                    }
                }
            }
        }

        // Draw debug box
        Debug.DrawLine(transform.position - new Vector3(1.5f, 1.5f, 0), transform.position + new Vector3(1.5f, 1.5f, 0), Color.red, 2f);
        Debug.DrawLine(transform.position - new Vector3(1.5f, -1.5f, 0), transform.position + new Vector3(1.5f, -1.5f, 0), Color.red, 2f);
    }

    // Draw the overlap box in the editor for debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}