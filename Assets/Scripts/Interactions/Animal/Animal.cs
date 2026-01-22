using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Animal : MonoBehaviour
{
    [Header("Animal Type")]
    public string animalType = "Cow";

    [Header("Movement")]
    public float moveSpeed = 1.2f;
    public float moveTime = 2f;
    public float idleTime = 2f;
    public float areaLimit = 2.5f;

    [Header("Voice")]
    public AudioClip voiceClip;
    public float voiceInterval = 3f;
    public float voiceRandom = 1.5f;
    public float voiceRange = 2.0f; 
    [Range(0f, 1f)] public float voiceVolumeMul = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private EmotePopup emote;
    private Transform player;

    private Vector2 startPos;
    private Vector2 moveDir = Vector2.zero;
    private bool isMoving;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        emote = GetComponent<EmotePopup>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        startPos = rb.position;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.drag = 8f; 

        StartCoroutine(RandomMovement());
        StartCoroutine(VoiceLoop());
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.velocity = moveDir * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        float dist = Vector2.Distance(rb.position, startPos);
        if (dist > areaLimit)
        {
            Vector2 back = (startPos - rb.position).normalized;
            moveDir = back;
        }
    }

    private IEnumerator RandomMovement()
    {
        while (true)
        {
            isMoving = false;
            animator?.SetBool("isMoving", false);
            yield return new WaitForSeconds(Random.Range(idleTime * 0.5f, idleTime * 1.5f));

            isMoving = true;
            moveDir = Random.insideUnitCircle.normalized;
            animator?.SetBool("isMoving", true);

            float timer = Random.Range(moveTime * 0.7f, moveTime * 1.2f);
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator VoiceLoop()
    {
        while (true)
        {
            float wait = voiceInterval + Random.Range(-voiceRandom, voiceRandom);
            if (wait < 0.5f) wait = 0.5f;
            yield return new WaitForSeconds(wait);

            if (player == null) continue;

            float d = Vector2.Distance(transform.position, player.position);
            if (d > voiceRange) continue; 

            if (voiceClip != null && AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(voiceClip, voiceVolumeMul);
            else
                DebugMessageUI.Instance?.ShowMessage(GetAnimalSoundText(animalType));
        }
    }

    public void OnPlayerContact()
    {
        emote?.ShowHeart();
    }

    public void OnAttacked()
    {
        emote?.ShowHeartbreak();
    }

    private string GetAnimalSoundText(string type)
    {
        switch (type.ToLower())
        {
            case "cow": return "MOO";
            case "pig": return "OINK";
            case "sheep": return "BAA";
            case "duck": return "QUACK";
            case "chicken": return "CLUCK";
            default: return "???";
        }
    }
}
