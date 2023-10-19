using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using UnityEngine.UIElements;
using System;
using static UnityEngine.Rendering.DebugUI;
using Firebase.Auth;

public class DatabaseHandler : MonoBehaviour
{
    private static List<Story> stories;
    private static List<Minigame> minigames;
    private static List<string> recentUsers;

    private static UserInfo userInfo;

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
                        //Debug.LogWarning("Found minigame");
                        // Access individual minigame data using minigameSnapshot
                        string minigameId = minigameSnapshot.Key;

                        DataSnapshot nameSnapshot = minigameSnapshot.Child("Name");
                        DataSnapshot descriptionSnapshot = minigameSnapshot.Child("Description");

                        string nameValue = nameSnapshot?.Value?.ToString() ?? "undefined";
                        string descriptionValue = descriptionSnapshot?.Value?.ToString() ?? "undefined";

                        // Handle the minigame data as needed
                        //Debug.Log($"Added minigame {nameValue}");
                        Minigame minigame = new Minigame(nameValue, descriptionValue, Activity.ActivityType.Minigame);
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
                        //Debug.Log($"Added story {nameValue}");
                        Story story = new Story(nameValue, descriptionValue, Activity.ActivityType.Story);
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

    //public static async Task<UserInfo> FetchUserInfoByIdAsync(string userId)
    //{
    //    userInfo = new UserInfo();

    //    DatabaseReference rootReference = FirebaseDatabase.DefaultInstance.RootReference;
    //    DatabaseReference userReference = rootReference.Child("users").Child(userId);

    //    TaskCompletionSource<UserInfo> tcs = new TaskCompletionSource<UserInfo>();

    //    await userReference.GetValueAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsCompleted)
    //        {
    //            DataSnapshot snapshot = task.Result;
    //            if (snapshot.Exists)
    //            {
    //                userInfo.DisplayName = snapshot.Child("displayName").Value.ToString();
    //                Debug.Log("1");
    //                userInfo.Handle = snapshot.Child("handle").Value.ToString();
    //                Debug.Log("2");
    //                userInfo.UserId = userId;
    //                Debug.Log("3");
    //                userInfo.ProfilePicture = snapshot.Child("profilePicture").Value.ToString();
    //                Debug.Log("4");
    //                userInfo.Title = snapshot.Child("title").Value.ToString();
    //                Debug.Log("5");
    //                userInfo.LastSignIn = long.Parse(snapshot.Child("lastSignIn").Value.ToString());
    //                Debug.Log("6");
    //                userInfo.StoriesRead = int.Parse(snapshot.Child("storiesRead").Value.ToString());
    //                Debug.Log("7");
    //                userInfo.GamesPlayed = int.Parse(snapshot.Child("gamesPlayed").Value.ToString());
    //                Debug.Log("8");
    //                userInfo.StoryXp = float.Parse(snapshot.Child("storyXp").Value.ToString());
    //                Debug.Log("9");
    //                userInfo.GameXp = float.Parse(snapshot.Child("gameXp").Value.ToString());
    //                Debug.Log("10");
    //                userInfo.StoryTitlesWon = int.Parse(snapshot.Child("storyTitlesWon").Value.ToString());
    //                Debug.Log("11");
    //                userInfo.GameTitlesWon = int.Parse(snapshot.Child("gameTitlesWon").Value.ToString());
    //                Debug.Log("12");
    //                userInfo.CountryOfOrigin = snapshot.Child("countryOfOrigin").Value.ToString();
    //                Debug.Log("13");
    //                userInfo.Followers = new List<string>();
    //                Debug.Log("14");
    //                userInfo.Following = new List<string>();
    //                Debug.Log("15");

    //                DatabaseReference followersReference = userReference.Child("followers");
    //                followersReference.GetValueAsync().ContinueWithOnMainThread(followersTask =>
    //                {
    //                    if (followersTask.IsCompleted)
    //                    {
    //                        DataSnapshot followersSnapshot = followersTask.Result;

    //                        if (followersSnapshot.Exists)
    //                        {
    //                            foreach(var childSnapshot in followersSnapshot.Children)
    //                            {
    //                                Debug.Log(childSnapshot.Key);
    //                                string followerUid = childSnapshot.Value.ToString();
    //                                userInfo.Followers.Add(followerUid);
    //                            }
    //                        } else
    //                        {
    //                            Debug.LogWarning("Followers list is empty.");
    //                        }
    //                    } else
    //                    {
    //                        Debug.LogError("Error fetching followers list: " + followersTask.Exception);
    //                    }
    //                });

    //                DatabaseReference followingReference = userReference.Child("following");
    //                followingReference.GetValueAsync().ContinueWithOnMainThread(followingTask =>
    //                {
    //                    if (followingTask.IsCompleted)
    //                    {
    //                        DataSnapshot followingSnapshot = followingTask.Result;

    //                        if (followingSnapshot.Exists)
    //                        {
    //                            foreach (var childSnapshot in followingSnapshot.Children)
    //                            {
    //                                Debug.Log(childSnapshot.Key);
    //                                string followingUid = childSnapshot.Value.ToString();
    //                                userInfo.Following.Add(followingUid);
    //                            }
    //                        } else
    //                        {
    //                            Debug.LogWarning("Following list is empty.");
    //                        }
    //                    } else
    //                    {
    //                        Debug.LogError("Error fetching following list: " + followingTask.Exception);
    //                    }
    //                });
    //            } else
    //            {
    //                Debug.LogWarning("User could not be found.");
    //            }
    //        }

    //        tcs.SetResult(userInfo);
    //    });

    //    return await tcs.Task;
    //}

    public static async Task<UserInfo> FetchUserInfoByIdAsync(string userId)
    {
        userInfo = new UserInfo();

        DatabaseReference rootReference = FirebaseDatabase.DefaultInstance.RootReference;
        DatabaseReference userReference = rootReference.Child("users").Child(userId);

        TaskCompletionSource<UserInfo> tcs = new TaskCompletionSource<UserInfo>();

        try
        {
            DataSnapshot snapshot = await userReference.GetValueAsync();

            if (snapshot.Exists)
            {
                userInfo.DisplayName = snapshot.Child("displayName").Value.ToString();
                userInfo.Handle = snapshot.Child("handle").Value.ToString();
                userInfo.UserId = userId;
                userInfo.ProfilePicture = snapshot.Child("profilePicture").Value.ToString();
                userInfo.Title = snapshot.Child("title").Value.ToString();
                userInfo.LastSignIn = long.Parse(snapshot.Child("lastSignIn").Value.ToString());
                userInfo.StoriesRead = int.Parse(snapshot.Child("storiesRead").Value.ToString());
                userInfo.GamesPlayed = int.Parse(snapshot.Child("gamesPlayed").Value.ToString());
                userInfo.StoryXp = float.Parse(snapshot.Child("storyXp").Value.ToString());
                userInfo.GameXp = float.Parse(snapshot.Child("gameXp").Value.ToString());
                userInfo.StoryTitlesWon = int.Parse(snapshot.Child("storyTitlesWon").Value.ToString());
                userInfo.GameTitlesWon = int.Parse(snapshot.Child("gameTitlesWon").Value.ToString());
                userInfo.CountryCode = snapshot.Child("countryCode").Value.ToString();
                userInfo.CountryName = snapshot.Child("countryName").Value.ToString();
                userInfo.Followers = new List<string>();
                userInfo.Following = new List<string>();

                DatabaseReference followersReference = userReference.Child("followers");
                DataSnapshot followersSnapshot = await followersReference.GetValueAsync();

                if (followersSnapshot.Exists)
                {
                    foreach (var childSnapshot in followersSnapshot.Children)
                    {
                        string followerUid = childSnapshot.Value.ToString();
                        userInfo.Followers.Add(followerUid);
                    }
                }

                DatabaseReference followingReference = userReference.Child("following");
                DataSnapshot followingSnapshot = await followingReference.GetValueAsync();

                if (followingSnapshot.Exists)
                {
                    foreach (var childSnapshot in followingSnapshot.Children)
                    {
                        string followingUid = childSnapshot.Value.ToString();
                        userInfo.Following.Add(followingUid);
                    }
                }
            }
            else
            {
                Debug.LogWarning("User could not be found.");
            }

            tcs.SetResult(userInfo);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error fetching user info: " + ex.Message);
            tcs.SetException(ex);
        }

        return await tcs.Task;
    }

    public static UserInfo GetFetchedUserInfo()
    {
        return userInfo;
    }

    public static async Task<List<string>> FetchRecentUsersAsync(int numEntries = 5)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        recentUsers = new List<string>();
        DatabaseReference rootReference = FirebaseDatabase.DefaultInstance.RootReference;
        Query query = rootReference.Child("users").OrderByChild("timestamp").LimitToLast(numEntries);

        TaskCompletionSource<List<string>> tcs = new TaskCompletionSource<List<string>>();

        try
        {
            DataSnapshot snapshot = await query.GetValueAsync();

            if (snapshot.Exists)
            {
                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {
                    string userId = childSnapshot.Key;
                    if (userId == user.UserId) continue;

                    recentUsers.Add(userId);
                }
            }
            else
            {
                Debug.LogWarning("Error fetching most recent entries from query: Snapshot does not exist.");
            }

            tcs.SetResult(recentUsers);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error fetching most recent entries from query: " + ex.Message);
            tcs.SetException(ex);
        }

        return await tcs.Task;
    }

    public static List<string> GetFetchedRecentUsers()
    {
        return recentUsers;
    }

    public static void IncrementStat(string statName, int value)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            DatabaseReference rootReference = FirebaseDatabase.DefaultInstance.RootReference;
            DatabaseReference userReference = rootReference.Child("users").Child(user.UserId);

            userReference.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    DataSnapshot oldStatSnapshot = snapshot.Child(statName);
                    if (oldStatSnapshot.Exists)
                    {
                        int oldValue = int.Parse(oldStatSnapshot.Value.ToString());
                        userReference.Child(statName).SetValueAsync(oldValue + value).ContinueWithOnMainThread(task =>
                        {
                            if (task.IsCompleted)
                            {
                                Debug.Log($"Successfully updated stat {statName}");
                            }
                            else
                            {
                                Debug.LogWarning($"Unable to update stat {statName}");
                            }
                        });
                    }
                    else
                    {
                        userReference.Child(statName).SetValueAsync(0).ContinueWithOnMainThread(task =>
                        {
                            if (task.IsCompleted)
                            {
                                Debug.Log($"Successfully updated stat {statName}");
                            }
                            else
                            {
                                Debug.LogWarning($"Unable to update stat {statName}");
                            }
                        });
                    }
                }
                else
                {
                    Debug.LogWarning($"Error fetching stat {statName}");
                }
            });
        }
        else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }
}
