using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;

public class DatabaseHandler : MonoBehaviour
{
    private static List<Story> stories;
    private static List<Minigame> minigames;

    // DatabaseSingleton dbSingleton;

    // void Start()
    // {
    //     dbSingleton = DatabaseSingleton.Instance;

    //     // Initialize Firebase
    //     if (dbSingleton.IsFirebaseInitialized() == false)
    //     {
    //         dbSingleton.InitializeFirebase();
    //     }
    // }

    public static async Task<List<Minigame>> FetchMinigamesAsync()
    {
        minigames = new List<Minigame>();

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        DatabaseReference minigamesReference = reference.Child("minigames");

        TaskCompletionSource<List<Minigame>> tcs = new TaskCompletionSource<List<Minigame>>();

        await minigamesReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot != null && snapshot.Exists)
                {
                    // Parse and process the data here
                    foreach (DataSnapshot minigameSnapshot in snapshot.Children)
                    {
                        Debug.LogWarning("Found minigame");
                        // Access individual minigame data using minigameSnapshot
                        string minigameId = minigameSnapshot.Key;

                        DataSnapshot nameSnapshot = minigameSnapshot.Child("Name");
                        DataSnapshot descriptionSnapshot = minigameSnapshot.Child("Description");

                        string nameValue = nameSnapshot?.Value?.ToString() ?? "undefined";
                        string descriptionValue = descriptionSnapshot?.Value?.ToString() ?? "undefined";

                        // Handle the minigame data as needed
                        Debug.Log($"Added minigame {nameValue}");
                        Minigame minigame = new Minigame(nameValue, descriptionValue);
                        minigames.Add(minigame);
                    }
                }
                else
                {
                    Debug.LogWarning("No minigames found.");
                }
            }
            else
            {
                Debug.LogError("Error fetching minigames: " + task.Exception);
            }

            // Set the result for the TaskCompletionSource
            tcs.SetResult(minigames);
        });

        // Wait for the asynchronous operation to complete and return the result
        return await tcs.Task;
    }

    public static List<Minigame> GetFetchedMinigames()
    {
        return minigames;
    }

    public static async Task<List<Story>> FetchStoriesAsync()
    {
        stories = new List<Story>();

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        DatabaseReference storiesReference = reference.Child("stories");

        TaskCompletionSource<List<Story>> tcs = new TaskCompletionSource<List<Story>>();

        await storiesReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot != null && snapshot.Exists)
                {
                    // Parse and process the data here
                    foreach (DataSnapshot storySnapshot in snapshot.Children)
                    {
                        // Access individual story data using storySnapshot
                        string storyId = storySnapshot.Key;

                        DataSnapshot nameSnapshot = storySnapshot.Child("Name");
                        DataSnapshot descriptionSnapshot = storySnapshot.Child("Description");

                        string nameValue = nameSnapshot?.Value?.ToString() ?? "undefined";
                        string descriptionValue = descriptionSnapshot?.Value?.ToString() ?? "undefined";

                        // Handle the story data as needed
                        Debug.Log($"Added story {nameValue}");
                        Story story = new Story(nameValue, descriptionValue);
                        stories.Add(story);
                    }
                }
                else
                {
                    Debug.LogWarning("No stories found.");
                }
            }
            else
            {
                Debug.LogError("Error fetching stories: " + task.Exception);
            }

            // Set the result for the TaskCompletionSource
            tcs.SetResult(stories);
        });

        // Wait for the asynchronous operation to complete and return the result
        return await tcs.Task;
    }

    public static List<Story> GetFetchedStories()
    {
        return stories;
    }
}
