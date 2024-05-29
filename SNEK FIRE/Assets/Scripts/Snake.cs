using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    //for MovementInput1()
    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;
    [SerializeField] private float fire1Input;
    [SerializeField] private float fire2Input;

    [SerializeField] private Vector2Int direction = Vector2Int.right;       //Direction of head movement (go right by default) [using Vectroe2Int makes sure it sticks to the grid]
    [SerializeField] private Vector2Int bodyDirection;                      //Direction of Bodypart no.1 (used to prevent the head from colliding with the bodypart following it)
    [SerializeField] private Vector2Int tailDirection;                      //Track tail (last bodypart) direction
    [SerializeField] private Vector3 tailPositionBeforeMovement;            //Current position of the snake
    [SerializeField] private float movementIntervalForSPEED = .25f;         //[SPEED] Sets how long the interval is for each movement

    private List<GameObject> bodyparts;
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private bool alive = true;
    private bool isAttacking = false;

    private Animator playerAnim;
    private Animator headAnim;

    // Start is called before the first frame update
    private void Start()
    {
        bodyparts = new List<GameObject>();                                         //create list
        bodyparts.Add(gameObject);                                                  //add the head to the list
        headAnim = bodyparts[0].GetComponent<Animator>();                           //bodyparts[0] is now the head of the snake

        StartCoroutine(MoveSnake());                                                //start the moving coroutine (the inputs are handled by Update() for per-frame updates)
    }

    // Update is called once per frame
    private void Update()
    {
        Input1();                                                                   //Check for inputs every frame
    }

    private void FixedUpdate()
    {
    }

    private void Input1() {
        // Snake Movement [original style || using GetAxis]
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        fire1Input = Input.GetAxisRaw("Fire1");
        fire2Input = Input.GetAxisRaw("Fire2");

        // Get Direction of Body no.1 (help prevent head from moving back on itself and hitting said body)
        Vector3 body1_Direction = Vector3.zero;
        if (bodyparts.Count > 1) {
            body1_Direction = bodyparts[0].transform.position - bodyparts[1].transform.position;
        } else {
            // Get direction of Head (as it is also the current body). While it currently CAN reverse movement, this would be useful when it gains its first bodypart.
            Vector3 positionAfterMovement = new Vector3((int)this.transform.position.x, (int)this.transform.position.y, 0);
            body1_Direction = positionAfterMovement - tailPositionBeforeMovement;
        }
        bodyDirection = new Vector2Int((int)body1_Direction.x, (int)body1_Direction.y);

        if (verticalInput == 1) {
            if (!(bodyparts.Count > 1 && bodyDirection == Vector2Int.down))
            { direction = Vector2Int.up; }
        } else if (verticalInput == -1) {
            if (!(bodyparts.Count > 1 && bodyDirection == Vector2Int.up))
            { direction = Vector2Int.down; }
        } else if(horizontalInput == 1) {
            if (!(bodyparts.Count > 1 && bodyDirection == Vector2Int.left))
            { direction = Vector2Int.right; }
        } else if (horizontalInput == -1) {
            if (!(bodyparts.Count > 1 && bodyDirection == Vector2Int.right))
            { direction = Vector2Int.left; }
        }

        if (fire1Input == 1 && !isAttacking) StartCoroutine(Fire1());               //isAttacking prevents multiple calls (as this is updated every frame)
        else if (fire2Input == 1 && !isAttacking) StartCoroutine(Fire2());          //isAttacking prevents multiple calls (as this is updated every frame)

    }
    private void Input2() {
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

    IEnumerator Fire1()
    {
        isAttacking = true;                                                         // enable flag to prevent multiple calls
        //Code to launch regular Fire Bullet from FirePoint GameObject
        headAnim.SetTrigger("attack_t");                                            // PLAY ANIMATION
        Debug.Log("Fire1() called");
        yield return new WaitForSeconds(.69f);                                      // The animation is 41 frames (or around ~0.69s)
        isAttacking = false;                                                        // disable flag after animation
    }

    IEnumerator Fire2()
    {
        isAttacking = true;                                                         // enable flag to prevent multiple calls
        //Code to launch Special Bullet from FirePoint GameObject
        headAnim.SetTrigger("attack_t");                                            // PLAY ANIMATION
        Debug.Log("Fire2() called");
        yield return new WaitForSeconds(.69f);                                      // The animation is 41 frames (or around ~0.69s)
        isAttacking = false;                                                        // disable flag after animation
    }

    IEnumerator MoveSnake()
    {
        while (alive)
        {
            //save tail position before moving
            if (bodyparts.Count > 1)
            { tailPositionBeforeMovement = new Vector3(bodyparts[bodyparts.Count - 1].transform.position.x, bodyparts[bodyparts.Count - 1].transform.position.y, 0); }
            else
            { tailPositionBeforeMovement = new Vector3(this.transform.position.x, this.transform.position.y, 0); }

            //Move Bodyparts
            for (int i = bodyparts.Count - 1; i > 0; i--)                       // for each bodypart, go in reverse, repeat until it the last one is positioned to where the head is.
            {
                //Debug.Log("bodyparts[" + i + "].transform.position = bodyparts[" + (i - 1) + "].transform.position");
                bodyparts[i].transform.position = bodyparts[i - 1].transform.position;              // position it to the one next to it (set to a lower number)
                bodyparts[i].transform.eulerAngles = bodyparts[i - 1].transform.eulerAngles;        // also follow the rotation

            }

            //Move Snake Head
            this.transform.position = new Vector3(
                this.transform.position.x + direction.x,
                this.transform.position.y + direction.y,
                0.0f
                );

            //Turn Snake Head
            this.transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector2Int(direction)+90);

            SetTailDirection(tailPositionBeforeMovement);
            yield return new WaitForSeconds(movementIntervalForSPEED);
        }
    }
    private void MoveBack()
    {
        //So that the snake doesn't clip through the wall when it dies
        //Move Bodyparts
        for (int i = 0; i < bodyparts.Count - 1; i++) {                         //can reuse old one since that one already works in reverse, but the tail-end won't move yet.
            //Debug.Log("bodyparts[" + i + "].transform.position = bodyparts[" + (i + 1) + "].transform.position");
            bodyparts[i].transform.position = bodyparts[i + 1].transform.position;                  // position it to the one before to it "[i + 1]" (set to a higher numbered bodypart)
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
        Transform tail = bodyparts[bodyparts.Count - 1].transform;
        tail.transform.position = new Vector3(
            tail.transform.position.x + reverseDirection.x,
            tail.transform.position.y + reverseDirection.y,
            0.0f
            );
    }

    private float GetAngleFromVector2Int(Vector2Int direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360; 
        return angle;
    }

    private void SetTailDirection(Vector3 tailPositionBeforeMovement)
    {
        Vector3 getDirection = Vector3.zero;
        //Keep updating tail direction tracker
        if (bodyparts.Count > 1) {
            //Debug.Log("[MoveSnake()] Body2ndtolasst POS: " + bodyparts[bodyparts.Count - 1].transform.position);
            //Debug.Log("[MoveSnake()] Previous POS: " + tailPositionBeforeMovement);
            getDirection = bodyparts[bodyparts.Count - 1].transform.position - tailPositionBeforeMovement;
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
        for (int i = 0; i <= bodyparts.Count - 1; i++)                              //using "<=" this time to loop through all the bodyparts from head -> tail
        {
            playerAnim = bodyparts[i].GetComponent<Animator>();
            playerAnim.SetBool("death_b", true);                                    // SET DEATH SPRITE FOR EACH BODYPART
        }
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
        GameObject bodypart = Instantiate(this.bodyPrefab);                          // spawn new bodypart (and get transform)

        /*Calculate position of the new tail end using the old tail direction to spawn it behind
        (the old way spawns it on the tail or head at the very first growth can cause a collision with itself) */
        Vector3 oldTailPosition = bodyparts[bodyparts.Count - 1].transform.position;
        Vector3 oldTailRotation = bodyparts[bodyparts.Count - 1].transform.eulerAngles;

        //Debug.Log("[GROW()] OLD POS: " + oldTailPosition);
        //Debug.Log("[GROW()] tailDirection X: " + tailDirection.x + "tailDirection Y: " + tailDirection.y);
        Vector3 newTailPosition = oldTailPosition - new Vector3(tailDirection.x, tailDirection.y, 0);   //Offsetting new body
        //Debug.Log("[GROW()] NEW POS: " + newTailPosition);
        bodypart.transform.position = newTailPosition;                                        // set transform of new bodypart to the old one (replacing tail-end)
        bodypart.transform.eulerAngles = oldTailRotation;                                     // set rotation of new bodypart to the old one (replacing tail-end)
        bodyparts.Add(bodypart);                                                    // add this to the list of bodyparts
    }
}
