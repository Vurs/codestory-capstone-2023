using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;

public class AnswerMovement : MonoBehaviour
{
    public Rigidbody2D rbAns;

    public float speed = 3.0f;


    private Vector2 firstStartPostion;
    private Vector2 secondStartPostion;
    //Canvas canvas = FindObjectOfType<Canvas>();
    //float width = 0;
    private Camera camera;


    void Start()
    {
        firstStartPostion = GameObject.FindWithTag("Answer").transform.position;
        //secondStartPostion = GameObject.FindWithTag("WrongAns").transform.position;
        //width = canvas.GetComponent<RectTransform>().rect.width;
        camera = Camera.main;

    }

    private void Update()
    {
        rbAns.velocity = new Vector2(speed * -150, 0);        

        // Get the viewport position of the object
        Vector3 viewportPosition = camera.WorldToViewportPoint(transform.position);

        // Check if the object is off-screen
        if (viewportPosition.x < 0)
        {
            // The object is off-screen, you can perform your off-screen logic here
            int rand = Random.Range(1, 3);
            if (rand == 1) {
                transform.position = firstStartPostion;
            }
            if (rand == 2) { 
                transform.position = secondStartPostion;
            }

        }

    }
}
