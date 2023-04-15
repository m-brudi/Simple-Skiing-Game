using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] Button rideBtn;
    [SerializeField] Button statsBtn;

    public void Setup() {
     
        rideBtn.onClick.RemoveAllListeners();
        statsBtn.onClick.RemoveAllListeners();


        rideBtn.onClick.AddListener(() => {
            AudioManager.Instance.PlayTap();
            Controller.Instance.StartNewGame();
        });

        statsBtn.onClick.AddListener(() => {
            AudioManager.Instance.PlayTap();
            Controller.Instance.uiManager.ShowStatsPanel();
        });
    }
}
