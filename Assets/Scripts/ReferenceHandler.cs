using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferenceHandler : MonoBehaviour
{
    public List<GameObject> pages;
    public Button playButton;

    public List<GameObject> GetPagesList()
    {
        return pages;
    }

    public Button GetPlayButton()
    {
        return playButton;
    }
}
