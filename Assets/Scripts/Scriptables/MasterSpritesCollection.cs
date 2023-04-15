using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MasterSpriteCollection", menuName = "ScriptableObjects/MasterSpriteCollection")]
public class MasterSpritesCollection : ScriptableObject {

    [SerializeField] SpritesCollection stones;
    [SerializeField] SpritesCollection smallCosmetics;
    [SerializeField] SpritesCollection monsters;

    public Sprite GetStoneSprite() {
        return stones.GetSprite();
    }
    public Sprite GetSmallCosmeticsSprite() {
        return smallCosmetics.GetSprite();
    }
    
    public Sprite GetMonsterSprite() {
        return monsters.GetSprite();
    }
}
