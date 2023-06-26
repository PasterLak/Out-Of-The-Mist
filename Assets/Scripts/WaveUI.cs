using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class WaveUI : MonoBehaviour
    {
        public WavesController Waves;
        public GameObject WaveBar;
        public TMP_Text InfoText;
        public TMP_Text WaveText;
        public Slider InfoSlider;


        private void Start()
        {
            UpdateWave();
            WavesController.OnTimeUpdated += UpdateInfoTimeBefore;
            WavesController.OnWaveStarted += UpdateWave;
            WavesController.OnEnemiesUpdated += UpdateEnemiesCount;
        }

        public void UpdateInfoTimeBefore(float time, float max)
        {
            if(InfoSlider.maxValue != max)
                InfoSlider.maxValue = max;
            
            InfoSlider.value = time;
            InfoText.text = "Time left: " + ConvertTimeToMMSS(time);
        }
        
        public void UpdateWave()
        {
            WaveText.text = "Wave: " + Waves.waveNumber;
        }
        
        public void UpdateEnemiesCount(int current, int max)
        {
            if(InfoSlider.maxValue != max)
            InfoSlider.maxValue = max;
            
            InfoSlider.value = current;
            InfoText.text = "Enemies: " + current + "/" + max;
        }
        
        public static string ConvertTimeToMMSS(float timeInSeconds) 
        {
            int timeInt = (int)Mathf.Round(timeInSeconds);

            int min = timeInt / 60;
            int sec = timeInt % 60;

            return string.Format("{0:00}:{1:00}", min, sec);
        }
    }
    
    
}