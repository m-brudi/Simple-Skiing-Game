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
        bestDist.text = (Controller.Instance.BestDistance * 10).ToString() + "m";
        bestTime.text = Controller.Instance.BestTime.ToString();
        bestSpeed.text = Controller.Instance.BestSpeed.ToString();
    }
}
