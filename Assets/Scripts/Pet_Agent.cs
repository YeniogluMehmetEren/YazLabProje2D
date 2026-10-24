using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Pet_Agent : Agent
{
    [SerializeField] GameObject mainCharacter;
    [SerializeField] GameObject boss;
    [SerializeField] Animator animator;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float jump = 5f;

    //[SerializeField] private TextMeshProUGUI textEpisode;
    //[SerializeField] private TextMeshProUGUI textReward;

    [SerializeField] public int _currentEpisode = 0;
    [HideInInspector] public float _cumulativeReward = 0f;
    private bool hedefteMi = false;

    [Header("Boss Ayarlari")]
    private Boss bossScript;
    [SerializeField] bool isBossShielded;
    [SerializeField] bool isBossShieldBreakable;
    public override void Initialize()
    {
        Debug.Log("Initialize()");

        _currentEpisode = 0;
        _cumulativeReward = 0f;

        bossScript = boss.GetComponent<Boss>();
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("OnEpisodeBegin()");

        _currentEpisode++;
        _cumulativeReward = 0f;
        hedefteMi = false;

        bossScript.Shield5050();

        SetAgentPos();
    }

    private void SetAgentPos()
    {
        transform.localPosition = Vector3.zero;
        mainCharacter.transform.localPosition = new Vector3(Random.Range(-30f, 30f),1,0);
        boss.transform.localPosition = new Vector3(Random.Range(-30f, 30f),1,0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float mainCharacterPosX_normalized = mainCharacter.transform.localPosition.x / 5f;
        float mainCharacterPosY_normalized = mainCharacter.transform.localPosition.y / 5f;

        sensor.AddObservation(mainCharacterPosX_normalized);
        sensor.AddObservation(mainCharacterPosY_normalized);

        float bossPosX_normalized = boss.transform.localPosition.x / 5f;
        float bossPosY_normalized = boss.transform.localPosition.y / 5f;
        isBossShielded = bossScript.IsShielded();
        isBossShieldBreakable = bossScript.IsShieldBreakable();
        float isBossShieldBreakableF = isBossShieldBreakable ? 1.0f : 0.0f;

        sensor.AddObservation(bossPosX_normalized);
        sensor.AddObservation(bossPosY_normalized);
        sensor.AddObservation(isBossShieldBreakableF);

        float agentPosX_normalized = transform.localPosition.x / 5f;
        float agentPosY_normalized = transform.localPosition.y / 5f;
        
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

        if (hedefteMi && !isBossShieldBreakable)
        {
            AddReward(0.01f);
        }

        _cumulativeReward = GetCumulativeReward();
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var action = act[0];
        
        if (action == 0)
        {
            animator.SetBool("Run", false);
        }

        switch (action)
        {
            case 1: //Move Left
                animator.SetBool("Run", true);
                transform.position += transform.right * moveSpeed * Time.deltaTime;
                break;
            case 2: //Move Right
                animator.SetBool("Run", true);
                transform.position += -transform.right * moveSpeed * Time.deltaTime;
                break;
            case 3: //Jump
                animator.SetBool("Run", false);
                transform.position += transform.up * jump * Time.deltaTime;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Tetiklendi " + other.tag);

        if (other.gameObject.CompareTag("goal"))
        {
            hedefteMi = true;
        }
        if (other.gameObject.CompareTag("bossShield"))
        {
            if (isBossShieldBreakable)
            {
                isBossShieldBreakable = false;
                isBossShielded = false;

                animator.SetTrigger("Attack");

                bossScript.ShieldKirAiIcin();

                AddReward(4f);

                //transform.localPosition = new Vector3(mainCharacter.transform.localPosition.x + 1.5f, transform.localPosition.y, 0);
            }
            else if(isBossShielded && !isBossShieldBreakable)
            {
                AddReward(-0.2f);
            }
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
