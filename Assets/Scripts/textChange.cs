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
    string[] questions = {"What do you end a line of code with?", "What do we use to print to a terminal in Java?", "End"};
    string[] answers = {";", "System.out.println();" };
    string[] fakeOuts = {".", "print();"}; 
    int questionIndex = 0;

    //Accessing the TextMesh component in Unity
    public TextMeshProUGUI questionText;

    //Timer in seconds - users will get 5 seconds to answer each question
    public float timer = 5f; 

    private void Start()
    {
        //Setting textmesh to the first question in the array
        questionText.text = questions[questionIndex]; 
    }

    void Update()
    {
        if (timer > 0)
        {
            //Decrement time by 1
            timer -= Time.deltaTime;
        }
        //This is pretty much saying if we are at the second last index run this scope.
        //TODO: Might have to change it to questionIndex-2 or something like that once theres more questions in the array
        else if (timer < 0 && questionIndex == 2)
        {
            timer = 0;
            //Access the last question in the array "End"
            questionText.text = questions[questionIndex++];
            Debug.Log("Done");
        }

        //Cool boi stuff happens here to change the question every 5 seconds and we're within range
        else if (timer < 0 && questionIndex  <= 2)
        {
            questionIndex++;
            timer = 5f;
            questionText.text = questions[questionIndex];
        }
        
    }
}
