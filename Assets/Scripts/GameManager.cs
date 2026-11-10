using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject winPanel;
    public TMP_Text coinsText;

    int totalCoins;
    int collectedCoins;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        foreach (var p in FindObjectsByType<Pickup>(FindObjectsSortMode.None))
        {
            if (p.type == PickupType.Score) totalCoins++;
        }

        if (winPanel != null) winPanel.SetActive(false);
        UpdateUI();
    }

    public void CoinCollected()
    {
        collectedCoins++;
        UpdateUI();

        if (totalCoins > 0 && collectedCoins >= totalCoins) Win();
    }

    void Win()
    {
        if (winPanel != null) winPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void Restart()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateUI()
    {
        if (coinsText != null) coinsText.text = "Coins: " + collectedCoins + "/" + totalCoins;
    }
}