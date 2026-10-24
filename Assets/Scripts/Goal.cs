using UnityEngine;
using UnityEngine.SceneManagement;

// Das Ziel: Erreicht Guenni die Schatzkiste, springt sie auf und eine Gewinn-Nachricht erscheint.
// Enter -> nächstes Level (nach dem letzten Level geht's zurück zu Level 1)
// R     -> aktuelles Level neu starten
// Dieses Skript kommt auf die Schatzkiste (mit Box Collider 2D, Is Trigger angehakt).
public class Goal : MonoBehaviour
{
    public Sprite openChestSprite;   // Hier Chest_Open reinziehen
    public GameObject winMessage;    // Hier das Text-Objekt "WinMessage" reinziehen

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
        if (!reached) return;

        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        // NEU: Enter -> nächstes Level laden
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            int nextLevel = currentLevel + 1;

            // Gibt es kein weiteres Level in der Scene List? Dann zurück zum ersten
            if (nextLevel >= SceneManager.sceneCountInBuildSettings) nextLevel = 0;

            SceneManager.LoadScene(nextLevel);
        }

        // R -> aktuelles Level neu starten
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(currentLevel);
        }
    }
}