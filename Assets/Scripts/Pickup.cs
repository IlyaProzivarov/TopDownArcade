using UnityEngine;

public enum PickupType { Damage, Score, Speed, Health }

public class Pickup : MonoBehaviour
{
    public PickupType type;
    public int value = 1;         
    public float multiplier = 1.8f; 
    public float duration = 3f;     

    void OnTriggerEnter2D(Collider2D other)
    {
        var p = other.GetComponent<PlayerController>();
        if (p == null) return;

        switch (type)
        {
            case PickupType.Damage: p.TakeDamage(value); return; 
            case PickupType.Score: p.AddScore(value); break;
            case PickupType.Speed: p.SpeedBoost(multiplier, duration); break;
            case PickupType.Health: p.Heal(value); break;
        }
        Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (type != PickupType.Damage) return;
        var p = other.GetComponent<PlayerController>();
        if (p != null) p.TakeDamage(value);
    }
}