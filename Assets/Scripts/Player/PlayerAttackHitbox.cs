using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAttackHitbox : MonoBehaviour
{
    public float offsetDistance = 1f;
    public float hitRadius = 1.0f;
    public LayerMask hitLayer;

    private Rigidbody2D rb;
    private playerController pc;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<playerController>();
    }

    public void DoAttackHit()
    {

        if (pc == null) return;

        Vector2 hitPos = rb.position + pc.lastMotionVector.normalized * offsetDistance;

        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPos, hitRadius);



        foreach (var h in hits)
        {
            var sk = h.GetComponentInParent<SkeletonAI>();
            if (sk != null)
            {
                Vector2 fromDir = (h.transform.position - transform.position).normalized;
                sk.TakeHit(fromDir);
                return;
            }

            var an = h.GetComponentInParent<Animal>();
            if (an != null)
            {
                an.OnAttacked();
                return;
            }

            var goblin = h.GetComponentInParent<Goblin>();
            if (goblin != null)
            {
                goblin.OnAttacked();
                return;
            }

        }
    }

    private void OnDrawGizmosSelected()
    {
        if (pc == null) return;
        Gizmos.color = Color.cyan;
        Vector2 hitPos = (Vector2)transform.position + pc.lastMotionVector.normalized * offsetDistance;
        Gizmos.DrawWireSphere(hitPos, hitRadius);
    }
}
