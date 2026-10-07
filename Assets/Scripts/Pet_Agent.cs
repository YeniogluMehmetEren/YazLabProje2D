using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Pet_Agent : Agent
{
    [SerializeField] GameObject mainCharacter;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float jump = 5f;

    [SerializeField] private TextMeshProUGUI textEpisode;
    [SerializeField] private TextMeshProUGUI textReward;

    private int _currentEpisode = 0;
    private float _cumulativeReward = 0f;

    public override void Initialize()
    {
        Debug.Log("Initialize()");

        _currentEpisode = 0;
        _cumulativeReward = 0f;
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("OnEpisodeBegin()");

        _currentEpisode++;
        _cumulativeReward = 0f;

        SetAgentPos();
    }

    private void SetAgentPos()
    {
        transform.localPosition = Vector3.zero;
        mainCharacter.transform.localPosition = new Vector3(Random.Range(-10f, 10f),0,0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float mainCharacterPosX_normalized = mainCharacter.transform.localPosition.x / 5f;
        float mainCharacterPosY_normalized = mainCharacter.transform.localPosition.y / 5f;

        float agentPosX_normalized = transform.localPosition.x / 5f;
        float agentPosY_normalized = transform.localPosition.y / 5f;

        sensor.AddObservation(mainCharacterPosX_normalized);
        sensor.AddObservation(mainCharacterPosY_normalized);
        sensor.AddObservation(agentPosX_normalized);
        sensor.AddObservation(agentPosY_normalized);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveAgent(actions.DiscreteActions);

        AddReward(-2f / MaxStep);

        _cumulativeReward = GetCumulativeReward();

        EditTexts();
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var action = act[0];

        switch (action)
        {
            case 1: //Move Left
                transform.position += transform.right * moveSpeed * Time.deltaTime;
                break;
            case 2: //Move Right
                transform.position += -transform.right * moveSpeed * Time.deltaTime;
                break;
            case 3: //Jump
                transform.position += transform.up * jump * Time.deltaTime;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("goal"))
        {
            AddReward(1f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("goal"))
        {
            AddReward(0.1f * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("goal"))
        {
            AddReward(-0.06f);
        }
    }

    private void EditTexts()
    {
        if (_cumulativeReward<0)
        {
            textReward.color = Color.red;
        }
        textReward.color = Color.green;
        textEpisode.text = "Episode: " + _currentEpisode.ToString();
        textReward.text = "Reword : " + _cumulativeReward.ToString();
    }
}
