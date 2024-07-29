using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AccountData
{
    public CharacterType selectedCharacter;
    public ItemType[] Items;
    public int gold;
    public LanguageType language;
    public bool[] PlayableCharacter;
    public int[] CharacterGifts;
    public int exitStreak;
    public int totalExitCount;
    public bool inDungeon;
    public float volumeBGM;
    public float volumeSFX;
    public int frameRateAnd;
    public int winResolutionIndex;
    public bool isFullscreen;

    public AccountData()
    {
        selectedCharacter = CharacterType.Miya;
        language = LanguageType.En;
        PlayableCharacter = new bool[(int)CharacterType.Max];
        PlayableCharacter[0] = true;
        CharacterGifts = new int[(int)CharacterType.Max];
        volumeBGM = 0.5f;
        volumeSFX = 0.5f;
        frameRateAnd = 1;
        winResolutionIndex = -1;
        isFullscreen = true;
    }
}
