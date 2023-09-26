using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeRunnerGameHandler : MonoBehaviour
{
    public static int elapsedTime = 0;

    private void Start()
    {
        StartCoroutine(StartCounter());
    }

    IEnumerator StartCounter()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            elapsedTime++;
        }
    }
}
