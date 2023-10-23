using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ProfilePictureManager : MonoBehaviour
{
    public Dictionary<string, Texture2D> profilePictures = new Dictionary<string, Texture2D>();

    private DatabaseReference databaseReference;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference.Child("profile_pictures");

            // Fetch and load profile pictures.
            LoadProfilePictures();
        });
    }

    void LoadProfilePictures()
    {
        databaseReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                profilePictures.Clear();

                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {
                    string imageId = childSnapshot.Key;
                    string imageUrl = childSnapshot.Value.ToString();
                    StartCoroutine(LoadImage(imageId, imageUrl));
                }
            }
            else
            {
                Debug.LogError("Failed to fetch profile pictures: " + task.Exception);
            }
        });
    }

    IEnumerator LoadImage(string imageId, string imageUrl)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            profilePictures.Add(imageId,texture);
        }
        else
        {
            Debug.LogError("Failed to load image: " + www.error);
        }
    }

    public void SetImage(Image targetImage, Texture2D texture)
    {
        targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }
}
