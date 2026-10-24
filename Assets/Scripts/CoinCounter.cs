using UnityEngine;
using TMPro;

// Zählt die eingesammelten Münzen und zeigt sie oben im Bild an.
// Dieses Skript kommt auf das Text-Objekt "CoinText" im Canvas.
public class CoinCounter : MonoBehaviour
{
    // "instance" erlaubt jeder Münze, den Zähler direkt zu finden
    public static CoinCounter instance;

    private TMP_Text coinText;
    private int coins = 0;
    private int totalCoins = 0;

    void Awake()
    {
        instance = this;
        coinText = GetComponent<TMP_Text>();
    }

    void Start()
    {
        // Zählt automatisch, wie viele Münzen im Level liegen
        totalCoins = FindObjectsByType<Coin>(FindObjectsSortMode.None).Length;
        UpdateText();
    }

    public void AddCoin()
    {
        coins++;
        UpdateText();
    }

    void UpdateText()
    {
        coinText.text = "Münzen: " + coins + " / " + totalCoins;
    }
}