using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerPositionHandler : MonoBehaviour
{
    public Rigidbody2D answerRb;
    public Rigidbody2D wrongAnswerRb;

    public float speed = 3.0f;

    private Vector2 firstStartPostion;
    private Vector2 secondStartPostion;
    private Camera camera;

    public textChange textChangeObject;

    bool answerOutOfBounds = false;
    bool wrongAnswerOutOfBounds = false;

    public bool answerHit = false;

    void Start()
    {
        firstStartPostion = GameObject.FindWithTag("Answer").transform.position;
        secondStartPostion = GameObject.FindWithTag("WrongAns").transform.position;
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        answerRb.velocity = new Vector2(speed * -1, 0);
        wrongAnswerRb.velocity = new Vector2(speed * -1, 0);

        // Get the viewport position of the object
        Vector3 viewportPositionAns = camera.WorldToViewportPoint(answerRb.transform.position);

        // Check if the object is off-screen
        if (viewportPositionAns.x < 0)
        {
            // Debug.Log("Out of range Answer");
            answerOutOfBounds = true;
        }

        Vector3 viewportPositionWrongAns = camera.WorldToViewportPoint(wrongAnswerRb.transform.position);
        // Check if the object is off-screen
        if (viewportPositionWrongAns.x < 0)
        {
            // Debug.Log("Out of range WrongAnswer");
            wrongAnswerOutOfBounds = true;
        }

        // If both answers are out of bounds, start a new question and reset their positions
        if (answerOutOfBounds == true && wrongAnswerOutOfBounds == true)
        {
            ResetPositions();
        }
    }

    public void ResetPositions()
    {
        int rand = Random.Range(1, 3);
        if (rand == 1)
        {
            answerRb.transform.position = firstStartPostion;
            wrongAnswerRb.transform.position = secondStartPostion;
        }
        else if (rand == 2)
        {
            answerRb.transform.position = secondStartPostion;
            wrongAnswerRb.transform.position = firstStartPostion;
        }

        answerOutOfBounds = false;
        wrongAnswerOutOfBounds = false;

        textChangeObject.IncrementQuestion();

        answerHit = false;
    }

    public void RandomizeStartPositions()
    {
        int rand = Random.Range(1, 3);
        if (rand == 1)
        {
            answerRb.transform.position = firstStartPostion;
            wrongAnswerRb.transform.position = secondStartPostion;
        }
        else if (rand == 2)
        {
            answerRb.transform.position = secondStartPostion;
            wrongAnswerRb.transform.position = firstStartPostion;
        }

        answerOutOfBounds = false;
        wrongAnswerOutOfBounds = false;

        answerHit = false;
    }
}
