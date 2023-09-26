using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndActivityHandler : MonoBehaviour
{
    public int activityXp;
    public int activityElapsedTime;

    public void EndActivity(GameObject obj, int xp, int elapsedTime)
    {
        activityXp = xp;
        activityElapsedTime = elapsedTime;

        DontDestroyOnLoad(obj);

        SceneManager.LoadSceneAsync("HomeScene");
    }
}
