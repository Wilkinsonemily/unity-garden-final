using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ToolsPlayerController : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] float offsetDistance = 0.6f;
    [SerializeField] float interactRadius = 0.75f;
    [SerializeField] LayerMask interactableLayer;

    private Rigidbody2D rb;
    private playerController player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<playerController>();
    }

    public void UseToolFromEvent()
    {
        UseToolInternal();
    }

    private void UseToolInternal()
    {
        Vector2 toolPosition = rb.position + player.lastMotionVector.normalized * offsetDistance;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(toolPosition, interactRadius, interactableLayer);

        ToolHit best = null;
        float bestDist = float.MaxValue;

        foreach (Collider2D c in colliders)
        {
            ToolHit toolHit = c.GetComponent<ToolHit>();
            if (toolHit == null) continue;

            float d = Vector2.Distance(toolPosition, c.ClosestPoint(toolPosition));
            if (d < bestDist)
            {
                bestDist = d;
                best = toolHit;
            }
        }

        if (best != null)
            best.Hit();
    }

    private void OnDrawGizmosSelected()
    {
        if (rb == null || player == null) return;

        Vector2 toolPosition = rb.position + player.lastMotionVector.normalized * offsetDistance;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(toolPosition, interactRadius);
    }
}
