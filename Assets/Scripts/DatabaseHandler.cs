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
                        DataSnapshot codeNameSnapshot = minigameSnapshot.Child("CodeName");

                        string nameValue = nameSnapshot?.Value?.ToString() ?? "undefined";
                        string descriptionValue = descriptionSnapshot?.Value?.ToString() ?? "undefined";
                        string codeNameValue = codeNameSnapshot?.Value?.ToString() ?? "undefined";

                        // Handle the minigame data as needed
                        //Debug.Log($"Added minigame {nameValue}");
                        Minigame minigame = new Minigame(nameValue, descriptionValue, codeNameValue, Activity.ActivityType.Minigame);
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
                        DataSnapshot codeNameSnapshot = storySnapshot.Child("CodeName");

                        string nameValue = nameSnapshot?.Value?.ToString() ?? "undefined";
                        string descriptionValue = descriptionSnapshot?.Value?.ToString() ?? "undefined";
                        string codeNameValue = codeNameSnapshot?.Value?.ToString() ?? "undefined";

                        // Handle the story data as needed
                        //Debug.Log($"Added story {nameValue}");
                        Story story = new Story(nameValue, descriptionValue, codeNameValue, Activity.ActivityType.Story);
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
                userInfo.ProfilePicture = int.Parse(snapshot.Child("profilePicture").Value.ToString());
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
                userInfo.IsDiscoverable = (bool)(snapshot.Child("isDiscoverable").Value ?? true);
                userInfo.DailyStreak = int.Parse((snapshot.Child("dailyStreak").Value ?? 0).ToString());
                userInfo.LastActivityCompleted = (snapshot.Child("lastActivityCompleted").Value ?? "0001-01-01T00:00:00").ToString();
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

                    DataSnapshot isDiscoverable = childSnapshot.Child("isDiscoverable");
                    if (isDiscoverable.Exists)
                    {
                        bool value = (bool)isDiscoverable.Value;
                        if (value == true)
                        {
                            recentUsers.Add(userId);
                        }
                    }
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

    public static void FollowUser(string userId, Action callback = null)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            DatabaseReference rootReference = FirebaseDatabase.DefaultInstance.RootReference;
            DatabaseReference usersReference = rootReference.Child("users");
            DatabaseReference userReference = usersReference.Child(user.UserId);

            CheckIfKeyExists(usersReference, userId, (targetExists) =>
            {
                if (targetExists)
                {
                    DatabaseReference otherUserReference = usersReference.Child(userId);

                    CheckIfKeyExists(userReference.Child("following"), userId, (alreadyFollowing) =>
                    {
                        if (!alreadyFollowing)
                        {
                            userReference.Child("following").Child(userId).SetValueAsync(userId);
                            otherUserReference.Child("followers").Child(user.UserId).SetValueAsync(user.UserId);
                            Debug.Log("Successfully followed user " + userId);

                            if (callback != null)
                            {
                                callback.Invoke();
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Error: You're already following that user!");
                        }
                    });
                } else
                {
                    Debug.LogError("Error: A user with that userId does not exist!");
                }
            });
        }
        else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }

    static void CheckIfKeyExists(DatabaseReference parentReference, string key, Action<bool> callback)
    {
        parentReference.Child(key).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                bool exists = snapshot.Exists;
                callback(exists);
            }
            else
            {
                Debug.LogError("Could not complete CheckIfKeyExists operation: " + task.Exception);
                callback(false);
            }
        });
    }

    public static void FetchDatabaseValue(string databasePathToValue, bool fallbackValue, Action<bool> callback)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            string formattedString = string.Format(databasePathToValue, user.UserId);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child(formattedString);
            reference.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        bool value = (bool)snapshot.Value;
                        callback(value);
                    }
                    else
                    {
                        Debug.LogWarning("A value at the path of " + formattedString + " does not exist!");
                        callback(fallbackValue);
                    }
                }
                else
                {
                    Debug.LogError("Could not complete FetchDatabaseValue operation: " + task.Exception);
                    callback(fallbackValue);
                }
            });
        } else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }

    public static void FetchDatabaseValue(string databasePathToValue, int fallbackValue, Action<int> callback)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            string formattedString = string.Format(databasePathToValue, user.UserId);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child(formattedString);
            reference.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        string value = snapshot.Value.ToString();
                        int parsedValue = int.Parse(value);
                        callback(parsedValue);
                    }
                    else
                    {
                        Debug.LogWarning("A value at the path of " + formattedString + " does not exist!");
                        callback(fallbackValue);
                    }
                }
                else
                {
                    Debug.LogError("Could not complete FetchDatabaseValue operation: " + task.Exception);
                    callback(fallbackValue);
                }
            });
        }
        else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }

    public static void FetchDatabaseValue(string databasePathToValue, string fallbackValue, Action<string> callback)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            string formattedString = string.Format(databasePathToValue, user.UserId);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child(formattedString);
            reference.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        string value = snapshot.Value.ToString();
                        callback(value);
                    }
                    else
                    {
                        Debug.LogWarning("A value at the path of " + formattedString + " does not exist!");
                        callback(fallbackValue);
                    }
                }
                else
                {
                    Debug.LogError("Could not complete FetchDatabaseValue operation: " + task.Exception);
                    callback(fallbackValue);
                }
            });
        }
        else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }

    public static void SetDatabaseValue(string databasePathToValue, bool value)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            string formattedString = string.Format(databasePathToValue, user.UserId);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child(formattedString);
            reference.SetValueAsync(value);
        }
        else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }

    public static void SetDatabaseValue(string databasePathToValue, string value)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            string formattedString = string.Format(databasePathToValue, user.UserId);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child(formattedString);
            reference.SetValueAsync(value);
        }
        else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }

    public static void SetDatabaseValue(string databasePathToValue, int value)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            string formattedString = string.Format(databasePathToValue, user.UserId);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child(formattedString);
            reference.SetValueAsync(value);
        }
        else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }

    public static void RemoveDatabaseValue(string databasePathToValue)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            string formattedString = string.Format(databasePathToValue, user.UserId);
            DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference.Child(formattedString);
            reference.RemoveValueAsync();
        }
        else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }
}
