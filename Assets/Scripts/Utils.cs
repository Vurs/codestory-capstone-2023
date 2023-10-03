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
        { "GRAY", new Color32(44, 53, 63, 255) }
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

    public static string ConvertNumberToShorthand(int number)
    {
        string[] suffixes = { "K", "M", "B", "T" };

        int index = 0;
        double num = (double)number;

        // Keep dividing the number by 1000 and moving to the next suffix until it's less than 1000
        while (num >= 1000 && index < suffixes.Length - 1)
        {
            num /= 1000;
            index++;
        }

        // Format the number with the appropriate suffix and decimal place
        string result = num.ToString("0.0") + suffixes[index];
        return result;
    }

    public static string ConvertNumberToShorthand(float number)
    {
        string[] suffixes = { "K", "M", "B", "T" };

        int index = 0;
        double num = (double)number;

        // Keep dividing the number by 1000 and moving to the next suffix until it's less than 1000
        while (num >= 1000 && index < suffixes.Length - 1)
        {
            num /= 1000;
            index++;
        }

        // Format the number with the appropriate suffix and decimal place
        string result = num.ToString("0.0") + suffixes[index];
        return result;
    }

    public static string FormatWithCommas(int input)
    {
        return input.ToString("N0");
    }
}
