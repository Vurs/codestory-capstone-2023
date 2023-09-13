using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activity
{
    public string name;
    public string description;

    public abstract void OnClick();

    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        return description;
    }
}
