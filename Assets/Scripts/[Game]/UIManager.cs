using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public static Action allZoneComplete;

    public RectTransform crosshair;
    public float crossHairFollowFactor;

    public Slider sadSlider;

    public TextMeshProUGUI districtNameText;
    public Slider completionSlider;

    List<ZoneManager> managers = new List<ZoneManager>();
    public TextMeshProUGUI completeDistrictTracker;
    private int currentZoneCleared;
    private int maxZoneToClear;

    public SoundManager _soundManager;

    private void Awake()
    {
        if (UIManager.Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        _soundManager = GetComponent<SoundManager>();
    }

    public void SetSlider(float maxValue, float currentValue)
    {
        sadSlider.maxValue = maxValue;
        sadSlider.value = currentValue;
    }

    public void UpdateSlider(float value)
    {
        sadSlider.value = value / sadSlider.maxValue;
    }

    public void SetDistrictInfo(string districtName, float completion)
    {
        districtNameText.text = districtName;

        completionSlider.value = completion;
    }

    public void SubscribeZoneManager(ZoneManager zoneManager)
    {
        managers.Add(zoneManager);

        maxZoneToClear = managers.Count();
        currentZoneCleared = 0;

        UpdateCompleteDistrict();
    }

    public void UpdateCompleteDistrict()
    {
        completeDistrictTracker.text = currentZoneCleared + " / " + maxZoneToClear;
    }

    public void DistrictComplete()
    {
        _soundManager.PlayDistrictVictorySound();

        currentZoneCleared++;
        UpdateCompleteDistrict();

        if (currentZoneCleared == maxZoneToClear)
        {
            allZoneComplete?.Invoke();
        }
    }
}
