using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDailyStreak : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DailyStreakHandler.HandleStreakOnLoad();
    }
}
