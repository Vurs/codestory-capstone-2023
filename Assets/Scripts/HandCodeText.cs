using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandCodeText : MonoBehaviour
{
    public TextMeshProUGUI exampleComments;
    public TextMeshProUGUI commentInstructions;
    public TMP_InputField inputCode;
    public Button commit;

    public GameObject popUpCanvas;
    public TMP_Text correctIncorrect;
    public TMP_Text blurb;
    public Button okButton;

    private string[] examples = { "//int wholeNum = 5;", @"//String words = ""Hello World"";", "//double decimalNum = 3.4;", "//int wholeNum = 5;", @"//String words = ""Hello World"";", "//double decimalNum = 3.4;", "//int wholeNum = 5;", @"//String words = ""Hello World"";", "//double decimalNum = 3.4;" };
    private string[] instructions = { "//Create an int variable called wholeNum that stores the value of 25", "//Create a String variable called stringVar that stores Java is fun", "//Create a double variable called doubleNum that holds 3.1415", "//Create an int variable called wholeNum that stores 300", "//Create a String variable called stringVar that stores Hello Java!", "//Create a double variaible called doubleNum that stores 7.543215", "//Create an int called wholeNum that store 404", "//Create a String variable called stringVar that stores I'm learning Java","//Create a double variaible called doubleNum that stores 154.25410"};

    private String[] answers = {"25", "Java is fun", "3.1415", "300", "Hello Java!", "7.543215", "404", "I'm learning Java", "154.25410"};

    private int exampleIterator = 0;
    private int instructionIterator = 0;
    private String questionType = "";
    private bool shouldIterate = false;

    // Start is called before the first frame update
    void Start()
    {
        exampleComments.text = examples[exampleIterator];
        commentInstructions.text = instructions[instructionIterator];

        commit.onClick.AddListener(ClickedCommitButton);

        Debug.Log("Examples length: " +examples.Length);
        Debug.Log("instructions length: " +instructions.Length);
        Debug.Log("answers length: " +answers.Length);
    }

    void iterateQuestion()
    {

        if (instructionIterator == instructions.Length - 1 && exampleIterator == examples.Length - 1) {
            commit.interactable = false;
            return;
        }

        //Success pop up here

        exampleIterator++;
        instructionIterator++;

        exampleComments.text = examples[exampleIterator];
        commentInstructions.text = instructions[instructionIterator];

        inputCode.text = "";

    }

    void PresentPopUp(string titleText, string blurbText, bool setShouldIterate)
    {
        shouldIterate = setShouldIterate;
        correctIncorrect.text = titleText;
        blurb.text = blurbText;
        popUpCanvas.SetActive(true);
    }

    public void OnButtonPress()
    {
        popUpCanvas.SetActive(false);
        if (shouldIterate == true)
        {
            iterateQuestion();
        }
    }

    private void ClickedCommitButton()
    {
        if (examples[exampleIterator].Contains("int"))
        {
            questionType = "int";
        }
        else if (examples[exampleIterator].Contains("String"))
        {
            questionType = "String";
        }
        else {
            questionType = "double";
        }

        Debug.Log(questionType);

        inputCode.text = inputCode.text.Trim();


        if (inputCode.text == "")
        {
            Debug.Log("Code is empty");
            PresentPopUp("Code Empty", "Write some code and try submitting again!", false);
            //TODO: Code Empty Pop up here
        }
        else
        {
            String[] str = inputCode.text.Split();
            String type = str[0];
            char[] strToChar = inputCode.text.ToCharArray();

            //For int question

            if (questionType == "int" && type == "int" && strToChar[strToChar.Length - 1] == ';' && inputCode.text.Contains("wholeNum"))
            {
                if (inputCode.text.Contains(@"""" + answers[instructionIterator] + @"""") || inputCode.text.Contains(@"""" + answers[instructionIterator]) || inputCode.text.Contains(answers[instructionIterator] + @""""))
                {
                    Debug.Log("Oh no. Make sure that your variable is set to the right value.");
                    PresentPopUp("Whoops!", "Make sure your variable is set to the right value!", false);
                }
                else if (inputCode.text.Contains(answers[instructionIterator]))
                {
                    Debug.Log("Success!");
                    PresentPopUp("Success!", "Great work!", true);
                }
                else {
                    Debug.Log("Oh no. Make sure that your variable is set to the right value.");
                    PresentPopUp("Whoops!", "Make sure your variable is set to the right value!", false);
                }
            }


            //For String question

            else if (questionType == "String" && type == "String" && strToChar[strToChar.Length - 1] == ';' && inputCode.text.Contains("stringVar"))
            {
                //inputCode.text = inputCode.text.ToLower();

                if (inputCode.text.Contains(@"""" +answers[instructionIterator]+@"""")) //Checks if the message is surrounded by double quotes
                {
                    Debug.Log("Success!");
                    PresentPopUp("Success!", "Great work!", true);
                }
                else
                {
                    Debug.Log("Oh no. Make sure that your variable is set to the right value.");
                    PresentPopUp("Whoops!", "Make sure your variable is set to the right value!", false);
                }
            }


            //For Double question

            else if (questionType == "double" && type == "double" && strToChar[strToChar.Length - 1] == ';' && inputCode.text.Contains("doubleNum"))
            {
                if (inputCode.text.Contains(@"""" + answers[instructionIterator] + @"""") || inputCode.text.Contains(@"""" + answers[instructionIterator]) || inputCode.text.Contains(answers[instructionIterator] + @""""))
                {
                    Debug.Log("Oh no. Make sure that your variable is set to the right value.");
                    PresentPopUp("Whoops!", "Make sure your variable is set to the right value!", false);
                }
                else if (inputCode.text.Contains(answers[instructionIterator]))
                {
                    Debug.Log("Success!");
                    PresentPopUp("Success!", "Great work!", true);
                }
                else
                {
                    Debug.Log("Oh no. Make sure that your variable is set to the right value.");
                    PresentPopUp("Whoops!", "Make sure your variable is set to the right value!", false);
                }
            }

            else
            {
                Debug.Log("Oh no you got an error in your code. Check if you're using the right type, value, and semicolon!");
                PresentPopUp("Whoops!", "Looks like there's an error. Check if you're using the right type, variable name, value, and semicolon!", false);
            }

        }

        if (instructionIterator == instructions.Length - 1) {
            Debug.Log("Here");
            PresentPopUp("Congratulations!", "You finished the level!", false);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
