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
    public GameObject homePage;
    public GameObject discoverPeople;
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
    public GameObject followUnfollowButton;

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
    public UserInfo currentlyViewedUser;

    private List<string> recentUsers;
    public GameObject discoverUserPrefab;
    public GameObject discoverContainer;

    public ProfilePictureManager profilePictureManager;

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
                StartCoroutine(PopulateFollowingFollowers(() =>
                {
                    StartCoroutine(GetMostRecentUsersCoroutine(() => {
                        StartCoroutine(PopulateDiscoverUsers());
                    }));
                }));
            }));
        } else
        {
            Debug.LogError("No user is currently authenticated.");
        }

        profilePictureManager = GameObject.Find("ProfilePictureManager").GetComponent<ProfilePictureManager>();
    }

    public void PopulateHome()
    {
        // Map all the info to the corresponding text labels
        greetingLabelHome.text = $"Hello, {userInfo.DisplayName}!";
        handleLabelHome.text = $"@{userInfo.Handle}";
        followingFollowersLabelHome.text = $"<b>{Utils.FormatWithCommas(userInfo.Following.Count)}</b> Following   <b>{Utils.FormatWithCommas(userInfo.Followers.Count)}</b> Followers";
        dailyStreakLabelHome.text = userInfo.DailyStreak.ToString();
        totalXpLabelHome.text = Utils.ConvertToShorthand(userInfo.StoryXp + userInfo.GameXp);
        totalTitlesLabelHome.text = Utils.FormatWithCommas(userInfo.StoryTitlesWon + userInfo.GameTitlesWon);
        profilePictureManager.SetImage(profilePictureHome, profilePictureManager.profilePictures["pfp_" + userInfo.ProfilePicture.ToString("D3")]);
    }

    public void RepopulateHome()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            StartCoroutine(GetUserDataCoroutine(user.UserId, () => {
                PopulateHome();
            }));
        }
    }

    public void PopulateProfile()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            currentlyViewedUser = userInfo;

            if (user.UserId == userInfo.UserId)
            {
                followUnfollowButton.SetActive(false);
            } else
            {
                DatabaseHandler.FetchDatabaseValue("users/{0}/following/" + userInfo.UserId, "", (value) =>
                {
                    TMP_Text buttonText = followUnfollowButton.transform.Find("Text (TMP)").GetComponent<TMP_Text>();

                    if (!string.IsNullOrEmpty(value))
                    {
                        buttonText.text = "Unfollow";
                    } else
                    {
                        buttonText.text = "Follow";
                    }

                    followUnfollowButton.SetActive(true);
                });
            }

            displayNameLabelProfile.text = userInfo.DisplayName;
            handleLabelProfile.text = $"@{userInfo.Handle}";
            titleLabelProfile.text = userInfo.Title;
            countryLabelProfile.text = userInfo.CountryName;
            StartCoroutine(Utils.LoadFlagImageCoroutine(countryImageProfile, userInfo.CountryCode));
            lastActiveLabelProfile.text = userInfo.UserId == user.UserId ? "Last Active: Just now" : "Last Active: " + Utils.FormatTimeAgo(userInfo.LastSignIn);
            followingFollowersLabelProfile.text = $"<b>{Utils.FormatWithCommas(userInfo.Following.Count)}</b> Following   <b>{Utils.FormatWithCommas(userInfo.Followers.Count)}</b> Followers";
            dailyStreakLabelProfile.text = userInfo.DailyStreak.ToString();
            totalXpLabelProfile.text = Utils.ConvertToShorthand(userInfo.StoryXp + userInfo.GameXp);
            totalTitlesLabelProfile.text = Utils.FormatWithCommas(userInfo.StoryTitlesWon + userInfo.GameTitlesWon);
            storiesReadLabelProfile.text = Utils.ConvertToShorthand(userInfo.StoriesRead);
            storyXpLabelProfile.text = Utils.ConvertToShorthand(userInfo.StoryXp);
            storyTitlesLabelProfile.text = Utils.FormatWithCommas(userInfo.StoryTitlesWon);
            gamesPlayedLabelProfile.text = Utils.ConvertToShorthand(userInfo.GamesPlayed);
            gameXpLabelProfile.text = Utils.ConvertToShorthand(userInfo.GameXp);
            gameTitlesWonProfile.text = Utils.FormatWithCommas(userInfo.GameTitlesWon);
            profilePictureManager.SetImage(profilePictureProfile, profilePictureManager.profilePictures["pfp_" + userInfo.ProfilePicture.ToString("D3")]);
        }
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

    public IEnumerator GetUserDataCoroutine(string userId, Action callback)
    {
        Task task = DatabaseHandler.FetchUserInfoByIdAsync(userId);
        yield return new WaitUntil(() => task.IsCompleted);

        userInfo = DatabaseHandler.GetFetchedUserInfo();

        //Debug.Log("Successfully loaded User Info");

        callback.Invoke();
    }

    IEnumerator PopulateFollowingFollowers(Action callback = null)
    {
        currentUserDisplayNameFF.text = userInfo.DisplayName;
        followingButtonText.text = $"<b>Following</b> {Utils.FormatWithCommas(userInfo.Following.Count)}";
        followersButtonText.text = $"<b>Followers</b> {Utils.FormatWithCommas(userInfo.Followers.Count)}";

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

                profilePictureManager.SetImage(profilePictureImage, profilePictureManager.profilePictures["pfp_" + userInfo.ProfilePicture.ToString("D3")]);
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

                profilePictureManager.SetImage(profilePictureImage, profilePictureManager.profilePictures["pfp_" + userInfo.ProfilePicture.ToString("D3")]);
                displayNameLabel.text = userInfo.DisplayName;
                usernameLabel.text = $"@{userInfo.Handle}";
                followerClone.transform.SetParent(followersContainer.transform, false);

                Button button = followerClone.GetComponent<Button>();
                button.onClick.AddListener(() => { OnFollowingFollowerClicked(followerUserId); });

                isCompleted = true;
            }));

            yield return new WaitUntil(() => isCompleted == true);
        }

        if (callback  != null)
        {
            callback.Invoke();
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

    void OnDiscoverClicked(string userId)
    {
        StartCoroutine(GetUserDataCoroutine(userId, () => {
            PopulateProfile();
            StartCoroutine(PopulateFollowingFollowers());
        }));

        homePage.SetActive(false);
        profilePage.SetActive(true);
    }

    IEnumerator GetMostRecentUsersCoroutine(Action callback)
    {
        Task task = DatabaseHandler.FetchRecentUsersAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        recentUsers = DatabaseHandler.GetFetchedRecentUsers();

        callback.Invoke();
    }

    IEnumerator PopulateDiscoverUsers()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            Utils.ClearAllChildren(discoverContainer);

            if (recentUsers.Count > 0)
            {
                foreach (string userId in recentUsers)
                {
                    bool isCompleted = false;

                    StartCoroutine(GetUserDataCoroutine(userId, () =>
                    {
                        GameObject discoverClone = Instantiate(discoverUserPrefab);
                        GameObject profilePictureHolder = discoverClone.transform.Find("ProfilePicture").gameObject;
                        Image profilePictureImage = profilePictureHolder.transform.Find("Image").gameObject.GetComponent<Image>();
                        TMP_Text displayNameLabel = discoverClone.transform.Find("DisplayName").gameObject.GetComponent<TMP_Text>();
                        TMP_Text usernameLabel = discoverClone.transform.Find("Username").gameObject.GetComponent<TMP_Text>();
                        TMP_Text followButtonText = discoverClone.transform.Find("FollowButton").Find("Text (TMP)").GetComponent<TMP_Text>();

                        profilePictureManager.SetImage(profilePictureImage, profilePictureManager.profilePictures["pfp_" + userInfo.ProfilePicture.ToString("D3")]);
                        displayNameLabel.text = userInfo.DisplayName;
                        usernameLabel.text = $"@{userInfo.Handle}";

                        DatabaseHandler.FetchDatabaseValue("users/{0}/following/" + userId, "", (value) =>
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                followButtonText.text = "Following";
                            }
                        });
    
                    discoverClone.transform.SetParent(discoverContainer.transform, false);

                        Button button = discoverClone.transform.Find("FollowButton").GetComponent<Button>();
                        button.onClick.AddListener(() => { OnFollowClicked(userId); });

                        Button profileButton = profilePictureHolder.GetComponent<Button>();
                        profileButton.onClick.AddListener(() => { OnDiscoverClicked(userId); });

                        isCompleted = true;
                    }));

                    yield return new WaitUntil(() => isCompleted == true);
                }
            }
            else
            {
                discoverPeople.SetActive(false);
            }
        }
    }

    void OnFollowClicked(string userId)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            Debug.Log("Attempting to follow " + userId);
            DatabaseHandler.FollowUser(userId, () => {
                StartCoroutine(GetUserDataCoroutine(user.UserId, () =>
                {
                    PopulateHome();
                    StartCoroutine(PopulateDiscoverUsers());
                }));
            });
        } else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }

    public void OnFollowUnfollowButtonClicked()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            if (currentlyViewedUser.UserId == user.UserId)
            {
                Debug.LogWarning("Called follow/unfollow on self, aborting");
                return;
            }

            DatabaseHandler.FetchDatabaseValue("users/{0}/following/" + currentlyViewedUser.UserId, "", (value) =>
            {
                TMP_Text buttonText = followUnfollowButton.transform.Find("Text (TMP)").GetComponent<TMP_Text>();

                if (!string.IsNullOrEmpty(value))
                {
                    Debug.Log("Attempting to unfollow " + currentlyViewedUser.UserId);
                    DatabaseHandler.RemoveDatabaseValue("users/{0}/following/" + currentlyViewedUser.UserId);
                    DatabaseHandler.RemoveDatabaseValue("users/" + currentlyViewedUser.UserId + "/followers/" + user.UserId);

                    StartCoroutine(GetUserDataCoroutine(currentlyViewedUser.UserId, () =>
                    {
                        PopulateProfile();
                        StartCoroutine(PopulateFollowingFollowers(() =>
                        {
                            StartCoroutine(GetUserDataCoroutine(user.UserId, () =>
                            {
                                buttonText.text = "Follow";
                                PopulateHome();
                                StartCoroutine(PopulateDiscoverUsers());
                            }));
                        }));
                    }));
                } else
                {
                    Debug.Log("Attempting to follow " + userInfo.UserId);

                    DatabaseHandler.FollowUser(currentlyViewedUser.UserId, () => {
                        StartCoroutine(GetUserDataCoroutine(currentlyViewedUser.UserId, () =>
                        {
                            PopulateProfile();
                            StartCoroutine(PopulateFollowingFollowers(() =>
                            {
                                StartCoroutine(GetUserDataCoroutine(user.UserId, () =>
                                {
                                    buttonText.text = "Unfollow";
                                    PopulateHome();
                                    StartCoroutine(PopulateDiscoverUsers());
                                }));
                            }));
                        }));
                    });
                }
            });
        }
        else
        {
            Debug.LogError("No user is currently authenticated.");
        }
    }
}
