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

    //[SerializeField] private TextMeshProUGUI textEpisode;
    //[SerializeField] private TextMeshProUGUI textReward;

    [SerializeField] public int _currentEpisode = 0;
    [HideInInspector] public float _cumulativeReward = 0f;
    private bool hedefteMi = false;
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
        hedefteMi = false;

        SetAgentPos();
    }

    private void SetAgentPos()
    {
        transform.localPosition = Vector3.zero;
        mainCharacter.transform.localPosition = new Vector3(Random.Range(-30f, 30f),0,0);
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

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        discreteActionsOut[0] = 0;

        if(Input.GetKey(KeyCode.L))
        {
            discreteActionsOut[0] = 1;
        }
        else if(Input.GetKey(KeyCode.J))
        {
            discreteActionsOut[0] = 2;
        }
        else if(Input.GetKey(KeyCode.K))
        {
            discreteActionsOut[0] = 3;  
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveAgent(actions.DiscreteActions);

        AddReward(-2f / MaxStep);

        if (hedefteMi)
        {
            AddReward(0.003f);
        }

        _cumulativeReward = GetCumulativeReward();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("goal"))
        {
            hedefteMi = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("goal"))
        {
            hedefteMi = false;
        }
    }
}
