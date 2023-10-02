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
    private string[] instructions = {"//Create a variable that stores your age", "//Create a variable that stores your first and last name", "//Create a variable that holds the time (use . in place of :)"}; 

    private int exampleIterator = 0;
    private int instructionIterator = 0;

    // Start is called before the first frame update
    void Start()
    {
        exampleComments.text = examples[exampleIterator]; 
        commentInstructions.text = instructions[instructionIterator];

        commit.onClick.AddListener(ClickedCommitButton);
    }

    private void ClickedCommitButton()
    {
        Debug.Log("Commit Button clicked");

        if (inputCode.text == "")
        {
            Debug.Log("Code is empty");
            //TODO: Create pop up box  for this
        }
        else {
            Debug.Log(inputCode.text);
            //TODO: Take the first element of the string to check if the variable and value are the right type 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
