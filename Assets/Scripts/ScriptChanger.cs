using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScriptChanger : MonoBehaviour
{
    public Button nextButton;
    public TextMeshProUGUI dialogBox;
    public Image npcImage;


    private int textPosition = 1;

    string[] script = { "Good morning agent, at precisely o hundred hours today, the US president was kidnapped by traitor agents.", "We received intel that Doctor Exception is the mastermind behind the recent kidnapping of the president", "and has left a series of cryptic coding questions behind.", "We believe once these questions are solved, it will lead us to the location of the president.", "As our best agent, we need you to rescue the president and bring Doctor Exception and his agents to justice.", "Good luck agent over and out.", "Greetings Agent, my name is Doctor Method and I am the lead scientist of the OOP agency", "I�ve been tasked with helping you through these puzzles to save the president from that vile Doctor Exception.", "Ahh that tricky Doctor Exception, this line of code is creating what�s known as a variable", "Variables allow us to assign numeric or String values into a placeholder using the assignment (=) operator", "This placeholder is known as a variable and can be whatever name you want it to be", "We can then use that variable throughout the program. But a very important part is missing. Primitive Data Types.", "Primitive Data Types allow us to specify what data type a variable will hold in Java and go before the variable name", "The major data types are int which hold whole numbers, double for decimal numbers, and String for strings or words/sentences", "Note String needs to be written with a capital s.", "An example of creating a variable will be as follows: int num = 10;" };

    // Start is called before the first frame update
    void Start()
    {
       
        nextButton.onClick.AddListener(ClickedButton);
        dialogBox.text = script[0]; 
    }

    void ClickedButton() {

        if (textPosition == 6) {
            npcImage.color = Color.red;
        }

        if (textPosition == script.Length)
        {
            Debug.Log("End");
            nextButton.interactable = false;
            return;
        }
        else
        {
            //Debug.Log("Button Clicked");
            dialogBox.text = script[textPosition];
            textPosition++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}