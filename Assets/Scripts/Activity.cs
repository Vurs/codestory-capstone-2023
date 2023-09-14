using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Activity
{
    protected string name;
    protected string description;

    public void OnClick()
    {
        ReferenceHandler refHandler = GameObject.Find("ReferenceHandler").GetComponent<ReferenceHandler>();
        List<GameObject> pagesList = refHandler.GetPagesList();

        GameObject gameStoryPage = pagesList.Find(x => x.name == "GameStoryPage");
        GameObject homePage = pagesList.Find(x => x.name == "HomePage");

        homePage.SetActive(false);
        gameStoryPage.SetActive(true);

        // Set the Name text and Description text accordingly
        TMP_Text nameText = gameStoryPage.transform.Find("Name").GetComponent<TMP_Text>();
        TMP_Text descriptionText = gameStoryPage.transform.Find("Description").GetComponent<TMP_Text>();

        nameText.text = name;
        descriptionText.text = description;

        // In the future when graphics are made, set the BannerImage as well
    }

    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        return description;
    }
}
