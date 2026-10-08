using UnityEngine;

public class AgentUI : MonoBehaviour
{
    [SerializeField] private Pet_Agent agent;

    private GUIStyle defaultStyle = new GUIStyle();
    private GUIStyle positiveStyle = new GUIStyle();
    private GUIStyle negativeStyle = new GUIStyle();

    private void Start()
    {
        defaultStyle.fontSize = 36;
        defaultStyle.normal.textColor = Color.white;

        positiveStyle.fontSize = 36;
        positiveStyle.normal.textColor = Color.green;

        negativeStyle.fontSize = 36;
        negativeStyle.normal.textColor = Color.red;
    }

    private void OnGUI()
    {
        string debugEpisode = "Episode: " + agent._currentEpisode + "   Step: " + agent.StepCount;
        string debugReward = "Reward: " + agent.GetCumulativeReward().ToString();

        GUIStyle rewardStyle = agent.GetCumulativeReward() < 0 ? negativeStyle : positiveStyle;

        GUI.Label(new Rect(20, 20, 500, 30), debugEpisode, defaultStyle);
        GUI.Label(new Rect(20, 60, 500, 30), debugReward, rewardStyle);
    }
}
