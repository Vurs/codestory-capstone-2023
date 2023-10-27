using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class Activity
{
    public enum ActivityType
    {
        Minigame,
        Story
    }

    protected string name;
    protected string description;
    protected string codeName;
    protected ActivityType activityType;

    public void OnClick()
    {
        // Get references
        ReferenceHandler refHandler = GameObject.Find("ReferenceHandler").GetComponent<ReferenceHandler>();
        List<GameObject> pagesList = refHandler.GetPagesList();
        Button playButton = refHandler.GetPlayButton();

        GameObject gameStoryPage = pagesList.Find(x => x.name == "GameStoryPage");
        GameObject homePage = pagesList.Find(x => x.name == "HomePage");

        homePage.SetActive(false);
        gameStoryPage.SetActive(true);

        // Set the Name text and Description text accordingly
        TMP_Text nameText = gameStoryPage.transform.Find("Name").GetComponent<TMP_Text>();
        TMP_Text descriptionText = gameStoryPage.transform.Find("Description").GetComponent<TMP_Text>();

        nameText.text = name;
        descriptionText.text = description;

        Texture2D texture = Resources.Load<Texture2D>($"{codeName}_Art");
        if (texture != null)
        {
            Image bannerImage = gameStoryPage.transform.Find("BannerImage").GetComponent<Image>();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            bannerImage.sprite = sprite;
            bannerImage.color = Color.white;
        }

        playButton.onClick.AddListener(OnPlayClick);
    }

    void OnPlayClick()
    {
        SceneManager.LoadSceneAsync(codeName);

        //if (activityType  == ActivityType.Minigame)
        //{
        //    SceneManager.LoadSceneAsync(name);
        //} else
        //{
        //    // In the future, make a separate Scene for every story and call them by their codenames
        //    SceneManager.LoadSceneAsync("StoryMapScene");
        //    // When we add more stories we can do SceneManager.LoadSceneAsync(codeName);
        //}
    }

    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        return description;
    }

    public string GetCodeName()
    {
        return codeName;
    }
}
