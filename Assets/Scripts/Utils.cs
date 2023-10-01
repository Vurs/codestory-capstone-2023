using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Utils : MonoBehaviour
{
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
        Debug.Log("Loaded Country image!");
    }
}
