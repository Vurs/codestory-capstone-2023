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

    void Start()
    {
        firstStartPostion = GameObject.FindWithTag("Answer").transform.position;
        secondStartPostion = GameObject.FindWithTag("WrongAns").transform.position;
    }

    private void Update()
    {
        rbAns.velocity = new Vector2(speed * -150, 0);

        var ans = GameObject.FindWithTag("Answer").transform.position.x;
        var wrongAns = GameObject.FindWithTag("WrongAns").transform.position.x;

        if (ans < -502)
        {
            transform.position = firstStartPostion;
        }
        //if (wrongAns < -502)
        //{
        //    transform.position = secondStartPostion;
        //}

    }
}
