using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCImageHandler : MonoBehaviour
{
    public Texture2D[] images;

    public void ApplyTexture(Image image, Texture2D texture)
    {
        if (texture != null)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            image.sprite = sprite;
            image.color = Color.white;
        }
    }
}
