using UnityEngine;

public class EmotePopup : MonoBehaviour
{
    public GameObject heartPrefab;
    public GameObject heartbreakPrefab;

    public Vector3 offset = new Vector3(0, 1.2f, 0);
    public float showTime = 2f;

    [Header("Size Control")]
    public float emoteScale = 0.25f;

    private GameObject current;

    public void ShowHeart()
    {
        Show(heartPrefab);
    }

    public void ShowHeartbreak()
    {
        Show(heartbreakPrefab);
    }

    private void Show(GameObject prefab)
    {
        if (prefab == null) return;

        if (current != null) Destroy(current);

        current = Instantiate(prefab, transform);
        current.transform.localPosition = offset;
        current.transform.localRotation = Quaternion.identity;
        current.transform.localScale = Vector3.one * emoteScale;

        SpriteRenderer sr = current.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingLayerName = "UI";
            sr.sortingOrder = 999;
        }

        Destroy(current, showTime);
    }
}
