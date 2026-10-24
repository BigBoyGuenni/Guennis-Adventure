using UnityEngine;
using UnityEngine.SceneManagement;

// Das Ziel: Erreicht Guenni die Schatzkiste, springt sie auf,
// eine Gewinn-Nachricht erscheint und mit R startet das Level neu.
// Dieses Skript kommt auf die Schatzkiste (mit Box Collider 2D, Is Trigger angehakt).
public class Goal : MonoBehaviour
{
    public Sprite openChestSprite;   // Hier Chest_Open reinziehen
    public GameObject winMessage;    // Hier das Text-Objekt "Geschafft!" reinziehen

    private bool reached = false;

    void Start()
    {
        // Nachricht am Anfang verstecken
        if (winMessage != null) winMessage.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (reached) return; // nur einmal auslösen

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return; // war nicht Guenni

        reached = true;

        // Kiste öffnen
        GetComponent<SpriteRenderer>().sprite = openChestSprite;

        // Nachricht zeigen
        if (winMessage != null) winMessage.SetActive(true);

        // Guenni anhalten und Steuerung ausschalten
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        player.enabled = false;
    }

    void Update()
    {
        // Nach dem Gewinnen: R drückt -> Level neu starten
        if (reached && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}