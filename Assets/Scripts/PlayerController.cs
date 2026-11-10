using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 6f;
    public float acceleration = 30f;
    public bool useGyro = false;
    public VirtualJoystick joystick;

    [Header("Lives")]
    public int maxLives = 5;
    public int lives = 3;
    public float invulnerableTime = 1f;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text livesText;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Color originalColor;
    Vector2 input;
    int score;
    bool invulnerable;
    float boostTimeLeft;
    float boostMultiplier = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Start()
    {
        if (SystemInfo.supportsGyroscope) Input.gyro.enabled = true;
        UpdateUI();
    }

    void Update()
    {
        input = ReadInput();
        if (boostTimeLeft > 0f) boostTimeLeft -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        float speed = boostTimeLeft > 0f ? maxSpeed * boostMultiplier : maxSpeed;
        Vector2 target = input * speed;
        rb.linearVelocity = Vector2.MoveTowards(
            rb.linearVelocity, target, acceleration * Time.fixedDeltaTime);
    }

    Vector2 ReadInput()
    {
        // Виртуальный джойстик
        Vector2 j = joystick != null ? joystick.Direction : Vector2.zero;
        if (j.sqrMagnitude > 0.01f) return j;

        // Гироскоп 
        if (useGyro && SystemInfo.supportsGyroscope)
        {
            Vector3 g = Input.gyro.gravity;
            Vector2 tilt = new Vector2(g.x, g.y) * 3f;
            if (tilt.magnitude < 0.15f) tilt = Vector2.zero; 
            return Vector2.ClampMagnitude(tilt, 1f);
        }

        // Клавиатура: WASD и стрелки
        Vector2 k = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        return Vector2.ClampMagnitude(k, 1f);
    }

    public void ToggleGyro() { useGyro = !useGyro; }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void Heal(int amount)
    {
        lives = Mathf.Min(lives + amount, maxLives);
        UpdateUI();
    }

    public void SpeedBoost(float multiplier, float duration)
    {
        boostMultiplier = multiplier;
        boostTimeLeft = duration;
    }

    public void TakeDamage(int amount)
    {
        if (invulnerable) return;
        lives -= amount;
        UpdateUI();

        if (lives <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        StartCoroutine(InvulnerabilityRoutine());
    }

    IEnumerator InvulnerabilityRoutine()
    {
        invulnerable = true;
        Color c = originalColor;
        c.a = 0.4f;
        sr.color = c;
        yield return new WaitForSeconds(invulnerableTime);
        sr.color = originalColor;
        invulnerable = false;
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (livesText != null) livesText.text = "Lives: " + lives;
    }
}