using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ItemAwardSO :ScriptableObject
{
    public List<AwardInfo> awards;
}



[System.Serializable]
public class AwardInfo
{
    public int awardId;
    public string awardName;
    public Sprite awardSprite;
    public ItemType awardType;
    
}

public enum ItemType
{
    WHEEL,
    CHASSIS
}
