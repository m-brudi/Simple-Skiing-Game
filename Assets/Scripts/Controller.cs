using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Controller : SingletonMonoBehaviour<Controller>
{
    [SerializeField] SaveSystem saveSystem;
    public MapGenerator mapGenerator;
    public UIManager uiManager;
    public Player player;
    public MasterSpritesCollection spritesCollection;
    [SerializeField] Transform avalanche;

    [SerializeField] int howManyStops = 0;
    [Space]
    public float avalancheSpeed;
    public float startAvalancheSpeed;

    [Space]
    [SerializeField] GameObject titleBackground;
    

    float bottomOfScreen;
    int coins;
    bool canMove;
    float time;
    bool timerIsRunning;
    GameData gameData;
    


    public delegate void PlayerMoved(int row);
    public static event PlayerMoved OnPlayerMoved;

    public int HowManyStops {
        get {
            return howManyStops;
        }
        set {
            howManyStops = value;
            avalancheSpeed = startAvalancheSpeed + ((float)value / 70);
            uiManager.UpdateCounters(value,Coins);
            OnPlayerMoved?.Invoke(value);
            mapGenerator.MakeAnotherRow();
            if (value == 1) StartTimer();
            if (value == 2) uiManager.HideArrows();
        }
    }

    public int Coins {
        get {
            return coins;
        }
        set {
            coins = value;
            uiManager.UpdateCounters(HowManyStops,value);
        }
    }
    public bool CanMove {
        get {
            return canMove;
        }
        set {
            canMove = value;
        }
    }

    public float BottomOfScreen {
        get {
            return player.transform.position.y;
        }
    }

    public Stop CurrentPlayerStop {
        get {
            return player.currentStop;
        }
    }

    public bool SFX {
        get {
            return gameData.sfx;
        }
        set {
            gameData.sfx = value;
            Save();
        }
    }
    public bool Music {
        get {
            return gameData.music;
        }
        set {
            gameData.music = value;
            if (value) AudioManager.Instance.PlayTheme();
            else AudioManager.Instance.StopTheme();
            Save();
        }
    }

    public int BestDistance {
        get {
            return gameData.bestDistance;
        }
        set {
            gameData.bestDistance = value;
            Save();
        }
    }
    public int BestTime {
        get {
            return gameData.bestTime;
        }
        set {
            gameData.bestTime = value;
            Save();
        }
    }
    public int BestSpeed {
        get {
            return gameData.bestSpeed;
        }
        set {
            gameData.bestSpeed = value;
            Save();
        }
    }

    void Save() {
        saveSystem.Save(gameData);
    }

    public void ResetSave() {
        saveSystem.ResetSave();
        gameData = saveSystem.Load();

    }
    private void Awake() {
        gameData = saveSystem.Load();
    }

    private void Start() {
        SetupTitleScreen();
        AudioManager.Instance.PlayTheme();
    }

    public void SetupTitleScreen() {
        player.NewStart();
        titleBackground.SetActive(true);
        uiManager.ShowTitleScreen();
        avalanche.gameObject.SetActive(false);
    }

    public void StartNewGame() {
        uiManager.HideTitleScreen();
        titleBackground.SetActive(false);
        player.NewStart();
        avalanche.localPosition = new Vector3(0, 6, 0);
        mapGenerator.StartNewMap();
        avalanche.gameObject.SetActive(true);
        howManyStops = 0;
        canMove = true;
        uiManager.ShowArrows();
        //StartTimer();
    }

    public void PlayerDied(bool avalanche) {
        uiManager.HideArrows();
        timerIsRunning = false;
        canMove = false;
        player.Death();
        StartCoroutine(DeathDelay(avalanche));
    }
    public void MoveAvalancheCloser() {
        avalanche.DOKill();
        avalanche.DOLocalMoveY(avalanche.localPosition.y - 1.5f, 0.2f);
    }

    public void MoveAvalancheFarther() {
        avalanche.DOKill();
        if(avalanche.localPosition.y <6) avalanche.DOLocalMoveY(avalanche.localPosition.y +1f, 0.2f);

        if (avalanche.localPosition.y > 6) avalanche.localPosition = new Vector3(0, 6, 0);
    }

    public void StartTimer() {
        time = 0;
        timerIsRunning = true;
    }

    public void UnPauseTimer() {
        timerIsRunning = true;
    }

    public void StopTimer() {
        timerIsRunning = false;
    }

    void Update() {
        if (canMove) {
            avalanche.localPosition += -Vector3.up * avalancheSpeed * Time.deltaTime;
            if (avalanche.localPosition.y <= 0) {
                PlayerDied(true);
                AudioManager.Instance.PlayAvalanche();
            }
           
        }
        if (timerIsRunning) {
            time += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
    IEnumerator DeathDelay(bool avalanche) {
        yield return new WaitForSeconds(avalanche? .5f:2);
        mapGenerator.CleanMap();

        uiManager.ShowSummeryPanel(howManyStops, time);
    }

    public void RetryScene() {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartNewGame();
    }
}
