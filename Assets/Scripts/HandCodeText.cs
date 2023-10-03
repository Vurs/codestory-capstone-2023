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

    private string[] examples = { "//int wholeNum = 5;", @"//String words = ""Hello World"";", "//double decimalNum = 3.4;" };
    private string[] instructions = { "//Create an int variable that stores the value of 25", "//Create a String variable that stores Java is fun", "//Create a double variable that holds 3.1415" };

    private int exampleIterator = 0;
    private int instructionIterator = 0;
    private String questionType = "";

    // Start is called before the first frame update
    void Start()
    {
        exampleComments.text = examples[exampleIterator];
        commentInstructions.text = instructions[instructionIterator];

        commit.onClick.AddListener(ClickedCommitButton);
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


        if (inputCode.text == "")
        {
            Debug.Log("Code is empty");
            //TODO: Code Empty Pop up here
        }
        else
        {
            String[] str = inputCode.text.Split();
            String type = str[0];
            char[] strToChar = inputCode.text.ToCharArray();



            Debug.Log(strToChar[strToChar.Length - 1]);

            //For int question

            if (questionType == "int" && type == "int" && strToChar[strToChar.Length - 1] == ';')
            {
                if (inputCode.text.Contains("25"))
                {
                    Debug.Log("Success!");
                    iterateQuestion();
                }
                else
                {
                    Debug.Log("Oh no. Make sure that your variable is set to the right value.");
                }
            }


            //For String question

            else if (questionType == "String" && type == "String" && strToChar[strToChar.Length - 1] == ';')
            {
                inputCode.text = inputCode.text.ToLower();

                if (inputCode.text.Contains(@"""java is fun""")) //Checks if the message is surrounded by double quotes
                {
                    Debug.Log("Success!");
                    iterateQuestion();
                }
                else
                {
                    Debug.Log("Oh no. Make sure that your variable is set to the right value.");
                }
            }


            //For Double question

            else if (questionType == "double" && type == "double" && strToChar[strToChar.Length - 1] == ';')
            {
                inputCode.text = inputCode.text.ToLower();

                if (inputCode.text.Contains("3.1415"))
                {
                    Debug.Log("Success!");
                    iterateQuestion();
                }
                else
                {
                    Debug.Log("Oh no. Make sure that your variable is set to the right value.");
                }
            }

            else
            {
                Debug.Log("Oh no you got an error in your code. Check if you're using the right type, value, and semicolon!");
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

//TODO: Need to figure out a way to only accept ints or doubles without double quotaition marks -> Right now it accepts both -> Maybe parse it? 
//TODO: Pop up windows for if the input box is blank, error messages, and success messages 
//TODO: Add more questions 
//TODO: Figure out a way to check if the variable has no spaces -> example: wholeNum instead of whole num