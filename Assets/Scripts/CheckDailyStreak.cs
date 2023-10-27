using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDailyStreak : MonoBehaviour
{
    public AudioSource success;

    // Start is called before the first frame update
    void Start()
    {
        DailyStreakHandler.HandleStreakOnLoad();
    }

    public void PlaySuccessSound()
    {
        StartCoroutine(PlaySuccessSoundCoroutine());
    }

    IEnumerator PlaySuccessSoundCoroutine()
    {
        yield return new WaitForSeconds(2f);

        bool canPlay = PlayerPrefs.GetInt("SoundEffectsEnabled") == 0 ? false : true;
        if (canPlay)
        {
            success.Play();
        }
    }
}
