using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
public class SummaryPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dstTxt;
    [SerializeField] TextMeshProUGUI timeTxt;
    [SerializeField] TextMeshProUGUI speedTxt;
    [SerializeField] Button retryBtn;
    [SerializeField] Button backBtn;
    [SerializeField] Color recordColor;
    [SerializeField] Color normalColor;
    public void Setup(int dist, float time) {
        AudioManager.Instance.PlayGameOver();
        transform.DOScale(1, 0.3f);
        retryBtn.onClick.RemoveAllListeners();
        backBtn.onClick.RemoveAllListeners();
        retryBtn.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(false);
        dstTxt.transform.DOScale(0, 0);
        timeTxt.transform.DOScale(0, 0);
        speedTxt.transform.DOScale(0, 0);

        dstTxt.color = normalColor;
        timeTxt.color = normalColor;
        speedTxt.color = normalColor;

        StartCoroutine(Seq(dist, time));
        
    }

    IEnumerator Seq(int dist, float time) {

        if (dist > Controller.Instance.BestDistance) {
            Controller.Instance.BestDistance = dist;
            dstTxt.color = recordColor;
        }

        yield return new WaitForSeconds(.3f);
        dstTxt.text = (dist*10).ToString()+"m";
        dstTxt.transform.DOScale(1, .3f).SetEase(Ease.InQuint);
        AudioManager.Instance.PlayPop();

        yield return new WaitForSeconds(.2f);

        if (time > Controller.Instance.BestTime) {
            Controller.Instance.BestTime = (int)time;
            timeTxt.color = recordColor;

        }

        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timeTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeTxt.transform.DOScale(1, .3f).SetEase(Ease.InQuint);
        AudioManager.Instance.PlayPop();

        yield return new WaitForSeconds(.2f);
        int meters = dist * 10;
        float mPerS = meters / time;
        float kmPerH = mPerS * 3.6f;

        if (kmPerH > Controller.Instance.BestSpeed) {
            Controller.Instance.BestSpeed = (int)kmPerH;
            speedTxt.color = recordColor;
        }

        speedTxt.text = Mathf.FloorToInt(kmPerH) + "<br>Km/h";
        speedTxt.transform.DOScale(1, .3f).SetEase(Ease.InQuint);
        AudioManager.Instance.PlayPop();

        yield return new WaitForSeconds(.2f);
        retryBtn.gameObject.SetActive(true);
        retryBtn.onClick.AddListener(() => Retry());
        yield return new WaitForSeconds(.3f);
        backBtn.gameObject.SetActive(true);
        backBtn.onClick.AddListener(() => BackToTitleScreen());

    }

    void BackToTitleScreen() {
        AudioManager.Instance.PlayTap();
        transform.DOScale(0, 0.3f).OnComplete(() => {
            gameObject.SetActive(false);
            Controller.Instance.SetupTitleScreen();
        });
        Controller.Instance.uiManager.HideBlackOut();
    }

    void Retry() {
        AudioManager.Instance.PlayTap();
        transform.DOScale(0, 0.3f).OnComplete(()=> {
            gameObject.SetActive(false);
            Controller.Instance.RetryScene();
            });
        Controller.Instance.uiManager.HideBlackOut();
    }
}
