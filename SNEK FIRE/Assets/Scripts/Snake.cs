using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    //[SerializeField] private float horizontalInput;
    //[SerializeField] private float verticalInput;
    //[SerializeField] private float speed = 30.0f;
    private Vector2 direction = Vector2.right;          //go right by default
    private List<Transform> bodyparts;
    [SerializeField] private Transform bodyPrefab;
    private bool alive = true;
    private bool attacking;

    // Start is called before the first frame update
    private void Start()
    {
        bodyparts = new List<Transform>();
        bodyparts.Add(this.transform);
    }

    // Update is called once per frame
    private void Update()
    {
        SnakeMovement();
    }

    private void SnakeMovement()
    {
        // Snake Movement
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            if (!(bodyparts.Count > 1 && direction == Vector2.down))
            {
                direction = Vector2.up;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!(bodyparts.Count > 1 && direction == Vector2.up))
            {
                direction = Vector2.down;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!(bodyparts.Count > 1 && direction == Vector2.right))
            {
                direction = Vector2.left;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!(bodyparts.Count > 1 && direction == Vector2.left))
            {
                direction = Vector2.right;
            }
        }
    }

    private void FixedUpdate()
    {
        if (alive) {
            for (int i = bodyparts.Count - 1; i > 0; i--) {
                bodyparts[i].position = bodyparts[i - 1].position;
            }
            this.transform.position = new Vector3(
                this.transform.position.x + direction.x,
                this.transform.position.y + direction.y,
                0.0f
                );
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall"){
            Debug.Log("YOU DIED.");
            alive = false;
        } else if (other.tag == "Food"){
            Grow();
        } else if (other.tag == "Player"){
            Debug.Log("YOU CRASHED ON YOURSELF.");
        }
    }

    private void Grow(){
        Transform bodypart = Instantiate(this.bodyPrefab);              // spawn new bodypart
        bodypart.position = bodyparts[bodyparts.Count - 1].position;    // set transform of new bodypart to the old one
        bodyparts.Add(bodypart);                                        // add this to the list of bodyparts

    }
}
