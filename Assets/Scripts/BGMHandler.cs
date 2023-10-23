using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMHandler : MonoBehaviour
{
    public AudioSource source;
    public float defaultVolume = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MusicEnabled"))
        {
            source.volume = PlayerPrefs.GetInt("MusicEnabled") == 0 ? 0 : defaultVolume;
        } else
        {
            source.volume = defaultVolume;
        }
    }
}
