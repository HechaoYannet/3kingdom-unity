using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 入口
/// </summary>
public class GamePlaying : MonoBehaviour
{
    public static GamePlaying instance;
    public Transform PlayTrans;

    public GameState gameState;
    private GameResult gameResult;

    /// <summary>
    /// 游戏结束时触发。参数为胜负结果。
    /// </summary>
    public System.Action<GameResult> OnGameResult;

    //状态控制
    public GameState GameState
    {
        get => gameState;
        set
        {
            gameState = value;
            switch (gameState)
            {
                case GameState.Start:
                    // 进入游戏
                    RoundManager.instance.EnterGamer();
                    GameState = GameState.Gameing;
                    break;
                case GameState.Gameing:

                    break;
                case GameState.Over:
                    Debug.Log("游戏结束");
                    OnGameResult?.Invoke(gameResult);
                    break;
            }
        }
    }
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameState = GameState.Start;
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    public void GameOver(GameResult gameResult=GameResult.Defeat)
    {
        this.gameResult = gameResult;
        GameState = GameState.Over;
    }
}
public enum GameState
{
    // 开始
    Start,
    // 游戏中
    Gameing,
    // 结束
    Over
}

public enum GameResult
{
    Win,
    Defeat
}