using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Stop : MonoBehaviour
{
    public int index;
    public Stop leftStop;
    public Stop rightStop;
    public List<Stop> parents;
    public int myRow;
    [Space]
    [SerializeField] SpriteRenderer monsterImg;
    [SerializeField] SpriteRenderer monCoinImg;
    [SerializeField] SpriteRenderer myGroundImg;
    [SerializeField] GameObject yeti;
    [SerializeField] GameObject myEmptyImg;
    [SerializeField] List<SpriteRenderer> smallCosmetics;
    [SerializeField] Sprite snowmanBody;
    [Space]
    [SerializeField] Sprite blueArrow;
    [SerializeField] Sprite redArrow;
    [SerializeField] List<SpriteRenderer> arrows;

    public bool snowman = false;
    public enum StopType {
        yeti,
        empty,
        slowDown,
        monCoins,
    }

    public StopType myType;
    bool toDelete = false;

    public bool IsParent (List<Stop> stops) {
        return parents.Any(x => stops.Any(y => y == x));
    }

    private void OnEnable() {
        Controller.OnPlayerMoved += PlayerMoved;
    }

    private void OnDisable() {
        Controller.OnPlayerMoved -= PlayerMoved;
    }
    void PlayerMoved(int playerRow) {
            if (playerRow-1 > myRow && index > 0) {
                SlowlyDelete();
            }
        if (toDelete && playerRow - 1 > myRow) Destroy(gameObject);
    }


    public void SlowlyDelete() {
        toDelete = true;
    }

    public void Setup() {
        monsterImg.gameObject.SetActive(false);
        monCoinImg.gameObject.SetActive(false);
        myGroundImg.gameObject.SetActive(false);
        myEmptyImg.SetActive(true);
        yeti.SetActive(false);

        float rand = Random.value;
        if (index > 1) {
            if (rand > 0.85f) {
                //fight - yeti
                myType = StopType.yeti;
                yeti.SetActive(true);
                myGroundImg.gameObject.SetActive(true);
                myEmptyImg.SetActive(false);

            } else if (rand > 0.65f) {
                //slowDown
                myType = StopType.slowDown;
                monsterImg.gameObject.SetActive(true);
                Sprite sp = Controller.Instance.spritesCollection.GetMonsterSprite();
                if (sp.name == "snowman") snowman = true;
                monsterImg.sprite = sp;
                myGroundImg.gameObject.SetActive(true);
                myEmptyImg.SetActive(false);

            } else {
                //empty
                myType = StopType.empty;
                monsterImg.gameObject.SetActive(false);
                int r = Random.Range(0, 2);
                foreach (var item in arrows) {
                    if (r == 0) item.sprite = blueArrow;
                    else item.sprite = redArrow;
                }
            }
        }
        if(myRow < 5) {
            ChangeFromYeti();
        }
        SetupCosmetics();
    }

    public void ChangeFromYeti() {
        float rand = Random.value;
        monsterImg.gameObject.SetActive(false);
        monCoinImg.gameObject.SetActive(false);
        myGroundImg.gameObject.SetActive(false);
        myEmptyImg.SetActive(true);
        yeti.SetActive(false);

        if (rand < 0.6f) {
            //empty
            myType = StopType.empty;
            monsterImg.gameObject.SetActive(false);
            int r = Random.Range(0, 2);
            foreach (var item in arrows) {
                if (r == 0) item.sprite = blueArrow;
                else item.sprite = redArrow;
            }
         } else {
            //slowdown
            myType = StopType.slowDown;
            monsterImg.gameObject.SetActive(true);
            Sprite sp = Controller.Instance.spritesCollection.GetMonsterSprite();
            if (sp.name == "snowman") snowman = true;
            monsterImg.sprite = sp;
            myGroundImg.gameObject.SetActive(true);
            myEmptyImg.SetActive(false);

        }
    }

    void SetupCosmetics() {
        foreach (var item in smallCosmetics) {
            float rand = Random.value;
            if (rand < 0.15f) item.sprite = Controller.Instance.spritesCollection.GetSmallCosmeticsSprite();
            else item.sprite = null;
        }
    }

    public void PlayerOnMe() {
        switch (myType) {
            case StopType.yeti:
                AudioManager.Instance.PlayYeti();
                Controller.Instance.PlayerDied(false);
                break;
            case StopType.monCoins:
                monCoinImg.gameObject.SetActive(false);
                Controller.Instance.Coins++;
                break;
            case StopType.slowDown:
                AudioManager.Instance.PlayHit();
                Controller.Instance.player.StopSnowman();
                Controller.Instance.player.PlayDeathPart();
                Controller.Instance.MoveAvalancheCloser();
                if (snowman) {
                    Controller.Instance.player.StartSnowman();
                    monsterImg.sprite = snowmanBody;
                }
                break;
            default:
                break;
        }
    }


    // ========== ELECTRICITY =========//
    /*
    public void UpdateMyPower(int minus) {
        myPower = myPower - minus;
        if(myPower <= 0) {
            myStats.SetActive(false);
            monsterImg.gameObject.SetActive(false);
            myType = StopType.empty;
        }
        myTxt.text = myPower.ToString();
        myTxt.transform.DOPunchScale(Vector3.one * .3f, 0.3f);
    }

    public void FindFightingNeighbours(int deepLvls, int dmg) {
        List<Stop> fightingNeighbours = new();
        if (deepLvls > 0) {
            if (leftStop.myType == StopType.fight) {
                fightingNeighbours.Add(leftStop);
            }
            if (rightStop.myType == StopType.fight) {
                fightingNeighbours.Add(rightStop);
            }
            StartCoroutine(DelayChainedAttack(fightingNeighbours, dmg, deepLvls));
        }
    }

    IEnumerator DelayChainedAttack(List<Stop> stops, int dmg, int deepLvls) {
        foreach (var item in stops) {
            if (item == leftStop) {
                electricPartLeft.Play();
            } else if(item == rightStop) {
                electricPartRight.Play();
            }
        }
        yield return new WaitForSeconds(0.2f);
        foreach (var item in stops) {
            if(deepLvls > 1) item.FindFightingNeighbours(deepLvls - 1, dmg);
            item.UpdateMyPower(dmg);
        }
    }
    */
    //===================//

}
