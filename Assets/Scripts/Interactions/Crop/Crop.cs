using System.Collections;
using UnityEngine;

public class Crop : ToolHit
{
    [Header("Crop Type (for UI/JSON)")]
    public string cropId = "Carrot";

    [Header("Drop Settings")]
    [SerializeField] GameObject harvestedDrop;
    [SerializeField] int dropCount = 1;
    [SerializeField] float dropRadius = 0.3f;

    [Header("Respawn Settings")]
    [SerializeField] float respawnTime = 10f;

    private bool ready = true;

    private SpriteRenderer[] renderers;
    private Collider2D[] colliders;

    private Coroutine respawnRoutine;

    private void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>(true);
        colliders = GetComponentsInChildren<Collider2D>(true);
    }

    public override void Hit()
    {
        if (!ready) return;

        for (int i = 0; i < dropCount; i++)
        {
            Vector3 dropPos = transform.position;
            dropPos.x += Random.Range(-dropRadius, dropRadius);
            dropPos.y += Random.Range(-dropRadius, dropRadius);

            GameObject drop = Instantiate(harvestedDrop, dropPos, Quaternion.identity);

            PickUpItem pickup = drop.GetComponentInChildren<PickUpItem>(true);
            if (pickup != null)
            {
                pickup.SetSourceCrop(this);
                pickup.SetCropId(cropId);
            }
            else
            {
                Debug.LogWarning("Drop prefab missing PickUpItem: " + harvestedDrop.name);
            }
        }

        ready = false;
        SetVisible(false);
    }

    private void SetVisible(bool v)
    {
        if (renderers != null)
            foreach (var r in renderers) if (r) r.enabled = v;

        if (colliders != null)
            foreach (var c in colliders) if (c) c.enabled = v;
    }

    public void StartRespawn()
    {
        if (respawnRoutine != null) StopCoroutine(respawnRoutine);
        respawnRoutine = StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnTime);

        ready = true;
        SetVisible(true);
        respawnRoutine = null;
    }
}
