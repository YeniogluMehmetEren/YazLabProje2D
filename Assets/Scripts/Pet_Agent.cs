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

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D agentCollider;
    [SerializeField] private LayerMask yerLayer;

    [Header("Bölüm Kontrolü İçin")]
    [SerializeField] public int _currentEpisode = 0;
    [SerializeField] public float _cumulativeReward = 0f;
    [SerializeField]  private bool hedefteMi = false;

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
        transform.localPosition = new Vector3(Random.Range(-152f, -142), 1.8f, 0);
        mainCharacter.transform.localPosition = new Vector3(Random.Range(-187f, -170f), 3.5f, 0);
        boss.transform.localPosition = new Vector3(Random.Range(-135f, -105f), 3.5f, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        /*if(mainCharacter!=null && boss!=null)
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
           
        }*/

        if (mainCharacter != null && boss != null)
        {
            Vector3 oyuncuyaUzaklik = mainCharacter.transform.position - transform.position;
            Vector3 bossaUzaklik = boss.transform.position - transform.position;

            sensor.AddObservation(Mathf.Clamp(oyuncuyaUzaklik.x / 50f, -1f, 1f));
            sensor.AddObservation(Mathf.Clamp(oyuncuyaUzaklik.y / 50f, -1f, 1f));

            sensor.AddObservation(Mathf.Clamp(bossaUzaklik.x / 50f, -1f, 1f));
            sensor.AddObservation(Mathf.Clamp(bossaUzaklik.y / 50f, -1f, 1f));

            isBossShieldBreakable = bossScript.IsShieldBreakable();
            sensor.AddObservation(isBossShieldBreakable ? 1.0f : 0.0f);
        }
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

        /*if (hedefteMi)
        {
            if (!isBossShieldBreakable)
            {
                AddReward(0.1f);
            }
            else
            {
                AddReward(-0.02f);
            }

        }*/

        _cumulativeReward = GetCumulativeReward();
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var action = act[0];
        
        if (action == 0)
        {
            animator.SetBool("Run", false);
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        switch (action)
        {
            case 1: //Move Left
                animator.SetBool("Run", true);
                //transform.position += transform.right * moveSpeed * Time.deltaTime;
                rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
                break;
            case 2: //Move Right
                animator.SetBool("Run", true);
                //transform.position += -transform.right * moveSpeed * Time.deltaTime;
                rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
                break;
            case 3: //Jump
                animator.SetBool("Run", false);
                transform.position += transform.up * jump * Time.deltaTime;
                //transform.position += transform.up * jump * Time.deltaTime;
                if (agentCollider.IsTouchingLayers(yerLayer))
                {
                    rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Tetiklendi " + other.tag);

        if (other.gameObject.CompareTag("goal"))
        {
            //hedefteMi = true;

            if (!isBossShieldBreakable)
            {
                AddReward(2f);
                EndEpisode();
            }
        }
        if (other.gameObject.CompareTag("bossShield"))
        {
            if (isBossShieldBreakable)
            {
                animator.SetTrigger("Attack");

                isBossShieldBreakable = false;
                isBossShielded = false;

                bossScript.ShieldKirAiIcin();

                AddReward(3f);
                EndEpisode();
            }
            else
            {
                AddReward(-0.5f);
                EndEpisode();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("goal"))
        {
            //hedefteMi = false;
        }
    }
}
