// 屏幕空间UI元素接口
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// 回合指示器实现
public class TurnIndicator : MonoBehaviour, IScreenSpaceElement
{
    [Header("UI组件")]
    [SerializeField] private TextMeshPro turnText;
    [SerializeField] private Image playerIndicator;
    [SerializeField] private Image opponentIndicator;
    [SerializeField] private Slider turnTimer;

    void Start()
    {
        // 注册到屏幕空间UI管理器
        UIManager.Instance.GetScreenSpaceManager().RegisterHUDElement("turn_indicator", this);
    }

    public void UpdateElement(object data)
    {
        if (data is TurnData turnData)
        {
            UpdateTurnDisplay(turnData);
        }
    }

    private void UpdateTurnDisplay(TurnData turnData)
    {
        // 更新回合文本
        turnText.text = $"回合 {turnData.turnNumber}";

        // 更新玩家指示器
        bool isPlayerTurn = turnData.currentPlayer == "player";
        playerIndicator.color = isPlayerTurn ? Color.green : Color.gray;
        opponentIndicator.color = !isPlayerTurn ? Color.green : Color.gray;

        // 更新计时器
        if (turnTimer != null && turnData.totalTime > 0)
        {
            turnTimer.value = turnData.timeRemaining / turnData.totalTime;
        }
    }

    // 回合数据模型
    [System.Serializable]
    public class TurnData
    {
        public int turnNumber;
        public string currentPlayer;
        public float timeRemaining;
        public float totalTime;
    }
}