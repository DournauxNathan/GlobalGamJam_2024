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

    public TextMeshProUGUI districtNameText;
    public Slider completionSlider;

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
        sadSlider.value = sadSlider.value / sadSlider.maxValue;
    }

    public void SetDistrictInfo(string districtName, float completion)
    {
        districtNameText.text = districtName;

        completionSlider.value = completion;
    }

}
