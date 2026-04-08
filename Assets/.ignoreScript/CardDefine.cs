using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "卡牌配置", fileName = "卡牌")]
public class CardDefine : ScriptableObject
{
    // 卡牌图片
    public Sprite cardImage;
    // 音效
    public AudioClip useAudioClip;
    public AudioClip takeEffectAudioClip;

    // 能否主动释放
    public bool IsInitiative;
    // 能否满血释放
    public bool FullHPCanInitiative;
    // 攻击力，杀
    public int Attack;
    // 恢复值
    public int Recover;

    //过牌值
    public int GetCardCount;

    // 可以被响应 我出杀对方可以出闪
    public bool CanBeResponse;

    // 别人出那些卡，我可以响应
    public List<CardDefine> RespondCards;

    //////////////////////////
    // 防御力
    public int Defense;
    // 在响应敌人的卡牌时，取消对方的功能，无懈可击
    public bool IsInvalid;


}
