using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RunnerCollision : MonoBehaviour
{
    public textChange textChangeObject;
    public AnswerPositionHandler answerPositionHandler;
    public static int score = 0;
    private int lives = 3; 
    public TextMeshProUGUI scoreValue;

    //TODO: Change with hearts if get the prefab hearts 
    public TextMeshProUGUI livesValue;

    bool debounce = false;

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

        if (col.gameObject.tag == "Answer")
        {
            score++;
            Debug.Log("You got it! score: " +score);
            scoreValue.text = score.ToString();
        }
        else
        {
            Debug.Log("Oops, wrong answer :(");
            lives--;
            livesValue.text = lives.ToString(); 
        }

        if (lives == 0) {
            Debug.Log("Game over");
        }
    }

    IEnumerator ToggleDebounce()
    {
        debounce = true;
        yield return new WaitForSeconds(1f);
        debounce = false;
    }
}
