using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rowCounterTxt;
    [SerializeField] SummaryPanel summaryPanel;
    [SerializeField] Transform blackOut;
    [SerializeField] TitleScreen titleScreen;
    [SerializeField] StatsPanel statsPanel;
    [SerializeField] GameObject soundsButtons;
    [SerializeField] Button musicBtn;
    [SerializeField] Button sfxBtn;
    [SerializeField] Color activeBtnColor;
    [SerializeField] Color inactiveBtnColor;
    [SerializeField] GameObject arrows;

    void Start()
    {
        UpdateCounters(0, Controller.Instance.Coins);
        HideBlackOut();
        musicBtn.onClick.RemoveAllListeners();
        sfxBtn.onClick.RemoveAllListeners();
        musicBtn.onClick.AddListener(() => Music());
        sfxBtn.onClick.AddListener(() => Sfx());

        if (Controller.Instance.Music) {
            musicBtn.GetComponent<Image>().color = activeBtnColor;
        } else {
            musicBtn.GetComponent<Image>().color = inactiveBtnColor;
        }
        if (Controller.Instance.SFX) {
            sfxBtn.GetComponent<Image>().color = activeBtnColor;
        } else {
            sfxBtn.GetComponent<Image>().color = inactiveBtnColor;
        }
        HideArrows();
    }

    void Music() {
        Controller.Instance.Music = !Controller.Instance.Music;
        if (Controller.Instance.Music) {
            musicBtn.GetComponent<Image>().color = activeBtnColor;
        } else {
            musicBtn.GetComponent<Image>().color = inactiveBtnColor;
        }
    }

    void Sfx() {
        Controller.Instance.SFX = !Controller.Instance.SFX;
        if (Controller.Instance.SFX) {
            sfxBtn.GetComponent<Image>().color = activeBtnColor;
        } else {
            sfxBtn.GetComponent<Image>().color = inactiveBtnColor;
        }
    }

    public void ShowArrows() {
        arrows.SetActive(true);
    }

    public void HideArrows() {
        arrows.SetActive(false);
    }

    public void ShowTitleScreen() {
        soundsButtons.SetActive(true);
        titleScreen.gameObject.SetActive(true);
        titleScreen.Setup();
    }

    public void HideTitleScreen() {
        soundsButtons.SetActive(false);
        titleScreen.gameObject.SetActive(false);
    }

    public void UpdateCounters(int row, int mons) {
        if(row > 0) rowCounterTxt.text = row.ToString();
    }
    public void ShowSummeryPanel(int dist, float time) {
        soundsButtons.SetActive(true);
        ShowBlackOut();
        rowCounterTxt.text = "";
        summaryPanel.gameObject.SetActive(true);
        summaryPanel.Setup(dist, time);
    }
    public void ShowBlackOut() {
        blackOut.DOScale(1, 0.1f);
    }

    public void HideBlackOut() {
        blackOut.DOScale(0, 0.1f);
    }

    public void ShowStatsPanel() {
        statsPanel.gameObject.SetActive(true);
        statsPanel.Setup();
    }
    public void CloseStatsPanel() {
        statsPanel.gameObject.SetActive(false);

    }
}
