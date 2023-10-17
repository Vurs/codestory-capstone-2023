using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetScroll : MonoBehaviour
{
    public ScrollRect[] scrollRects;

    // Start is called before the first frame update
    void Start()
    {
        foreach(ScrollRect rect in scrollRects) {
            rect.verticalNormalizedPosition = 1.0f;
        }
    }
}
