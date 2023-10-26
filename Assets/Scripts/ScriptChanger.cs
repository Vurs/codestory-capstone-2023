using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScriptChanger : MonoBehaviour
{
    public Button nextButton;
    public Button backButton;
    public TextMeshProUGUI dialogBox;
    public Image npcImage;

    public Animator animator;

    private int textPosition = 0;

    string[] script = { "Good morning agent, at precisely o hundred hours today, the US president was kidnapped by traitor agents.", "We received intel that Doctor Exception is the mastermind behind the recent kidnapping of the president", "and has left a series of cryptic coding questions behind.", "We believe once these questions are solved, it will lead us to the location of the president.", "As our best agent, we need you to rescue the president and bring Doctor Exception and his agents to justice.", "Good luck agent over and out.", "Greetings Agent, my name is Doctor Method and I am the lead scientist of the OOP agency", "I've been tasked with helping you through these puzzles to save the president from that vile Doctor Exception. Lets look at the first one now", "Ahh that tricky Doctor Exception, this line of code is creating what's known as a variable", "Variables allow us to assign numeric or String values into a placeholder using the assignment (=) operator", "This placeholder is known as a variable and can be whatever name you want it to be", "We can then use that variable throughout the program. But a very important part is missing. Primitive Data Types.", "Primitive Data Types allow us to specify what data type a variable will hold in Java and go before the variable name", "The major data types are int which hold whole numbers, double for decimal numbers, and String for strings or words/sentences", @"Note String needs to be written with a capital s and need to have double quotes around the word eg: String word = ""hello"";", "An example of creating an int variable will be as follows: int num = 10;", "Lastly, an example of creating a double variable will be: double doubleNum = 5.4;", "With all this being said, solve the coding question below. Good luck agent." };

    // Start is called before the first frame update
    void Start()
    {
       
        nextButton.onClick.AddListener(ClickedNextButton);
        backButton.onClick.AddListener(ClickedBackButton);
        backButton.interactable = false;

        dialogBox.text = script[textPosition];
        Debug.Log(textPosition);
    }

    void ClickedBackButton()
    {
        if (textPosition > 0)
        {
            textPosition--;
            dialogBox.text = script[textPosition];
            nextButton.interactable = true;
        }

        if (textPosition == 0) {
            backButton.interactable = false;
        }

        if (textPosition == 5) {
            npcImage.color = Color.white;
        }

        if (textPosition == 7) {
            animator.SetBool("ShowAnswer", false);
        }
    }

    void ClickedNextButton() {

        if (textPosition == 0) {
            backButton.interactable = true;
        }

        if (textPosition == 7) {
            Debug.Log("Here");
            animator.SetBool("ShowAnswer", true);
        }

        if (textPosition == 5) {
            npcImage.color = Color.red;
        }

        if (textPosition == script.Length - 1)
        {
            //Debug.Log("End");
            //nextButton.interactable = false;
            //return;
        }
        else
        {
            //Debug.Log("Button Clicked");
            Debug.Log("Else TextPosition 1: " + textPosition);
            textPosition++;
            dialogBox.text = script[textPosition];
            Debug.Log(" ElseTextPosition 2: " + textPosition);

            if (textPosition == script.Length - 1)
            {
                Debug.Log("End");
                nextButton.interactable = false;
                return;
            }
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}