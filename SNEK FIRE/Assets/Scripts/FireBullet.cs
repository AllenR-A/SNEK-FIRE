using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    [SerializeField] private float speed = 14f;
    [SerializeField] private Rigidbody2D rb;
    private Animator bulletAnim;
    private Snake snakeScript;

    // Start is called before the first frame update
    void Start()
    {
        snakeScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Snake>();

        bulletAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        transform.Rotate(0, 0, 90);                     /*Rotate to the correct orientation after velocity is set
                    snakeScript                                     (if this is done prior to setting velocity, it shoots into the wrong direction)*/
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // First collision
        if (collision.CompareTag("PlayerBody"))
        {
            Debug.Log("Collision:" + collision.name);
            if (snakeScript != null)
            {
                // Call snake's method to handle body-part destruction
                snakeScript.HandleBodyPartHit(collision.gameObject);
            }
        }

        else if (collision.CompareTag("DeadBody"))
        {
            Debug.Log("Collision:" + collision.name);
            StartCoroutine(AnimateAndDestroy(0.5f));            //Explode upon contact
        }

        else if (collision.CompareTag("Wall"))
        {
            Debug.Log("Collision:" + collision.name);
            StartCoroutine(AnimateAndDestroy(0.5f));            //Explode upon contact
        }
    }

    IEnumerator AnimateAndDestroy(float time)
    {
        // Freeze the bullet's position (as it hits the wall)
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;

        bulletAnim.SetTrigger("explode_t");                     // PLAY EXPLOSION ANIMATION
        yield return new WaitForSeconds(time);                  // Wait for animation to end
        Destroy(gameObject);
    }
}
