using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;
    //[SerializeField] private float speed = 30.0f;
    private Vector2 direction = Vector2.right;          //go right by default

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Dog Movement
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //Vector3 newPosition = new Vector3(0.0f, verticalInput, horizontalInput);    // set new position via WASD
        //transform.LookAt(transform.position + newPosition);                         // look at new position

        if (verticalInput > 0) {
            direction = Vector2.up;
        } else if (verticalInput < 0) {
            direction = Vector2.down;
        } else if(horizontalInput > 0) {
            direction = Vector2.right;
        } else if (horizontalInput < 0) {
            direction = Vector2.left;
        }
        //if (Input.GetKeyDown(KeyCode.W)) {
        //    direction = Vector2.up;
        //} else if (Input.GetKeyDown(KeyCode.S)) {
        //    direction = Vector2.down;
        //} else if (Input.GetKeyDown(KeyCode.A)) {
        //    direction = Vector2.left;
        //} else if (Input.GetKeyDown(KeyCode.D)) {
        //    direction = Vector2.right;
        //}


    }

    private void FixedUpdate()
    {
        this.transform.position = new Vector3(
            this.transform.position.x + direction.x,
            this.transform.position.y + direction.y,
            0.0f
            );
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall"){
            Debug.Log("YOU DIED.");
        }
    }
}
