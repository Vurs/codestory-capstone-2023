using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Utils : MonoBehaviour
{
    public static Dictionary<string, Color32> TabColors = new Dictionary<string, Color32> {
        { "BLUE", new Color32(1, 95, 200, 255) },
        { "GRAY", new Color32(44, 53, 63, 255) },
        { "TAB_UNUSED", new Color32(255, 255, 255, 67) },
        { "TAB_USED", new Color32(255, 255, 255, 255) }
    };

    public static string ConvertToMS(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return $"{minutes}:{seconds:D2}";
    }

    public static IEnumerator LoadFlagImageCoroutine(Image image, string countryCode)
    {
        string flagApiUrl = $"https://flagsapi.com/{countryCode}/flat/64.png";

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(flagApiUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            SetImage(image, texture);
        }
        else
        {
            Debug.LogError("Failed to load image: " + www.error);
        }
    }

    static void SetImage(Image image, Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        image.sprite = sprite;
        //Debug.Log("Loaded Country image!");
    }

    public static void ClearAllChildren(GameObject parent)
    {
        var children = new List<GameObject>();

        foreach (Transform child in parent.transform)
            children.Add(child.gameObject);

        children.ForEach(child => Destroy(child));
    }

    public static string ConvertToShorthand(float number)
    {
        string[] suffixes = { "", "K", "M", "B", "T" };
        int suffixIndex = 0;

        while (number >= 1000.0 && suffixIndex < suffixes.Length - 1)
        {
            number /= 1000.0f;
            suffixIndex++;
        }

        string result = number.ToString("0.0");
        if (result.EndsWith(".0"))
        {
            result = number.ToString("0");
        }

        if (suffixIndex > 0)
        {
            result += suffixes[suffixIndex];
        }

        return result;
    }

    public static string FormatWithCommas(int input)
    {
        return input.ToString("N0");
    }

    public static string FormatTimeAgo(long unixTimestampMilliseconds)
    {
        DateTime currentTime = DateTime.UtcNow;
        DateTime targetTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimestampMilliseconds).UtcDateTime;

        TimeSpan timeDifference = currentTime - targetTime;

        if (timeDifference.TotalMinutes <= 5)
        {
            return "Just now";
        }
        if (timeDifference.TotalDays >= 1)
        {
            int daysAgo = (int)timeDifference.TotalDays;
            return $"{daysAgo} {(daysAgo == 1 ? "day" : "days")} ago";
        }
        if (timeDifference.TotalHours >= 1)
        {
            int hoursAgo = (int)timeDifference.TotalHours;
            return $"{hoursAgo} {(hoursAgo == 1 ? "hour" : "hours")} ago";
        }
        if (timeDifference.TotalMinutes >= 1)
        {
            int minutesAgo = (int)timeDifference.TotalMinutes;
            return $"{minutesAgo} {(minutesAgo == 1 ? "minute" : "minutes")} ago";
        }
        if (timeDifference.TotalSeconds >= 1)
        {
            int secondsAgo = (int)timeDifference.TotalSeconds;
            return $"{secondsAgo} {(secondsAgo == 1 ? "second" : "seconds")} ago";
        }

        return "Just now";
    }
}
