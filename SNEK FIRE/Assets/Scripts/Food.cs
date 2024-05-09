using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D area;

    // Start is called before the first frame update
    private void Start()
    {
        RandomLocation();
    }

    private void RandomLocation()
    {
        Bounds bounds = this.area.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0);   //rounded comply within the grid
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
