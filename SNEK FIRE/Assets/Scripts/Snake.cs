using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    //for MovementInput1()
    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;
    [SerializeField] private float speed = 30.0f;


    [SerializeField] private Vector2Int direction = Vector2Int.right;       //Direction of movement (go right by default) [using Vectroe2Int makes sure it sticks to the grid]
    [SerializeField] private Vector3 tailPositionBeforeMovement;            //Current position of the snake
    [SerializeField] private Vector2Int tailDirection;                      //Track tail direction
    [SerializeField] private float movementInterval = .25f;                 //[SPEED] Sets how long the interval is for each movement

    private List<Transform> bodyparts;
    [SerializeField] private Transform bodyPrefab;
    [SerializeField] private bool alive = true;
    private bool attacking;

    // Start is called before the first frame update
    private void Start()
    {
        bodyparts = new List<Transform>();
        bodyparts.Add(this.transform);

        StartCoroutine(MoveSnake());
    }

    // Update is called once per frame
    private void Update()
    {
        MovementInput1();
    }

    private void FixedUpdate()
    {
    }

    private void MovementInput1() {

        // Snake Movement [original style || using GetAxis]

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Vector3 newPosition = new Vector3(0.0f, verticalInput, horizontalInput);    // set new position via WASD
        //transform.LookAt(transform.position + newPosition);                         // look at new position

        if (verticalInput == 1) {
            if (!(bodyparts.Count > 1 && direction == Vector2Int.down))
            { direction = Vector2Int.up; }
        } else if (verticalInput == -1) {
            if (!(bodyparts.Count > 1 && direction == Vector2Int.up))
            { direction = Vector2Int.down; }
        } else if(horizontalInput == 1) {
            if (!(bodyparts.Count > 1 && direction == Vector2Int.left))
            { direction = Vector2Int.right; }
        } else if (horizontalInput == -1) {
            if (!(bodyparts.Count > 1 && direction == Vector2Int.right))
            { direction = Vector2Int.left; }
        }
    }

    private void MovementInput2() {

        // Snake Movement [using KeyCode]
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            if (!(bodyparts.Count > 1 && direction == Vector2Int.down)) {
                direction = Vector2Int.up;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            if (!(bodyparts.Count > 1 && direction == Vector2Int.up)) {
                direction = Vector2Int.down;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (!(bodyparts.Count > 1 && direction == Vector2Int.right)) {
                direction = Vector2Int.left;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            if (!(bodyparts.Count > 1 && direction == Vector2Int.left)) {
                direction = Vector2Int.right;
            }
        }
    }


    IEnumerator MoveSnake()
    {
        while (alive)
        {
            //save tail position before moving
            if (bodyparts.Count > 1)
            { tailPositionBeforeMovement = new Vector3(bodyparts[bodyparts.Count - 1].position.x, bodyparts[bodyparts.Count - 1].position.y, 0); }
            else
            { tailPositionBeforeMovement = new Vector3(this.transform.position.x, this.transform.position.y, 0); }

            //Move Bodyparts
            for (int i = bodyparts.Count - 1; i > 0; i--)                       // for each bodypart, go in reverse, repeat until it the last one is positioned to where the head is.
            {
                //Debug.Log("bodyparts[" + i + "].position = bodyparts[" + (i - 1) + "].position");
                bodyparts[i].position = bodyparts[i - 1].position;              // position it to the one next to it (set to a lower number)

            }

            //Move Snake Head
            this.transform.position = new Vector3(
                this.transform.position.x + direction.x,
                this.transform.position.y + direction.y,
                0.0f
                );

            SetTailDirection(tailPositionBeforeMovement);
            yield return new WaitForSeconds(movementInterval);
        }
    }
    private void MoveBack()
    {
        //So that the snake doesn't clip through the wall when it dies
        //Move Bodyparts
        for (int i = 0; i < bodyparts.Count - 1; i++) {                         //can reuse old one since that one already works in reverse, but the tail-end won't move yet.
            //Debug.Log("bodyparts[" + i + "].position = bodyparts[" + (i + 1) + "].position");
            bodyparts[i].position = bodyparts[i + 1].position;                  // position it to the one before to it "[i + 1]" (set to a higher numbered bodypart)
        }

        //Reverse direction manually (for the tail)
        Vector2Int reverseDirection = new Vector2Int(0, 0);
        if (tailDirection == Vector2Int.down)
        { reverseDirection = Vector2Int.up; }
        else if (tailDirection == Vector2Int.up)
        { reverseDirection = Vector2Int.down; }
        else if (tailDirection == Vector2Int.left)
        { reverseDirection = Vector2Int.right; }
        else if (tailDirection == Vector2Int.right)
        { reverseDirection = Vector2Int.left; }

        //Move Snake Tail Back
        Transform tail = bodyparts[bodyparts.Count - 1];
        tail.transform.position = new Vector3(
            tail.transform.position.x + reverseDirection.x,
            tail.transform.position.y + reverseDirection.y,
            0.0f
            );
    }

    private void SetTailDirection(Vector3 tailPositionBeforeMovement)
    {
        Vector3 getDirection = Vector3.zero;
        //Keep updating tail direction tracker
        if (bodyparts.Count > 1) {
            //Debug.Log("[MoveSnake()] Body2ndtolasst POS: " + bodyparts[bodyparts.Count - 1].position);
            //Debug.Log("[MoveSnake()] Previous POS: " + tailPositionBeforeMovement);
            getDirection = bodyparts[bodyparts.Count - 1].position - tailPositionBeforeMovement;
        } else {
            //Get direction of Head (as it is also the current tail)
            Vector3 positionAfterMovement = new Vector3((int)this.transform.position.x, (int)this.transform.position.y, 0);
            getDirection = positionAfterMovement - tailPositionBeforeMovement;
        }
        tailDirection = new Vector2Int((int)getDirection.x, (int)getDirection.y);
        //Debug.Log("[MoveSnake()] Tail Direction [Vector2Int]: " + tailDirection);
    }

    private void Death() {
        alive = false;      //must activate first [as MoveBack() makes the head crash into the body, making this run again]
        MoveBack();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall" && alive) {
            Debug.Log("YOU DIED.");
            Death();
        } else if (other.tag == "Food"){
            Grow();
        } else if (other.tag == "Player" && alive){
            Debug.Log("YOU CRASHED ON YOURSELF.");
            Death();
        }
    }

    private void Grow(){
        Transform bodypart = Instantiate(this.bodyPrefab);              // spawn new bodypart (and get transform)

        /*Calculate position of the new tail end using the old tail direction to spawn it behind
        (the old way spawns it on the tail or head at the very first growth can cause a collision with itself) */
        Vector3 oldTailPosition = bodyparts[bodyparts.Count - 1].position;

        //Debug.Log("[GROW()] OLD POS: " + oldTailPosition);
        //Debug.Log("[GROW()] tailDirection X: " + tailDirection.x + "tailDirection Y: " + tailDirection.y);
        Vector3 newTailPosition = oldTailPosition - new Vector3(tailDirection.x, tailDirection.y, 0);
        //Debug.Log("[GROW()] NEW POS: " + newTailPosition);
        bodypart.position = newTailPosition;                            // set transform of new bodypart to the old one (replacing tail-end)
        bodyparts.Add(bodypart);                                        // add this to the list of bodyparts
    }
}
