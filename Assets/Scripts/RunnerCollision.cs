using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerCollision : MonoBehaviour
{
    public textChange textChangeObject;
    public AnswerPositionHandler answerPositionHandler;

    bool debounce = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (debounce == true) return;

        StartCoroutine(ToggleDebounce());

        answerPositionHandler.ResetPositions();

        if (col.gameObject.tag == "Answer")
        {
            Debug.Log("You got it!");
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
