using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Data;
using System;

public class ToggleSwitch : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private bool _isOn = false;
    public bool isOn
    {
        get
        {
            return _isOn;
        }
    }

    [SerializeField]
    private RectTransform toggleIndicator;
    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Color onColor;
    [SerializeField]
    private Color offColor;

    private float offX;
    private float onX;

    [SerializeField]
    private float tweenTime = 0.25f;

    public delegate void ValueChanged(bool value);
    public event ValueChanged valueChanged;

    public bool defaultValue = false;
    public string playerPrefsKey = "";
    public bool isDatabaseValue = false;
    public string databasePathToValue = "";

    // Start is called before the first frame update
    void Start()
    {
        offX = 0; // toggleIndicator.anchoredPosition.x; // Start position
        onX = 58.54258f; // backgroundImage.rectTransform.rect.width - (toggleIndicator.rect.width * 0.775f);

        if (isDatabaseValue == true)
        {
            DatabaseHandler.FetchDatabaseValue(databasePathToValue, true, (boolValue) =>
            {
                ForceToggle(boolValue);
            });
        } else
        {
            if (PlayerPrefs.HasKey(playerPrefsKey) == true)
            {
                ForceToggle(PlayerPrefs.GetInt(playerPrefsKey) == 1);
            }
            else
            {
                ForceToggle(defaultValue);
            }
        }
    }

    private void OnEnable()
    {
        Toggle(isOn); // Make sure the switch is set correctly
    }

    public void ForceToggle(bool value)
    {
        _isOn = value;

        ToggleColor(isOn);
        MoveIndicator(isOn);

        if (valueChanged != null)
        {
            valueChanged(isOn);
        }
    }

    public void Toggle(bool value, Action callback = null)
    {
        if (value != isOn)
        {
            _isOn = value;

            ToggleColor(isOn);
            MoveIndicator(isOn);

            if (valueChanged != null)
            {
                valueChanged(isOn);
            }

            // Handle logic when value is changed, maybe perform a callback
            if (callback != null)
            {
                callback.Invoke();
            }
        }
    }

    private void ToggleColor(bool value)
    {
        if (value)
        {
            backgroundImage.DOColor(onColor, tweenTime);
        } else
        {
            backgroundImage.DOColor(offColor, tweenTime);
        }
    }

    private void MoveIndicator(bool value)
    {
        if (value)
        {
            toggleIndicator.DOAnchorPosX(onX, tweenTime);
        } else
        {
            toggleIndicator.DOAnchorPosX(offX, tweenTime);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Toggle(!isOn, () =>
        {
            int value = isOn == true ? 1 : 0;

            if (isDatabaseValue == true)
            {
                DatabaseHandler.SetDatabaseValue(databasePathToValue, isOn);
            } else
            {
                PlayerPrefs.SetInt(playerPrefsKey, value);
            }
        });
    }
}
