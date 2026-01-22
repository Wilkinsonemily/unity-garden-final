using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class SkeletonAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseRange = 5f;
    public float attackRange = 1.2f;
    public float attackInterval = 1.0f;

    [Header("HP / Respawn")]
    public int maxHp = 3;
    public float wakeUpTime = 10f;

    [Header("Recoil")]
    public float recoilForce = 4f;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;

    private int currentHp;

    private bool isAttacking;
    private bool isDead;
    private bool isDying;

    private Coroutine attackRoutine;
    private Coroutine deathRoutine;

    private Collider2D[] allColliders;
    private SpriteRenderer[] allRenderers;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        allColliders = GetComponentsInChildren<Collider2D>(true);
        allRenderers = GetComponentsInChildren<SpriteRenderer>(true);
    }

    void Start()
    {
        ResetSkeleton();
    }

    void Update()
    {
        if (player == null) return;
        if (isDead || isDying) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1.4f, 1.4f, 1.4f);
        else
            transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);

        if (!isAttacking)
        {
            if (dist <= chaseRange && dist > attackRange)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                rb.velocity = dir * moveSpeed;
                anim.SetBool("isMoving", true);
            }
            else
            {
                rb.velocity = Vector2.zero;
                anim.SetBool("isMoving", false);
            }

            if (dist <= attackRange && attackRoutine == null)
            {
                attackRoutine = StartCoroutine(AttackLoop());
            }
        }
    }

    IEnumerator AttackLoop()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;
        anim.SetBool("isMoving", false);

        while (!isDead && !isDying && player != null &&
               Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Attack");

            yield return new WaitForSeconds(0.2f);
            TryDamagePlayer();

            yield return new WaitForSeconds(attackInterval);
        }

        isAttacking = false;
        attackRoutine = null;
    }

    void TryDamagePlayer()
    {
        if (isDead || isDying) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var h in hits)
        {
            if (!h.CompareTag("Player")) continue;

            var pc = h.GetComponent<playerController>();
            if (pc != null)
            {
                Vector2 recoilDir = (h.transform.position - transform.position).normalized;
                pc.TakeDamage(recoilDir);
            }
        }
    }

    public void TakeHit(Vector2 fromDir)
    {
        if (isDead || isDying) return;

        currentHp--;
        Debug.Log($"Skeleton hit! HP: {currentHp}");

        anim.ResetTrigger("Hurt");
        anim.SetTrigger("Hurt");

        rb.velocity = Vector2.zero;
        rb.AddForce(fromDir * recoilForce, ForceMode2D.Impulse);

        if (currentHp <= 0)
        {
            isDying = true;

            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                attackRoutine = null;
            }
            isAttacking = false;

            if (deathRoutine != null)
                StopCoroutine(deathRoutine);

            deathRoutine = StartCoroutine(DieAndWakeUp());
        }
    }

    IEnumerator DieAndWakeUp()
    {
        isDead = true;
        rb.velocity = Vector2.zero;

        anim.SetBool("isDead", true);
        anim.ResetTrigger("Die");
        anim.SetTrigger("Die");

        yield return new WaitForSecondsRealtime(0.6f);

        // hide & disable collision
        SetVisible(false);
        SetCollidable(false);

        yield return new WaitForSecondsRealtime(wakeUpTime);

        ResetSkeleton();
    }

    void ResetSkeleton()
    {
        currentHp = maxHp;

        isDead = false;
        isDying = false;
        isAttacking = false;

        rb.velocity = Vector2.zero;

        anim.SetBool("isDead", false);
        anim.SetBool("isMoving", false);
        anim.ResetTrigger("Die");
        anim.ResetTrigger("Hurt");
        anim.ResetTrigger("Attack");

        SetVisible(true);
        SetCollidable(true);

        deathRoutine = null;
    }

    void SetVisible(bool v)
    {
        if (allRenderers == null) return;
        foreach (var r in allRenderers)
            if (r) r.enabled = v;
    }

    void SetCollidable(bool v)
    {
        if (allColliders == null) return;
        foreach (var c in allColliders)
            if (c) c.enabled = v;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
