using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

[System.Serializable]
public class GameData {

    public float saveVersion;
    public bool music;
    public bool sfx;
    public int bestDistance;
    public int bestTime;
    public int bestSpeed;
    public int bestDistanceInSnowman;

    public System.DateTime lastSaveTime;

    public GameData() {
        music = sfx = true;
        bestDistance = bestTime = bestSpeed = bestDistanceInSnowman = 0;
}
}