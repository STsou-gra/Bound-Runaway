using System;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float delayTime = 3.0f;
    private String record;
    private void Start()
    {
        SurviveTimer(0);
    }

    private void Update()
    {
        //シーン読み込みからの経過時間から待機時間を引く
        float elapsedTime = Time.timeSinceLevelLoad - delayTime;
        //3秒経つまでelapsedTImeはマイナスなので0で固定して処理を抜ける
        if (elapsedTime < 0)
        {
            SurviveTimer(0);
            return;
        }
        SurviveTimer(elapsedTime);
    }

    private void SurviveTimer(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        int centiseconds = timeSpan.Milliseconds / 10;
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", (int)timeSpan.TotalHours, timeSpan.Minutes, timeSpan.Seconds, centiseconds);

        GameObject[] cpus = GameObject.FindGameObjectsWithTag("CPU");
        if (cpus.Length == 0)
        {
            SaveResult();
        }
    }


    private void SaveResult()
    {
        PlayerPrefs.SetString("ClearTime", timerText.text);
        PlayerPrefs.Save();
    }

}
