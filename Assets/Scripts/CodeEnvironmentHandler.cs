using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeEnvironmentHandler : MonoBehaviour
{
    public static int elapsedTime = 0;
    public static bool gameOver = false;

    private void Start()
    {
        elapsedTime = 0;
        gameOver = false;
        StartCoroutine(StartCounter());
    }

    IEnumerator StartCounter()
    {
        Debug.Log("Called");
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (gameOver == false)
            {
                Debug.Log("Counting up " + elapsedTime);
                elapsedTime++;
            }
        }
    }
}
