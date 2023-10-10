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
    string[] questions = { "What do you end a line of code with?", "Use to print to a terminal?", "Which of the following is a double?", "Which of the following is an int?", "Which is the assignment operator?", "Used to declare text variables?", "Used to declare constant variables?", "Used to store a single character", "Default value of an uninitialized integer variable?", "How do you write a single-line comment?", "Used to declare an array of integers?", "Used to read user input from the console?", "Which of these methods returns a value?", "How do you use an external library in your Java program?" };
    string[] answers = { ";", "System.out.println();", "3.5", "404", "=", "String", "final", "char", "0", "//", "int[]", "Scanner(System.in)", "public String method()", "import" };
    string[] fakeOuts = { ".", "print();", "1", "12.5", "!=", "char[]", "const", "string", "-1", "--", "List<int>", "Console.ReadLine()", "public void method()", "using" };
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
        Dictionary<int, string[]> answersDict = new Dictionary<int, string[]>() {
            { 0, new string[]{ ";" } },
            { 1, new string[]{ "System.out.println();" } },
            { 2, new string[]{ System.Math.Round(Random.Range(10f, 100f) + 0.5f, 1).ToString("0.0") } },
            { 3, new string[]{ Random.Range(1, 120).ToString() } },
            { 4, new string[]{ "=" } },
            { 5, new string[]{ "String" } },
            { 6, new string[]{ "final" } },
            { 7, new string[]{ "char" } },
            { 8, new string[]{ "0" } },
            { 9, new string[]{ "//" } },
            { 10, new string[]{ "int[]" } },
            { 11, new string[]{ "Scanner(System.in)" } },
            { 12, new string[]{ "public String method()", "private int func()", "long compute()", "protected float add()" } },
            { 13, new string[]{ "import" } },
        };

        Dictionary<int, string[]> fakeOutsDict = new Dictionary<int, string[]>() {
            { 0, new string[]{ ".", "?", "/" } },
            { 1, new string[]{ "print();", "console.log();", "echo" } },
            { 2, new string[]{ Random.Range(1, 120).ToString() } },
            { 3, new string[]{ System.Math.Round(Random.Range(10f, 100f) + 0.5f, 1).ToString("0.0") } },
            { 4, new string[]{ "!=", "==", "~=" } },
            { 5, new string[]{ "char[]", "List<char>", "str" } },
            { 6, new string[]{ "const", "constant", "let" } },
            { 7, new string[]{ "character", "String", "chara" } },
            { 8, new string[]{ "-1", "100", "1" } },
            { 9, new string[]{ "--", "~~", "#" } },
            { 10, new string[]{ "List<int>", "int{}", "int<>" } },
            { 11, new string[]{ "Console.ReadLine()", "System.ReadLine()", "input()" } },
            { 12, new string[]{ "public void method()", "static void main()", "protected void func()" } },
            { 13, new string[]{ "using", "#include", "require" } },
        };

        int rand = Random.Range(0, questions.Length);

        //Setting textmesh to the first question in the array
        questionText.text = questions[rand];
        rightAnsText.text = answersDict[rand][Random.Range(0, answersDict[rand].Length)];
        wrongAnsText.text = fakeOutsDict[rand][Random.Range(0, fakeOutsDict[rand].Length)];

        answerPositionHandler.RandomizeStartPositions();
    }

    public void IncrementQuestion()
    {
        ////This is pretty much saying if we are at the second last index run this scope.
        //if (questionIndex == questions.Length - 1)
        //{
        //    //Access the last question in the array "End"
        //    Debug.Log("Done");
        //    circleJumper.enabled = false;
        //    endActivityHandler.EndActivity(endActivityHandler.gameObject, RunnerCollision.score / 10, CodeRunnerGameHandler.elapsedTime);
        //}

        ////Cool boi stuff happens here to change the question when we're within range
        //else if (questionIndex < questions.Length + 1)
        //{
        //    questionIndex++;
        //    questionText.text = questions[questionIndex];
        //    rightAnsText.text = answers[questionIndex];
        //    wrongAnsText.text = fakeOuts[questionIndex];
        //}

        Dictionary<int, string[]> answersDict = new Dictionary<int, string[]>() {
            { 0, new string[]{ ";" } },
            { 1, new string[]{ "System.out.println();" } },
            { 2, new string[]{ System.Math.Round(Random.Range(10f, 100f) + 0.5f, 1).ToString("0.0") } },
            { 3, new string[]{ Random.Range(1, 120).ToString() } },
            { 4, new string[]{ "=" } },
            { 5, new string[]{ "String" } },
            { 6, new string[]{ "final" } },
            { 7, new string[]{ "char" } },
            { 8, new string[]{ "0" } },
            { 9, new string[]{ "//" } },
            { 10, new string[]{ "int[]" } },
            { 11, new string[]{ "Scanner(System.in)" } },
            { 12, new string[]{ "public String method()", "private int func()", "long compute()", "protected float add()" } },
            { 13, new string[]{ "import" } },
        };

        Dictionary<int, string[]> fakeOutsDict = new Dictionary<int, string[]>() {
            { 0, new string[]{ ".", "?", "/" } },
            { 1, new string[]{ "print();", "console.log();", "echo" } },
            { 2, new string[]{ Random.Range(1, 120).ToString() } },
            { 3, new string[]{ System.Math.Round(Random.Range(10f, 100f) + 0.5f, 1).ToString("0.0") } },
            { 4, new string[]{ "!=", "==", "~=" } },
            { 5, new string[]{ "char[]", "List<char>", "str" } },
            { 6, new string[]{ "const", "constant", "let" } },
            { 7, new string[]{ "character", "String", "chara" } },
            { 8, new string[]{ "-1", "100", "1" } },
            { 9, new string[]{ "--", "~~", "#" } },
            { 10, new string[]{ "List<int>", "int{}", "int<>" } },
            { 11, new string[]{ "Console.ReadLine()", "System.ReadLine()", "input()" } },
            { 12, new string[]{ "public void method()", "static void main()", "protected void func()" } },
            { 13, new string[]{ "using", "#include", "require" } },
        };

        int rand = Random.Range(0, questions.Length);

        //Setting textmesh to the first question in the array
        questionText.text = questions[rand];
        rightAnsText.text = answersDict[rand][Random.Range(0, answersDict[rand].Length)];
        wrongAnsText.text = fakeOutsDict[rand][Random.Range(0, fakeOutsDict[rand].Length)];
    }
}
