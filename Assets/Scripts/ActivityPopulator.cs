using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;

public class ActivityPopulator : MonoBehaviour
{
    // Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    [Header("Prefabs")]
    public GameObject activityPrefab;

    [Header("Scrollers")]
    public GameObject followedActivities;
    public GameObject discoverStories;
    public GameObject discoverGames;

    List<Story> stories;
    List<Minigame> minigames;

    void Start()
    {
        StartCoroutine(FetchStoriesCoroutine(() =>
        {
            // Load stories into Followed Activities
            LoadStoriesOntoParent(followedActivities);

            // Load stories into Discover Stories
            LoadStoriesOntoParent(discoverStories);
        }));

        StartCoroutine(FetchMinigamesCoroutine(() =>
        {
            // Load minigames into Followed Activities
            LoadMinigamesOntoParent(followedActivities);

            // Load minigames into Discover Games
            LoadMinigamesOntoParent(discoverGames);
        }));
    }

    IEnumerator FetchStoriesCoroutine(Action callback)
    {
        stories = new List<Story>();

        Task task = DatabaseHandler.FetchStoriesAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        stories = DatabaseHandler.GetFetchedStories();

        //Debug.Log($"Successfully loaded {stories.Count} Stories");

        callback.Invoke();
    }

    IEnumerator FetchMinigamesCoroutine(Action callback)
    {
        minigames = new List<Minigame>();

        Task task = DatabaseHandler.FetchMinigamesAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        minigames = DatabaseHandler.GetFetchedMinigames();

        //Debug.Log($"Successfully loaded {minigames.Count} Minigames");

        callback.Invoke();
    }

    private void LoadStoriesOntoParent(GameObject parent)
    {
        foreach (Story story in stories)
        {
            GameObject activityClone = Instantiate(activityPrefab);
            GameObject title = activityClone.transform.Find("Title").gameObject;
            TMP_Text titleText = title.GetComponent<TMP_Text>();
            titleText.text = story.GetName();

            Texture2D texture = Resources.Load<Texture2D>($"{story.GetCodeName()}_Art");
            if (texture != null)
            {
                Image image = activityClone.transform.Find("Image").GetComponent<Image>();
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                image.sprite = sprite;
                image.color = Color.white;
            }

            activityClone.transform.SetParent(parent.transform, false);

            Button button = activityClone.GetComponent<Button>();
            button.onClick.AddListener(story.OnClick);
        }
    }

    private void LoadMinigamesOntoParent(GameObject parent)
    {
        foreach (Minigame minigame in minigames)
        {
            GameObject activityClone = Instantiate(activityPrefab);
            GameObject title = activityClone.transform.Find("Title").gameObject;
            TMP_Text titleText = title.GetComponent<TMP_Text>();
            titleText.text = minigame.GetName();

            Texture2D texture = Resources.Load<Texture2D>($"{minigame.GetCodeName()}_Art");
            if (texture != null)
            {
                Image image = activityClone.transform.Find("Image").GetComponent<Image>();
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                image.sprite = sprite;
                image.color = Color.white;
            }
            
            activityClone.transform.SetParent(parent.transform, false);

            Button button = activityClone.GetComponent<Button>();
            button.onClick.AddListener(minigame.OnClick);
        }
    }
}
