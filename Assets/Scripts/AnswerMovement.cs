using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;

public class AnswerMovement : MonoBehaviour
{
    public Rigidbody2D rbAns;

    private Vector2 firstStartPostion;
    private Vector2 secondStartPostion;
    //Canvas canvas = FindObjectOfType<Canvas>();
    //float width = 0;
    private Camera camera;

    public textChange textChangeObject;

    void Start()
    {
        firstStartPostion = GameObject.FindWithTag("Answer").transform.position;
        secondStartPostion = GameObject.FindWithTag("WrongAns").transform.position;
        //width = canvas.GetComponent<RectTransform>().rect.width;
        camera = Camera.main;
    }
}
