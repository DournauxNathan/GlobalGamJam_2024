using TMPro;
using UnityEngine;

namespace HelloGameDev
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private bool DisplayMilliseconds;
        [Header("Required References")]
        [SerializeField] private TMP_Text TimerLabel;

        private float _startTime;

        private void Awake()
        {
            _startTime = Time.time;

            // TODO Register to timer stop event
            // Example.OnVictory += DisableComponent;
        }

        private void OnDestroy()
        {
            // TODO Unregister to timer stop event
            // Example.OnVictory -= DisableComponent;
        }

        private void Update()
        {
            var elapsed = Time.time - _startTime;
            var seconds = (Mathf.Floor(elapsed) % 60f).ToString("00");
            var minutes = Mathf.Floor(elapsed / 60f).ToString("00");

            TimerLabel.text = $"{minutes}:{seconds}";

            if (DisplayMilliseconds)
                TimerLabel.text += $":{((elapsed * 100f) % 99f).ToString("00")}";
        }

        private void DisableComponent()
        {
            enabled = false;
        }
    }
}
