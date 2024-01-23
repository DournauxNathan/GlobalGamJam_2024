using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public RectTransform crosshair;
    public float crossHairFollowFactor;

    public Slider sadSlider;

    private void Awake()
    {
        if (UIManager.Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetSlider(float maxValue, float currentValue)
    {
        sadSlider.maxValue = maxValue;
    }

    public void UpdateSlider()
    {
        temperatureText.text = LevelManager.instance.temperature.ToString("0") + "°C";
        temperatureBar.fillAmount = LevelManager.instance.temperature / LevelManager.instance.maxTemperature;
    }



}
