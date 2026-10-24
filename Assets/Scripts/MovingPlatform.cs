using UnityEngine;

// Eine Plattform, die sanft zwischen ihrem Startpunkt und einem Zielpunkt hin und her fährt.
// Dieses Skript kommt auf die Plattform (mit Box Collider 2D, Layer "Ground").
// Die Plattform braucht außerdem einen Rigidbody 2D mit Body Type "Kinematic".
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    public Vector2 moveOffset = new Vector2(5f, 0f); // Wohin sie fährt (vom Start aus): X = seitlich, Y = hoch/runter
    public float cycleTime = 4f;                      // Sekunden für einmal hin und zurück

    // Wie schnell sich die Plattform gerade bewegt (liest Guenni aus, um mitzufahren)
    public Vector2 Velocity { get; private set; }

    private Rigidbody2D rb;
    private Vector2 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
    }

    void FixedUpdate()
    {
        // Wert zwischen 0 und 1, der sanft hin und her pendelt (an den Enden langsamer)
        float t = (1f - Mathf.Cos(Time.time * 2f * Mathf.PI / cycleTime)) / 2f;
        Vector2 newPosition = startPosition + moveOffset * t;

        Velocity = (newPosition - rb.position) / Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    // Zeigt den Fahrweg im Scene-Fenster als Linie an
    void OnDrawGizmos()
    {
        Vector3 from = Application.isPlaying ? (Vector3)startPosition : transform.position;
        Vector3 to = from + (Vector3)moveOffset;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(from, to);
        Gizmos.DrawWireSphere(to, 0.3f);
    }
}