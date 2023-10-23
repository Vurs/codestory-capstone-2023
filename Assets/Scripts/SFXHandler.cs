using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXHandler : MonoBehaviour
{
    public AudioSource success;
    public static bool canPlay;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("SoundEffectsEnabled"))
        {
            canPlay = PlayerPrefs.GetInt("SoundEffectsEnabled") == 0 ? false : true;
        } else
        {
            canPlay = true;
        }
    }

    public static void PlaySound(AudioSource source)
    {
        if (canPlay == true)
        {
            source.Play();
        }
    }
}
