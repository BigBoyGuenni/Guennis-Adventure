using UnityEngine;

// Lässt die Kamera Guenni weich hinterherfahren.
// Dieses Skript kommt auf die Main Camera (NICHT auf Guenni).
public class CameraFollow : MonoBehaviour
{
    public Transform target;          // Hier Guenni reinziehen
    public float smoothTime = 0.2f;   // Kleiner = Kamera folgt schneller, größer = weicher
    public Vector2 offset = new Vector2(2f, 1f); // Kamera etwas vor und über Guenni

    private Vector3 velocity = Vector3.zero;

    // LateUpdate läuft nach der Bewegung von Guenni, deshalb ruckelt es nicht
    void LateUpdate()
    {
        if (target == null) return;

        // Zielposition: Guennis Position plus Versatz, Z bleibt bei der Kamera (-10)
        Vector3 targetPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z
        );

        // Weich zur Zielposition gleiten
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}