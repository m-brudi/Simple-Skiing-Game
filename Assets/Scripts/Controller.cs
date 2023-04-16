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
    [SerializeField] int snowmanStops = 0;
    [SerializeField] int bestSnowmanStops = 0;
    [Space]
    public float avalancheSpeed;
    public float startAvalancheSpeed;

    [Space]
    [SerializeField] GameObject titleBackground;
    

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
            if (player.snowMan) SnowmanStops++;
            if (value == 1) StartTimer();
            if (value == 2) uiManager.HideArrows();
        }
    }

    public int SnowmanStops {
        get {
            return snowmanStops;
        }
        set {
            if(value == 0) {
                if(snowmanStops > bestSnowmanStops) {
                    bestSnowmanStops = snowmanStops;
                }
            }
            snowmanStops = value;
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
            return Load("bestdist");
            //return gameData.bestDistance;
        }
        set {
            gameData.bestDistance = value;
            Save("bestdist", value);
            //Save();
        }
    }
    public int BestTime {
        get {
            return Load("besttime");
            //return gameData.bestTime;
        }
        set {
            gameData.bestTime = value;
            Save("besttime", value);
            //Save();
        }
    }
    public int BestSpeed {
        get {
            return Load("bestspeed");
            //return gameData.bestSpeed;
        }
        set {
            gameData.bestSpeed = value;
            Save("bestspeed", value);
            //Save();
        }
    }
    public int BestSnowman {
        get {
            return Load("bestsnowman");
        }
        set {
            Save("bestsnowman", value);
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

    public void Save(string name, int value) {
        PlayerPrefs.SetInt(name, value);
    }

    public int Load(string name) {
        if (PlayerPrefs.HasKey(name)) return PlayerPrefs.GetInt(name);
        else return 0;
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
        CanMove = true;
        uiManager.ShowArrows();
        bestSnowmanStops = 0;
        snowmanStops = 0;
        //StartTimer();
    }

    public void PlayerDied(bool avalanche) {
        uiManager.HideArrows();
        timerIsRunning = false;
        CanMove = false;
        player.Death();
        StartCoroutine(DeathDelay(avalanche));
        if (bestSnowmanStops > BestSnowman) BestSnowman = bestSnowmanStops;
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
        if (CanMove) {
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
        if (snowmanStops > bestSnowmanStops) bestSnowmanStops = snowmanStops;
        uiManager.ShowSummeryPanel(howManyStops, time, bestSnowmanStops);
    }

    public void RetryScene() {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartNewGame();
    }
}
