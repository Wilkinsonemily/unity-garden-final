using UnityEngine;

public class AnimalContactTrigger : MonoBehaviour
{
    private Animal animal;

    void Awake()
    {
        animal = GetComponentInParent<Animal>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        animal?.OnPlayerContact();
    }
}
