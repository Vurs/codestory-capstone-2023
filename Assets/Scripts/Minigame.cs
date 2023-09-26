using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : Activity
{
    public Minigame(string name, string description, ActivityType activityType)
    {
        this.name = name;
        this.description = description;
        this.activityType = activityType;
    }
}
