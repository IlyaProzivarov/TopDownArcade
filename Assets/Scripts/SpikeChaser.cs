using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpikeChaser : MonoBehaviour
{
    public float moveSpeed = 2f;     
    public float chaseRange = 4f;    
    public float spinSpeed = 360f;   
    public float startDelay = 2f;    

    Rigidbody2D rb;
    Transform target;
    float timer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    void Start()
    {
        var player = FindFirstObjectByType<PlayerController>();
        if (player != null) target = player.transform;
    }

    void FixedUpdate()
    {
        rb.MoveRotation(rb.rotation + spinSpeed * Time.fixedDeltaTime);

        timer += Time.fixedDeltaTime;
        if (target == null || timer < startDelay) return;

        Vector2 toPlayer = (Vector2)target.position - rb.position;
        if (toPlayer.magnitude > chaseRange) return;

        rb.MovePosition(rb.position + toPlayer.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}