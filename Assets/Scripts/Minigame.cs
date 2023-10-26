using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : Activity
{
    public Minigame(string name, string description, string codeName, ActivityType activityType)
    {
        this.name = name;
        this.description = description;
        this.codeName = codeName;
        this.activityType = activityType;
    }
}
