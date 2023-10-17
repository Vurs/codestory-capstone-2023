using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using Newtonsoft.Json.Linq;
using TMPro;

public class VersionChecker : MonoBehaviour
{
    public string latestVersionURL = "https://codestory-current-version.s3.amazonaws.com/version.txt";
    public bool isLatestVersion = false;

    void Start()
    {
        //StartCoroutine(CheckForUpdate());
    }

    public IEnumerator CheckForUpdate(TMP_Text statusText, Action callback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(latestVersionURL))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning("Error checking for updates: " + www.error);
                yield break;
            }

            string latestVersion = www.downloadHandler.text;
            string currentVersion = GetLocalVersion();

            isLatestVersion = currentVersion == latestVersion;

            if (latestVersion != currentVersion)
            {
                AnimateSpinner.isSpinning = false;
                Debug.Log("A new version is available. Please update your app.");
                statusText.text = "A new version is available. Please update your app.";
            }

            callback.Invoke();
        }
    }

    string GetLocalVersion()
    {
        TextAsset appData = Resources.Load<TextAsset>("appdata");
        if (appData != null)
        {
            string jsonText = appData.text;
            JObject jsonResponse = JObject.Parse(jsonText);
            string buildVersion = (string)jsonResponse["build_version"];
            return buildVersion;
        } else
        {
            Debug.LogError("Failed to load appdata.json file");
            return "";
        }
    }
}
