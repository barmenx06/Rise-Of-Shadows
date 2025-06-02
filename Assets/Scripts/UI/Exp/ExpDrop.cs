// ExpDrop.cs
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ExpDrop : MonoBehaviour
{
    public int expAmount = 100;

    void Reset()
    {
        // Collider tetik olarak ayarlansýn
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        var stats = other.GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogWarning($"PlayerStats component not found on {other.name}");
            return;
        }

        stats.GainExp(expAmount);
        Destroy(gameObject);
    }
}