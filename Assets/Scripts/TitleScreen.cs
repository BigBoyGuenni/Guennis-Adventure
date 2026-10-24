using UnityEngine;
using UnityEngine.SceneManagement;

// Titelbildschirm: Enter oder Leertaste startet das Spiel (lädt die nächste Szene in der Scene List).
// Der "Drücke Enter"-Text blinkt dabei sanft.
// Dieses Skript kommt auf ein leeres Objekt namens "TitleScreen".
public class TitleScreen : MonoBehaviour
{
    public GameObject pressStartText;   // Hier den Text "Drücke Enter zum Starten" reinziehen
    public float blinkSpeed = 1.2f;     // Wie schnell der Text blinkt

    private float timer;

    void Update()
    {
        // Text blinken lassen: an und aus im Wechsel
        if (pressStartText != null)
        {
            timer += Time.deltaTime;
            pressStartText.SetActive((timer * blinkSpeed) % 1f < 0.7f);
        }

        // Spiel starten
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}