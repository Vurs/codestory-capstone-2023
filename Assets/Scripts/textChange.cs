using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//TODO: somehow centre the text questions in the actual unitiy game

public class textChange : MonoBehaviour
{
    //Questions that we will ask the user
    string[] questions = { "What do you end a line of code with?", "What do we use to print to a terminal in Java?", "Which of the following is a float?", "Which of the following is an int?", "Which of the following is the assignment operator","End" };
    string[] answers = { ";", "System.out.println();", "3.5", "404","=","End" };
    string[] fakeOuts = { ".", "print();", "1", "12.5" , "!=", "End" };
    int questionIndex = 0;

    //Accessing the TextMesh component in Unity
    public TextMeshProUGUI questionText;
    public TMP_Text rightAnsText;
    public TMP_Text wrongAnsText;

    public movement circleJumper;

    //Timer in seconds - users will get 5 seconds to answer each question
    public float timer = 5f;

    public AnswerPositionHandler answerPositionHandler;

    private void Start()
    {
        //Setting textmesh to the first question in the array
        questionText.text = questions[questionIndex];
        rightAnsText.text = answers[questionIndex];
        wrongAnsText.text = fakeOuts[questionIndex];

        answerPositionHandler.RandomizeStartPositions();
    }

    public void IncrementQuestion()
    {
        //This is pretty much saying if we are at the second last index run this scope.
        if (questionIndex == questions.Length - 1)
        {
            //Access the last question in the array "End"
            Debug.Log("Done");
            circleJumper.enabled = false;
        }

        //Cool boi stuff happens here to change the question when we're within range
        else if (questionIndex < questions.Length + 1)
        {
            questionIndex++;
            questionText.text = questions[questionIndex];
            rightAnsText.text = answers[questionIndex];
            wrongAnsText.text = fakeOuts[questionIndex];
        }

    }
}
