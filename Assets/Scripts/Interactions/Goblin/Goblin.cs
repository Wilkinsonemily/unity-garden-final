using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class Goblin : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.0f;
    public float moveTime = 2f;
    public float idleTime = 2f;
    public float areaLimit = 2f;

    [Header("Player Detect")]
    public float voiceRange = 1.8f;

    
    public AudioClip nearVoice;
    [Header("Voice Clips")]
    public AudioClip idleVoice;
    public AudioClip attackedVoice;

    [Header("Voice Timing")]
    public float idleVoiceInterval = 4f;
    public float attackedCooldown = 0.25f;
    public float nearCooldown = 1.0f;

    [Header("Voice Settings")]
    [Range(0f, 1f)] public float volume = 0.5f;

    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;
    private AudioSource audioSrc;

    private Vector2 startPos;
    private Vector2 moveDir;
    private bool isMoving;

    private float idleTimer = 0f;
    private bool playerNear = false;

    private float lastAttackedTime = -999f;
    private float lastNearTime = -999f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        rb.gravityScale = 0;
        rb.freezeRotation = true;

        audioSrc.playOnAwake = false;
        audioSrc.loop = false;
        audioSrc.spatialBlend = 0f;
        audioSrc.volume = volume;

        startPos = transform.position;

        StartCoroutine(RandomMovement());
    }

    void Update()
    {
        if (isMoving)
        {
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
        }

        UpdateNearState();

        HandleIdleVoice();
    }

    IEnumerator RandomMovement()
    {
        while (true)
        {
            isMoving = false;
            anim?.SetBool("isMoving", false);
            yield return new WaitForSeconds(Random.Range(idleTime * 0.5f, idleTime * 1.5f));

            isMoving = true;
            moveDir = Random.insideUnitCircle.normalized;
            anim?.SetBool("isMoving", true);

            float t = Random.Range(moveTime * 0.7f, moveTime * 1.2f);
            while (t > 0)
            {
                t -= Time.deltaTime;

                if (Vector2.Distance(transform.position, startPos) > areaLimit)
                    moveDir = (startPos - (Vector2)transform.position).normalized;

                yield return null;
            }
        }
    }

    void UpdateNearState()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        bool nowNear = dist <= voiceRange;

        if (nowNear && !playerNear)
        {
            playerNear = true;
            idleTimer = 0f;

            if (Time.time - lastNearTime >= nearCooldown)
            {
                lastNearTime = Time.time;
                PlayVoice(nearVoice != null ? nearVoice : idleVoice, Random.Range(1.15f, 1.35f), force: true);
            }
        }
        else if (!nowNear && playerNear)
        {
            playerNear = false;
            idleTimer = 0f;
        }
    }

    void HandleIdleVoice()
    {
        if (!playerNear) return;
        if (idleVoice == null) return;

        idleTimer += Time.deltaTime;
        if (idleTimer >= idleVoiceInterval)
        {
            idleTimer = 0f;
            PlayVoice(idleVoice, Random.Range(1.05f, 1.25f), force: false);
        }
    }

    public void OnAttacked()
    {
        if (Time.time - lastAttackedTime < attackedCooldown) return;
        lastAttackedTime = Time.time;

        PlayVoice(attackedVoice != null ? attackedVoice : idleVoice, 0.9f, force: true);
    }

    void PlayVoice(AudioClip clip, float pitch, bool force)
    {
        if (clip == null) return;

        audioSrc.volume = volume;
        audioSrc.pitch = pitch;

        if (force)
        {
            audioSrc.Stop();
            audioSrc.clip = clip;
            audioSrc.Play();
            return;
        }

        if (audioSrc.isPlaying) return;

        audioSrc.clip = clip;
        audioSrc.Play();
    }
}
