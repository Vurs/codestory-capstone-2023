using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rbAns;

    public float speed = 3.0f;


    void Start()
    {
        
    }

    private void Update()
    {
        rbAns.velocity  = new Vector2(speed* -150, 0);

        //-519 -> x

        if (transform.position.x < -570) {
            Debug.Log("Off screen");
        }
    }
}
