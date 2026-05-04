using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Wandering : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    [Header("Animation")]
    public string verticalParam = "Vert";
    public string stateParam = "State";
    [Range(0f, 1f)] public float runThreshold = 0.75f;
    public float animationDamp = 8f;

    private NavMeshAgent agent;
    private Animator animator;
    private float timer;
    private float verticalFlow;
    private float stateFlow;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = wanderTimer;
    }

    private void Update()
    {
        if (agent == null)
        {
            return;
        }

        if (!agent.enabled || !agent.isOnNavMesh)
        {
            UpdateAnimation(Time.deltaTime);
            return;
        }

        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            agent.SetDestination(newPos);
            timer = 0;
        }

        UpdateAnimation(Time.deltaTime);
    }

    private void UpdateAnimation(float deltaTime)
    {
        float speed = agent.velocity.magnitude;
        float normalizedSpeed = (agent.speed > Mathf.Epsilon) ? Mathf.Clamp01(speed / agent.speed) : 0f;

        // 0 = idle, 0.5 = walk, 1 = run (blended in animator with State float).
        float targetState = 0f;
        if (normalizedSpeed > 0.05f)
        {
            targetState = normalizedSpeed >= runThreshold ? 1f : 0.5f;
        }

        float damp = Mathf.Max(0f, animationDamp) * deltaTime;
        verticalFlow = Mathf.Lerp(verticalFlow, normalizedSpeed, damp);
        stateFlow = Mathf.Lerp(stateFlow, targetState, damp);

        animator.SetFloat(verticalParam, verticalFlow);
        animator.SetFloat(stateParam, stateFlow);
    }

    private Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        if (!NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, distance, NavMesh.AllAreas))
        {
            return origin;
        }

        return navHit.position;
    }
}