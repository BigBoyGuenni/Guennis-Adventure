using UnityEngine;

// Eine Münze, die leicht auf und ab schwebt und von Guenni eingesammelt wird.
// Dieses Skript kommt auf die Münze (mit Circle Collider 2D, Is Trigger angehakt).
public class Coin : MonoBehaviour
{
    public float floatHeight = 0.25f;  // Wie weit die Münze auf und ab schwebt
    public float floatSpeed = 2f;      // Wie schnell sie schwebt

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Sanftes Auf und Ab mit einer Sinuskurve
        float offset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPosition + new Vector3(0f, offset, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Nur Guenni darf Münzen einsammeln
        if (other.GetComponent<PlayerController>() == null) return;

        if (CoinCounter.instance != null) CoinCounter.instance.AddCoin();
        Destroy(gameObject); // Münze verschwindet
    }
}