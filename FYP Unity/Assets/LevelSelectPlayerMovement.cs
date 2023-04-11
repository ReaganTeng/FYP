using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 10f;

    Vector2 LastclickedPos;
    bool moving;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            LastclickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            moving = true;
        }

        if (moving && (Vector2)transform.position != LastclickedPos) {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, LastclickedPos, step);


        }
        else
        {
            moving = false;
        }
    }
}
