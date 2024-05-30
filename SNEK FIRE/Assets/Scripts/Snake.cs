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

    [SerializeField] private Vector2Int direction = Vector2Int.zero;        //Direction of head movement [using Vectroe2Int makes sure it sticks to the grid]
    [SerializeField] private Transform rotation;                            //Rotation of head
    [SerializeField] private Vector2Int bodyDirection;                      //Direction of Bodypart no.1 (used to prevent the head from colliding with the bodypart following it)
    [SerializeField] private Vector2Int tailDirection;                      //Track tail (last bodypart) direction
    [SerializeField] private Vector3 tailPositionBeforeMovement;            //Current position of the snake
    [SerializeField] private float movementIntervalForSPEED = .25f;         //[SPEED] Sets how long the interval is for each movement

    //Snake
    private List<GameObject> bodyparts;
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private bool alive = true;

    //Bullets
    [SerializeField] private GameObject fireBulletPrefab;
    [SerializeField] private GameObject specialBulletPrefab;
    [SerializeField] private Transform firePoint;
    private bool isAttacking = false;


    [SerializeField] private bool canFireRegular = false;                   //Is enabled if GameManager has stock of fire bullets
    [SerializeField] private bool canFireSpecial = false;                   //Is enabled if GameManager has stock of special bullets


    private Animator playerAnim;
    private Animator headAnim;

    //================ Encapsulation ================
    public bool IsAlive() { return alive; }
    public void IsAlive(bool status) { alive = status; }
    public bool IsAttacking() { return isAttacking; }
    public void IsAttacking(bool status) { isAttacking = status; }
    public bool CanFireRegular() { return canFireRegular; }
    public void CanFireRegular(bool status) { canFireRegular = status; }
    public bool CanFireSpecial() { return canFireSpecial; }
    public void CanFireSpecial(bool status) { canFireSpecial = status; }
    //================================================

    private ScoreManager scoreManager;

    // Start is called before the first frame update
    private void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        
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
            if (!(bodyparts.Count > 1 && bodyDirection == Vector2Int.down)) {
                if (bodyDirection == new Vector2Int(0, 8)) direction = Vector2Int.down;         //Keep going down (while body is teleported and moving down => Vector2Int(0,8))
                else direction = Vector2Int.up;                                                 //Prevent going up (while body is moving down)
            }          
        } else if (verticalInput == -1) {
            if (!(bodyparts.Count > 1 && bodyDirection == Vector2Int.up)) {
                if (bodyDirection == new Vector2Int(0, -8)) direction = Vector2Int.up;          //Keep going up (while body is teleported and moving up => Vector2Int(0,-8))
                else direction = Vector2Int.down;                                               //Prevent going down (while body is moving up)
            }
        } else if(horizontalInput == 1) {
            if (!(bodyparts.Count > 1 && bodyDirection == Vector2Int.left)) {
                if (bodyDirection == new Vector2Int(18, 0)) direction = Vector2Int.left;        //Keep going left (while body is teleported and moving left => Vector2Int(18,0))
                else direction = Vector2Int.right;                                              //Prevent going right (while body is moving left)
            }
        } else if (horizontalInput == -1) {
            if (!(bodyparts.Count > 1 && bodyDirection == Vector2Int.right)) {
                if (bodyDirection == new Vector2Int(-18, 0)) direction = Vector2Int.right;      //Keep going right (while body is teleported and moving right => Vector2Int(-18,0))
                else direction = Vector2Int.left;                                               //Prevent going left (while body is moving right)
            }
        }

        if (fire1Input == 1 && !isAttacking && canFireRegular) StartCoroutine(Fire1());         //isAttacking prevents multiple calls (as this is updated every frame)
        else if (fire2Input == 1 && !isAttacking && canFireSpecial) StartCoroutine(Fire2());    //isAttacking prevents multiple calls (as this is updated every frame)

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
        headAnim.SetTrigger("attack_t");                                            // PLAY ANIMATION
        Instantiate(fireBulletPrefab, firePoint.position, firePoint.rotation);      // spawn fire bullet
        Debug.Log("Fire1() called");
        yield return new WaitForSeconds(.69f);                                      // The animation is 41 frames (or around ~0.69s)
        isAttacking = false;                                                        // disable flag after animation
    }

    IEnumerator Fire2()
    {
        isAttacking = true;                                                         // enable flag to prevent multiple calls
        headAnim.SetTrigger("attack_t");                                            // PLAY ANIMATION
        Instantiate(specialBulletPrefab, firePoint.position, firePoint.rotation);   // spawn special bullet
        Debug.Log("Fire2() called");
        yield return new WaitForSeconds(.69f);                                      // The animation is 41 frames (or around ~0.69s)
        isAttacking = false;                                                        // disable flag after animation
    }

    private void Teleport()
    {
        //Teleport Snake to the opposite side if it reaches the borders of the screen
        if (transform.position.x == 0) transform.position = new Vector3(transform.position.x + 19, transform.position.y, 0.0f);
        else if (transform.position.x == 20) transform.position = new Vector3(transform.position.x - 19, transform.position.y, 0.0f);
        else if (transform.position.y == 0) transform.position = new Vector3(transform.position.x, transform.position.y + 9, 0.0f);
        else if (transform.position.y == 10) transform.position = new Vector3(transform.position.x, transform.position.y - 9, 0.0f);
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
            if (direction != Vector2.zero) { this.transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector2Int(direction) + 90); }

            SetTailDirection(tailPositionBeforeMovement);
            Teleport();                                                                         
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

        Teleport();
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

    public void HandleBodyPartHit(GameObject hitBodyPart, float time)
    {
        int hitIndex = bodyparts.IndexOf(hitBodyPart);
        if (hitIndex != -1)
        {

            // Destroy all body parts from tail to hitIndex
            for (int i = bodyparts.Count - 1; i > hitIndex; i--)                    // Don't include the exploded bodypart
            {
                playerAnim = bodyparts[i].GetComponent<Animator>();
                playerAnim.SetBool("death_b", true);                                // SET DEATH SPRITE FOR EACH BODYPART
                bodyparts[i].tag = "DeadBody";                                      // Change Tags to "DeadBody"
                bodyparts.RemoveAt(i);                                              // Remove from list (detaches from the snake)
            }
            bodyparts.RemoveAt(hitIndex);                                           // remove separately

            Debug.Log("Body parts destroyed starting from index: " + hitIndex);
            StartCoroutine(DelayDestroy(time, hitBodyPart));
        }
    }

    public void HandleBodyPartHitSpecial(GameObject hitBodyPart)
    {
        // This one is the same as above. It just doesn't have animations (since that one is already handled by the special bullet).
        int hitIndex = bodyparts.IndexOf(hitBodyPart);
        if (hitIndex != -1)
        {
            // Destroy all body parts from hitIndex to the end
            for (int i = bodyparts.Count - 1; i >= hitIndex; i--)
            {
                playerAnim = bodyparts[i].GetComponent<Animator>();
                playerAnim.SetBool("death_b", true);                                // SET DEATH SPRITE FOR EACH BODYPART
                bodyparts[i].tag = "DeadBody";                                      // Change Tags to "DeadBody"
                bodyparts.RemoveAt(i);                                              // Remove from list (detaches from the snake)
            }

            // Optionally, you could add some effect or animation when parts are removed
            Debug.Log("Body parts destroyed starting from index: " + hitIndex);
            Destroy(hitBodyPart);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Wall" && alive) {
            Debug.Log("YOU DIED.");
            Death();
        } else if (other.tag == "Food") {
            Debug.Log("YOU ATE.");
            scoreManager.UpdateScore(1);
            Grow();
        } else if (other.tag == "PlayerBody" && alive){
            Debug.Log("YOU CRASHED ON YOURSELF.");
            Death();
        } else if (other.tag == "BombBait" && alive) {
            Debug.Log("YOU GOT JEBAITED *BOOM*.");
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

    IEnumerator DelayDestroy(float time, GameObject gO)
    {
        yield return new WaitForSeconds(time);                  // Wait for animation to end
        Destroy(gO);
    }
}
