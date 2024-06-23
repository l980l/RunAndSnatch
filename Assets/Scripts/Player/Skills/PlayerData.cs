using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Miya,
    Bambi,
    Leo,
    Cosmo,
    Chrono,
    Misty,
    Max
}

[CreateAssetMenu(menuName = "ScriptableObject/PlayerData")] 
public class PlayerData : ScriptableObject
{
    public CharacterType characterType;
    public int maxHP;
    public float maxStamina;
    public float staminaRegenSpeed;
    public float speed;
    public float dodgeSpeed;
    public SkillEffect skill;
    public Sprite portraitImage;
}
