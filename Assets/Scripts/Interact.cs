using UnityEngine;
using UnityEngine.AI;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Transform farmPoint;
    private bool isCaptured = false;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Interact()
    {
        if (isCaptured)
        {
            return;
        }

        if (farmPoint == null)
        {
            Debug.LogWarning($"{name}: Farm point is not assigned.");
            return;
        }

        isCaptured = true;
        if (agent != null && agent.enabled)
        {
            // Warp keeps NavMeshAgent valid and lets wandering continue from farm.
            if (!agent.Warp(farmPoint.position))
            {
                transform.position = farmPoint.position;
            }
        }
        else
        {
            transform.position = farmPoint.position;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AnimalCaptured();
        }
        else
        {
            Debug.LogWarning("Interactable: GameManager.Instance is missing.");
        }
    }
}
