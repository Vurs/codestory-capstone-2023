using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RunnerCollision : MonoBehaviour
{
    public textChange textChangeObject;
    public AnswerPositionHandler answerPositionHandler;
    private int score = 0;
    private int lives = 3; 
    public TextMeshProUGUI scoreValue;

    //TODO: Change with hearts if get the prefab hearts 
    public TextMeshProUGUI livesValue;

    bool debounce = false;

    public TMP_Text correctIncorrect;
    public Animator correctIncorrectAnimator;

    private Dictionary<string, Color> textColors = new Dictionary<string, Color>()
    {
        { "Correct", new Color(0, 203, 255)},
        { "Incorrect", new Color(255, 0, 12) }
    };
    

    private void Start()
    {
        score = 0;
        scoreValue.text = score.ToString();
        livesValue.text = lives.ToString();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (debounce == true) return;

        StartCoroutine(ToggleDebounce());

        answerPositionHandler.ResetPositions();

        if (lives == 0)
        {
            Debug.Log("Game over");
            Time.timeScale = 0; //Remember to re init this to 1 to make stuff moving again 

            //Game Over screen here.
        }

        if (col.gameObject.tag == "Answer")
        {
            score++;
            correctIncorrect.color = textColors["Correct"];
            FlashText(correctIncorrect, "CORRECT!");
            Debug.Log("You got it! score: " +score);
            scoreValue.text = score.ToString();
        }
        else
        {
            correctIncorrect.color = textColors["Incorrect"];
            FlashText(correctIncorrect, "INCORRECT!");
            Debug.Log("Oops, wrong answer :(");
            lives--;
            livesValue.text = lives.ToString(); 
        }

    }

    void FlashText(TMP_Text textLabel, string textToDisplay)
    {
        textLabel.text = textToDisplay;
        correctIncorrectAnimator.SetTrigger("Flash");
    }

    IEnumerator ToggleDebounce()
    {
        debounce = true;
        yield return new WaitForSeconds(1f);
        debounce = false;
    }
}
