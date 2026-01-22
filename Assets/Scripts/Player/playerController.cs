using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class playerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;

    [Header("References")]
    public Animator animator;
    public Transform body;
    public Transform hair;
    public Transform tool;

    [Header("Hurt Settings")]
    public float recoilForce = 4f;         
    public float recoilDuration = 0.3f;     
    public float invincibleTime = 0.8f;

    [Header("Collection System")]
    public int cropsCollected = 0;

    [HideInInspector] 
    public Vector2 lastMotionVector = Vector2.down; 

    private Rigidbody2D rb;
    private Vector2 inputVector;
    private bool isMoving;
    private bool isRecoiling = false;
    private bool isInvincible = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        var actions = GetComponent<PlayerActions>();
        if (actions != null && actions.IsBusy) return;


        if (isRecoiling) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        inputVector = new Vector2(h, v).normalized;
        isMoving = inputVector.magnitude > 0;

        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            animator.SetFloat("horizontal", inputVector.x);
            animator.SetFloat("vertical", inputVector.y);

            lastMotionVector = inputVector; 
            animator.SetFloat("lastHorizontal", inputVector.x);
            animator.SetFloat("lastVertical", inputVector.y);
        }

        HandleDirectionalVisuals(inputVector);
    }

    void FixedUpdate()
    {
        var actions = GetComponent<PlayerActions>();
        if (actions != null && actions.IsBusy) return;

        if (isRecoiling)
            return; 

        rb.velocity = inputVector * speed;
    }

    private void HandleDirectionalVisuals(Vector2 dir)
    {
        if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);

        if (body) body.localRotation = Quaternion.Euler(0, 0, 0);
        if (hair) hair.localRotation = Quaternion.Euler(0, 0, 0);
        if (tool) tool.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void TakeDamage(Vector2 fromDirection)
    {
        if (isInvincible) return; 

        Debug.Log("Player is hurt");
        animator.SetTrigger("Hurt");

        StartCoroutine(ApplyRecoil(fromDirection));
    }

    private IEnumerator ApplyRecoil(Vector2 fromDirection)
    {
        isRecoiling = true;
        isInvincible = true;

        rb.velocity = Vector2.zero; 
        rb.AddForce(fromDirection * recoilForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(recoilDuration);

        isRecoiling = false;

        yield return new WaitForSeconds(invincibleTime - recoilDuration);
        isInvincible = false;
    }

    public void CollectCrop()
    {
        cropsCollected++;
        Debug.Log($"Crop harvested: {cropsCollected}");
        DebugMessageUI.Instance.ShowMessage($"Crop harvested: {cropsCollected}");
    }
}

