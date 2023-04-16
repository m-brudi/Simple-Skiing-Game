using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StatsPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bestDist;
    [SerializeField] TextMeshProUGUI bestTime;
    [SerializeField] TextMeshProUGUI bestSpeed;
    [SerializeField] TextMeshProUGUI bestSnowman;
    [SerializeField] Button backBtn;
    [SerializeField] Button resetBtn;

    public void Setup() {
        backBtn.onClick.RemoveAllListeners();
        resetBtn.onClick.RemoveAllListeners();

        SetupTxts();

        resetBtn.onClick.AddListener(() => {
            AudioManager.Instance.PlayTap();

            Controller.Instance.ResetSave();
            SetupTxts();
        });

        backBtn.onClick.AddListener(() => {
            Controller.Instance.uiManager.CloseStatsPanel();
            AudioManager.Instance.PlayTap();
        });
    }

    void SetupTxts() {
        bestDist.text = (Controller.Instance.BestDistance * 10).ToString() + " m";
        int time  = Controller.Instance.BestTime;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        bestTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (time > 0) {
            int meters = Controller.Instance.BestDistance * 10;
            float mPerS = meters / time;
            float kmPerH = mPerS * 3.6f;
            bestSpeed.text = Mathf.FloorToInt(kmPerH).ToString() + "Km/h";
        } else {
            bestSpeed.text = "0 Km/h";
        }
        bestSnowman.text = (Controller.Instance.BestSnowman * 10).ToString() + " m";
    }
}
