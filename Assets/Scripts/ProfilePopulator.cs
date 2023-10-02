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
using UnityEditor.Animations;

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
    public GameObject profilePage;
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

    [Header("Following/Followers UI")]
    public GameObject followingFollowersPage;
    public GameObject followersScroller;
    public GameObject followingScroller;
    public GameObject followersContainer;
    public GameObject followingContainer;
    public GameObject followingFollowerPrefab;
    public TMP_Text followingButtonText;
    public TMP_Text followersButtonText;
    public TMP_Text currentUserDisplayNameFF;

    public UserInfo userInfo;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            StartCoroutine(GetUserDataCoroutine(user.UserId, () => {
                PopulateHome();
                PopulateProfile();
                StartCoroutine(PopulateFollowingFollowers());
            }));
        } else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }

    public void PopulateHome()
    {
        // Map all the info to the corresponding text labels
        greetingLabelHome.text = $"Hello, {userInfo.DisplayName}!";
        handleLabelHome.text = $"@{userInfo.Handle}";
        followingFollowersLabelHome.text = $"<b>{userInfo.Following.Count}</b> Following   <b>{userInfo.Followers.Count}</b> Followers";
        dailyStreakLabelHome.text = "0"; // Change this later
        totalXpLabelHome.text = (userInfo.StoryXp + userInfo.GameXp).ToString();
        totalTitlesLabelHome.text = (userInfo.StoryTitlesWon + userInfo.GameTitlesWon).ToString();
        profilePictureHome.sprite = defaultProfilePicture;
    }

    public void PopulateProfile()
    {
        displayNameLabelProfile.text = userInfo.DisplayName;
        handleLabelProfile.text = $"@{userInfo.Handle}";
        titleLabelProfile.text = userInfo.Title;
        countryLabelProfile.text = userInfo.CountryName;
        //countryImageProfile.sprite = defaultProfilePicture; // Change this later
        StartCoroutine(Utils.LoadFlagImageCoroutine(countryImageProfile, userInfo.CountryCode));
        lastActiveLabelProfile.text = "Last Active: Just now"; // Change this later
        followingFollowersLabelProfile.text = $"<b>{userInfo.Following.Count}</b> Following   <b>{userInfo.Followers.Count}</b> Followers";
        dailyStreakLabelProfile.text = "0"; // Change this later
        totalXpLabelProfile.text = (userInfo.StoryXp + userInfo.GameXp).ToString();
        totalTitlesLabelProfile.text = (userInfo.StoryTitlesWon + userInfo.GameTitlesWon).ToString();
        storiesReadLabelProfile.text = userInfo.StoriesRead.ToString();
        storyXpLabelProfile.text = userInfo.StoryXp.ToString();
        storyTitlesLabelProfile.text = userInfo.StoryTitlesWon.ToString();
        gamesPlayedLabelProfile.text = userInfo.GamesPlayed.ToString();
        gameXpLabelProfile.text = userInfo.GameXp.ToString();
        gameTitlesWonProfile.text = userInfo.GameTitlesWon.ToString();
        profilePictureProfile.sprite = defaultProfilePicture; // Change this later
    }

    public void PopulateOwnProfile()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            StartCoroutine(GetUserDataCoroutine(user.UserId, () => {
                PopulateProfile();
                StartCoroutine(PopulateFollowingFollowers());
            }));
        }
        else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }

    IEnumerator GetUserDataCoroutine(string userId, Action callback)
    {
        Task task = DatabaseHandler.FetchUserInfoByIdAsync(userId);
        yield return new WaitUntil(() => task.IsCompleted);

        userInfo = DatabaseHandler.GetFetchedUserInfo();

        //Debug.Log("Successfully loaded User Info");

        callback.Invoke();
    }

    IEnumerator PopulateFollowingFollowers()
    {
        currentUserDisplayNameFF.text = userInfo.DisplayName;
        followingButtonText.text = $"<b>Following</b> {userInfo.Following.Count}";
        followersButtonText.text = $"<b>Followers</b> {userInfo.Followers.Count}";

        Utils.ClearAllChildren(followingContainer);
        Utils.ClearAllChildren(followersContainer);

        List<string> currentUserFollowing = new List<string>();
        foreach (string followingUserId in userInfo.Following)
        {
            currentUserFollowing.Add(followingUserId);
        }

        List<string> currentUserFollowers = new List<string>();
        foreach (string followersUserId in userInfo.Followers)
        {
            currentUserFollowers.Add(followersUserId);
        }

        foreach (string followingUserId in currentUserFollowing)
        {
            bool isCompleted = false;

            StartCoroutine(GetUserDataCoroutine(followingUserId, () =>
            {
                GameObject followingClone = Instantiate(followingFollowerPrefab);
                GameObject profilePictureHolder = followingClone.transform.Find("ProfilePicture").gameObject;
                Image profilePictureImage = profilePictureHolder.transform.Find("Image").gameObject.GetComponent<Image>();
                TMP_Text displayNameLabel = followingClone.transform.Find("DisplayName").gameObject.GetComponent<TMP_Text>();
                TMP_Text usernameLabel = followingClone.transform.Find("Username").gameObject.GetComponent<TMP_Text>();

                profilePictureImage.sprite = defaultProfilePicture; // Change this later
                displayNameLabel.text = userInfo.DisplayName;
                usernameLabel.text = $"@{userInfo.Handle}";
                followingClone.transform.SetParent(followingContainer.transform, false);

                Button button = followingClone.GetComponent<Button>();
                button.onClick.AddListener(() => { OnFollowingFollowerClicked(followingUserId); });

                isCompleted = true;
            }));

            yield return new WaitUntil(() => isCompleted == true);
        }

        foreach (string followerUserId in currentUserFollowers)
        {
            bool isCompleted = false;

            StartCoroutine(GetUserDataCoroutine(followerUserId, () =>
            {
                GameObject followerClone = Instantiate(followingFollowerPrefab);
                GameObject profilePictureHolder = followerClone.transform.Find("ProfilePicture").gameObject;
                Image profilePictureImage = profilePictureHolder.transform.Find("Image").gameObject.GetComponent<Image>();
                TMP_Text displayNameLabel = followerClone.transform.Find("DisplayName").gameObject.GetComponent<TMP_Text>();
                TMP_Text usernameLabel = followerClone.transform.Find("Username").gameObject.GetComponent<TMP_Text>();

                profilePictureImage.sprite = defaultProfilePicture; // Change this later
                displayNameLabel.text = userInfo.DisplayName;
                usernameLabel.text = $"@{userInfo.Handle}";
                followerClone.transform.SetParent(followersContainer.transform, false);

                Button button = followerClone.GetComponent<Button>();
                button.onClick.AddListener(() => { OnFollowingFollowerClicked(followerUserId); });

                isCompleted = true;
            }));

            yield return new WaitUntil(() => isCompleted == true);
        }
    }

    void OnFollowingFollowerClicked(string userId)
    {
        StartCoroutine(GetUserDataCoroutine(userId, () => {
            PopulateProfile();
            StartCoroutine(PopulateFollowingFollowers());
        }));

        followingFollowersPage.SetActive(false);
        profilePage.SetActive(true);
    }
}
