using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : Activity
{
    public Story(string name, string description)
    {
        this.name = name;
        this.description = description;
    }

    public override void OnClick()
    {
        Debug.Log($"Clicked '{name}' StoryPane!");
    }
}
