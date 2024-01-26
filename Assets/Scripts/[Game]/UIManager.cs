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
    public static Action zoneCleared, allZoneCleared;

    [Header("Timer Settings")]
    [SerializeField] private TextMeshProUGUI timerText; // Reference to TextMeshPro Text component
    public float timer { get; set; }

    [Header("Crosshair Settings")]
    public RectTransform crosshair;
    public float crossHairFollowFactor;

    [Header("Player Infos")]
    [SerializeField] private Slider sadSlider;

    [Header("District Infos")]
    [SerializeField] private TextMeshProUGUI districtNameText;
    [SerializeField] private Slider completionSlider;

    List<ZoneManager> managers = new List<ZoneManager>();
    [SerializeField] private TextMeshProUGUI completeDistrictTracker;
    private int currentZoneCleared;
    private int maxZoneToClear;

    public SoundManager _soundManager;
    public bool updateMusic;

    public GameObject optionsPanel;
    public bool openOptionsMenu { get; set; }

    public bool isGameOver { get; set; }

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

        _soundManager = GetComponent<SoundManager>();
        openOptionsMenu = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptions();
        }

        // Update the timer
        UpdateTimer();

        // Update the TextMeshPro Text with the timer value
        timerText.text = FormatTime(timer);
    }

    private void ToggleOptions()
    {
        openOptionsMenu = !openOptionsMenu;

        Cursor.visible = openOptionsMenu;

        optionsPanel.SetActive(openOptionsMenu);

        if (openOptionsMenu)
        {

            Cursor.lockState = CursorLockMode.None;
        }
        else
        {

            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SetSlider(float maxValue, float currentValue)
    {
        sadSlider.maxValue = maxValue;
        sadSlider.value = currentValue;
    }

    public void UpdateSlider(float value)
    {
        sadSlider.value = value / sadSlider.maxValue;

        if (value <= 0)
        {
            PlayerController.onPlayerDown?.Invoke();
            StartCoroutine(IncreaseSliderOverTime(5f));
        }
    }

    IEnumerator IncreaseSliderOverTime(float duration)
    {
        float elapsedTime = 0f;
        float startValue = sadSlider.minValue;
        float endValue = sadSlider.maxValue;

        while (elapsedTime < duration)
        {
            // Calculate the current value based on the elapsed time
            float currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);

            // Set the slider value
            sadSlider.value = currentValue;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the slider reaches its maximum value
        sadSlider.value = endValue;

        PlayerController.resetPlayer?.Invoke();

        // Coroutine has finished, you can perform any actions here
        Debug.Log("Slider reached maximum value!");
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
        zoneCleared?.Invoke();

        currentZoneCleared++;
        updateMusic = true;

        Debug.Log("");

        UpdateCompleteDistrict();

        if (currentZoneCleared == maxZoneToClear)
        {
            allZoneCleared?.Invoke();
        }
    }

    #region Timer

    void UpdateTimer()
    {
        // Check if the timer has reached 0
        if (!isGameOver && timer >= 0)
        {
            // Decrement the timer by the time elapsed since the last frame
            timer += Time.deltaTime;
        }
    }

    internal string FormatTime(float timeInSeconds)
    {
        // Format the time as minutes:seconds
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    #endregion
}
