using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private float interactRadius = 1.5f;
    [SerializeField] private LayerMask interactMask = ~0;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform interactOrigin;

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Vector3 origin = interactOrigin != null ? interactOrigin.position : transform.position;

        // First, try camera raycast (look-at interaction).
        if (playerCamera != null)
        {
            Vector3 rayOrigin = playerCamera.transform.position;
            Vector3 direction = playerCamera.transform.forward;

            if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, interactDistance, interactMask, QueryTriggerInteraction.Collide))
            {
                Interactable rayTarget = hit.collider.GetComponentInParent<Interactable>();
                if (rayTarget != null)
                {
                    rayTarget.Interact();
                    return;
                }
            }
        }

        // Fallback: interact with the closest animal near player.
        Collider[] nearby = Physics.OverlapSphere(origin, interactRadius, interactMask, QueryTriggerInteraction.Collide);
        Interactable closest = null;
        float closestDist = float.MaxValue;

        for (int i = 0; i < nearby.Length; i++)
        {
            Interactable candidate = nearby[i].GetComponentInParent<Interactable>();
            if (candidate == null)
            {
                continue;
            }

            float dist = Vector3.Distance(origin, candidate.transform.position);
            if (dist < closestDist)
            {
                closest = candidate;
                closestDist = dist;
            }
        }

        if (closest != null)
        {
            closest.Interact();
            return;
        }

        Debug.Log("No interactable animal found in front or nearby.");
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = interactOrigin != null ? interactOrigin.position : transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, interactRadius);
    }
}
