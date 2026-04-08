using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;

    private List<Card> cards = new List<Card>();

    public EnemyAI enemy;//启用AI（改）

    public GRoundState RoundState { get; private set; }
    string path = "Prefabs/cardItem";//改

    private void Awake()
    {
        instance = this;
        enemy = new EnemyAI(true);
    }

    public void EnterGamer()
    {

        // 清除可能存在的脏数据--协程
        StopAllCoroutines();
        //Player.Instance.StopAllCoroutines();
        //Player.Instance.RemoveAllCard();

        enemy.RemoveAllCard();

        //抽牌
        cards = CardManager.instance.Shuffle(10);//改
        // 开启检测游戏结束的协程
        StartCoroutine(CheckGameOver());
        // 进入游戏主刷新
        StartCoroutine(UpdateRound());

    }

    /// <summary>
    /// 大回合循环
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateRound()
    {
        //起始手牌
        yield return StartCoroutine(CardManager.instance.DealCards());
        // 只要一方没输，就一直循环
        while (true)
        {
            foreach (Player currentSubPlayer in PlayerManager.Instance.players)
            {
                //大回合开始
                currentSubPlayer.IsSubRound = true;
                //准备
                RoundState = GRoundState.Preparing;
                yield return StartCoroutine(RoleManager.Instance.EnterPreparing());
                //判定
                RoundState = GRoundState.Checking;
                yield return StartCoroutine(currentSubPlayer.EnterChecking());
                //摸牌
                RoundState = GRoundState.GettingCard;
                yield return StartCoroutine(CardManager.instance.DealCards(currentSubPlayer));//发牌
                //出牌
                RoundState = GRoundState.Battling;
                yield return StartCoroutine(currentSubPlayer.EnterRound());
                //弃牌
                RoundState = GRoundState.RefusingCard;
                yield return StartCoroutine(currentSubPlayer.EnterRefusing());
                //结束
                RoundState = GRoundState.Ending;
                yield return StartCoroutine(currentSubPlayer.EnterEnding());
                //大回合结束
                currentSubPlayer.IsSubRound = false;
            }
        }
    }

    IEnumerator CheckGameOver()
    {
        while (true)
        {
            yield return null;
            if (GamePlaying.instance.gameState == GameState.Over)
            {
                // 关掉UpdateRound
                StopAllCoroutines();
                StopCoroutine(UpdateRound());
                StopCoroutine(CheckGameOver());
            }
        }
    }
}
public enum GRoundState
{
    Preparing, Checking, GettingCard, Battling, RefusingCard, Ending
}
/*
 * 加载Assets目录下任何一个位置的资源，但只能在编辑器模式下；
 * 在发布环境下使用assetbundle方式加载资源
//// 通过Resources.LoadAtPath方法动态加载Prefab
//private void LoadPrefabByResources()
//{
//    Object prefab = Resources.LoadAssetAtPath("Assets/Prefabs/Cube.prefab", typeof(GameObject));
//    GameObject cube = (GameObject)Instantiate(prefab);
//    cube.transform.parent = transform;
//}

*/