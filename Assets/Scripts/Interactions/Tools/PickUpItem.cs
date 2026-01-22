using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public string cropId = "";
    private Crop sourceCrop;

    public void SetSourceCrop(Crop crop)
    {
        sourceCrop = crop;
    }

    public void SetCropId(string id)
    {
        cropId = id;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        CropInventory.Instance?.AddCrop(cropId);

        if (sourceCrop != null)
            sourceCrop.StartRespawn();

        Destroy(gameObject);
    }
}
