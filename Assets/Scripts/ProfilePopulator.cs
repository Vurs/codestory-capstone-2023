using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;

public class ProfilePopulator : MonoBehaviour
{
    [Header("Variables")]
    public Sprite defaultProfilePicture;

    [Header("Home UI")]
    public TMP_Text greetingLabelHome;
    public TMP_Text handleLabelHome;
    public TMP_Text followingFollowersLabelHome;
    public TMP_Text dailyStreakLabelHome;
    public TMP_Text totalXpLabelHome;
    public TMP_Text totalTitlesLabelHome;
    public Image profilePictureHome;

    [Header("Profile UI")]
    public TMP_Text displayNameLabelProfile;
    public TMP_Text handleLabelProfile;
    public TMP_Text titleLabelProfile;
    public TMP_Text countryLabelProfile;
    public Image countryImageProfile;
    public TMP_Text lastActiveLabelProfile;
    public TMP_Text followingFollowersLabelProfile;
    public TMP_Text dailyStreakLabelProfile;
    public TMP_Text totalXpLabelProfile;
    public TMP_Text totalTitlesLabelProfile;
    public TMP_Text storiesReadLabelProfile;
    public TMP_Text storyXpLabelProfile;
    public TMP_Text storyTitlesLabelProfile;
    public TMP_Text gamesPlayedLabelProfile;
    public TMP_Text gameXpLabelProfile;
    public TMP_Text gameTitlesWonProfile;
    public Image profilePictureProfile;

    public UserInfo userInfo;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetProfileDataCoroutine(() => {
            PopulateProfile();
        }));
    }

    public void PopulateProfile()
    {
        // Map all the info to the corresponding text labels
        greetingLabelHome.text = $"Hello, {userInfo.DisplayName}!";
        handleLabelHome.text = $"@{userInfo.Handle}";
        followingFollowersLabelHome.text = $"<b>{(userInfo.Following != null ? userInfo.Following.Count : 0)}</b> Following   <b>{(userInfo.Followers != null ? userInfo.Followers.Count : 0)}</b> Followers";
        dailyStreakLabelHome.text = "0"; // Change this later
        totalXpLabelHome.text = (userInfo.StoryXp + userInfo.GameXp).ToString();
        totalTitlesLabelHome.text = (userInfo.StoryTitlesWon + userInfo.GameTitlesWon).ToString();
        profilePictureHome.sprite = defaultProfilePicture;

        displayNameLabelProfile.text = userInfo.DisplayName;
        handleLabelProfile.text = $"@{userInfo.Handle}";
        titleLabelProfile.text = userInfo.Title;
        countryLabelProfile.text = userInfo.CountryOfOrigin;
        countryImageProfile.sprite = defaultProfilePicture; // Change this later
        lastActiveLabelProfile.text = "Last Active: Just now"; // Change this later
        followingFollowersLabelProfile.text = $"<b>{(userInfo.Following != null ? userInfo.Following.Count : 0)}</b> Following   <b>{(userInfo.Followers != null ? userInfo.Followers.Count : 0)}</b> Followers";
        dailyStreakLabelProfile.text = "0"; // Change this later
        totalXpLabelProfile.text = (userInfo.StoryXp + userInfo.GameXp).ToString();
        totalTitlesLabelProfile.text = (userInfo.StoryTitlesWon + userInfo.GameTitlesWon).ToString();
        storiesReadLabelProfile.text = userInfo.StoriesRead.ToString();
        storyXpLabelProfile.text = userInfo.StoryXp.ToString();
        storyTitlesLabelProfile.text = userInfo.StoryTitlesWon.ToString();
        gamesPlayedLabelProfile.text = userInfo.GamesPlayed.ToString();
        gameXpLabelProfile.text = userInfo.GameXp.ToString();
        gameTitlesWonProfile.text = userInfo.GameTitlesWon.ToString();
        profilePictureProfile.sprite = defaultProfilePicture;
    }

    IEnumerator GetProfileDataCoroutine(Action callback)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null )
        {
            Task task = DatabaseHandler.FetchUserInfoByIdAsync(user.UserId);
            yield return new WaitUntil(() => task.IsCompleted);

            userInfo = DatabaseHandler.GetFetchedUserInfo();

            Debug.Log("Successfully loaded User Info");

            callback.Invoke();
        }
        else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }
}
