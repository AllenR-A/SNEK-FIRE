using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    [SerializeField] private float speed = 14f;
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        transform.Rotate(0, 0, 90);                     /*Rotate to the correct orientation after velocity is set
                                                         (if this is done prior to setting velocity, it shoots into the wrong direction)*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
