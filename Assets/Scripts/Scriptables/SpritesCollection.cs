using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteCollection", menuName = "ScriptableObjects/SpriteCollection")]
public class SpritesCollection : ScriptableObject
{
    [SerializeField] List<Sprite> sprites;
    int ind = 0;
    public Sprite GetSprite() {
        ind++;
        if (ind == sprites.Count) ind = 0;
        return sprites[ind];
    }
}
