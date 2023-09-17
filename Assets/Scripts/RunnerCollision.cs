using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RunnerCollision : MonoBehaviour
{
    public textChange textChangeObject;
    public AnswerPositionHandler answerPositionHandler;
    private int score = 0;
    public TextMeshProUGUI scoreValue; 

    bool debounce = false;

    private void Start()
    {
        score = 0;
        scoreValue.text = score.ToString();
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
        }
    }

    IEnumerator ToggleDebounce()
    {
        debounce = true;
        yield return new WaitForSeconds(1f);
        debounce = false;
    }
}
