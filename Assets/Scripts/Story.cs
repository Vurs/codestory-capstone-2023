using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : Activity
{
    public Story(string name, string description, ActivityType activityType)
    {
        this.name = name;
        this.description = description;
        this.activityType = activityType;
    }
}
