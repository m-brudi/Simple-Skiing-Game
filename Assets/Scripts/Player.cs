using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Player : MonoBehaviour
{
    public Stop currentStop;
    [SerializeField] SpriteRenderer mySprite;
    [SerializeField] ParticleSystem deathPart;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite goSprite;
    [SerializeField] GameObject snowmanHead;
    [SerializeField] TrailRenderer trail1;
    [SerializeField] TrailRenderer trail2;
    bool readyToMove;
    public bool snowMan;
    public void NewStart() {
        trail1.enabled = false;
        trail2.enabled = false;
        currentStop = Controller.Instance.mapGenerator.firstStop;
        transform.position = currentStop.transform.position;
        mySprite.enabled = true;
        readyToMove = true;
        trail1.enabled = true;
        trail2.enabled = true;
        StopSnowman();
        snowMan = false;
    }

    void Update(){
        if (Controller.Instance.CanMove) {

            if (Input.GetMouseButtonDown(0) && readyToMove) {
                var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
                if(Input.mousePosition.x < playerScreenPoint.x) {
                    AudioManager.Instance.PlayMove();
                    mySprite.transform.DOKill();
                    mySprite.sprite = normalSprite;
                    mySprite.transform.DOLocalRotate(new(0, 0, -15), 0.1f);
                    transform.DOKill();
                    readyToMove = false;
                    currentStop = currentStop.leftStop;
                    StartCoroutine(Move(currentStop));
                } else {
                    AudioManager.Instance.PlayMove();
                    mySprite.transform.DOKill();
                    mySprite.sprite = normalSprite;
                    mySprite.transform.DOLocalRotate(new(0, 0, 15), 0.1f);
                    transform.DOKill();
                    readyToMove = false;
                    currentStop = currentStop.rightStop;
                    StartCoroutine(Move(currentStop));
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && readyToMove) {
                AudioManager.Instance.PlayMove();
                mySprite.transform.DOKill();
                mySprite.sprite = normalSprite;
                mySprite.transform.DOLocalRotate(new(0, 0, -15), 0.1f);
                transform.DOKill();
                readyToMove = false;
                currentStop = currentStop.leftStop;
                StartCoroutine(Move(currentStop));
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) && readyToMove) {
                AudioManager.Instance.PlayMove();
                mySprite.transform.DOKill();
                mySprite.sprite = normalSprite;
                mySprite.transform.DOLocalRotate(new(0, 0, 15), 0.1f);
                transform.DOKill();
                readyToMove = false;
                currentStop = currentStop.rightStop;
                StartCoroutine(Move(currentStop));
            }
        }
    }

    public void Death() {
        readyToMove = false;
        deathPart.Play();
        mySprite.enabled = false;
    }

    public void PlayDeathPart() {
        deathPart.Play();
    }

    IEnumerator Move(Stop target) {
        mySprite.sprite = goSprite;
        transform.DOMove(target.transform.position, .6f).SetEase(Ease.OutQuart);
        Controller.Instance.MoveAvalancheFarther();
        yield return new WaitForSeconds(.2f);
        Controller.Instance.HowManyStops++;
        target.PlayerOnMe();
        readyToMove = true;
        yield return new WaitForSeconds(.1f);
        mySprite.transform.DOLocalRotate(new(0, 0, 0), 0.1f);
        mySprite.sprite = normalSprite;
    }

    public void StartSnowman() {
        snowmanHead.SetActive(true);
        snowMan = true;
    }

    public void StopSnowman() {
        Controller.Instance.SnowmanStops = 0;
        snowmanHead.SetActive(false);
        snowMan = false;
    }
}
