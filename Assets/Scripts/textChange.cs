using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

//TODO: somehow centre the text questions in the actual unitiy game

public class textChange : MonoBehaviour
{
    //Questions that we will ask the user
    string[] questions = { "What do you end a line of code with?", "Use to print to a terminal?", "Which of the following is a double?", "Which of the following is an int?", "Which is the assignment operator?", "Used to declare text variables?", "Used to declare constant variables?", "Used to store a single character", "Default value of an uninitialized integer variable?", "How do you write a single-line comment?", "Used to declare an array of integers?", "Used to read user input from the console?", "Which of these methods returns a value?", "How do you use an external library in your Java program?", "End" };
    string[] answers = { ";", "System.out.println();", "3.5", "404", "=", "String", "final", "char", "0", "//", "int[]", "Scanner(System.in)", "public String method()", "import", "End" };
    string[] fakeOuts = { ".", "print();", "1", "12.5", "!=", "char[]", "const", "string", "-1", "--", "List<int>", "Console.ReadLine()", "public void method()", "using", "End" };
    int questionIndex = 0;

    //Accessing the TextMesh component in Unity
    public TextMeshProUGUI questionText;
    public TMP_Text rightAnsText;
    public TMP_Text wrongAnsText;

    public movement circleJumper;

    //Timer in seconds - users will get 5 seconds to answer each question
    public float timer = 5f;

    public AnswerPositionHandler answerPositionHandler;

    public EndActivityHandler endActivityHandler;

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
            endActivityHandler.EndActivity(endActivityHandler.gameObject, RunnerCollision.score * 10, CodeRunnerGameHandler.elapsedTime);
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
