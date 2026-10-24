using UnityEngine;

// Endloser Hintergrund mit Tiefeneffekt (Parallax).
// Dieses Skript kommt auf jede der drei Hintergrund-Ebenen.
// Läuft nach der Kamera (DefaultExecutionOrder), damit nichts ruckelt.
[DefaultExecutionOrder(100)]
public class ParallaxLayer : MonoBehaviour
{
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f; // 1 = bewegt sich mit der Kamera (ganz weit weg), 0 = steht still (ganz nah)
    public float yOffset = 0f;          // Hintergrund etwas höher oder tiefer schieben

    private Transform cam;
    private float startX;
    private float spriteWidth;

    void Start()
    {
        cam = Camera.main.transform;
        startX = transform.position.x;

        // Breite EINES Bildes (nicht der ganzen gekachelten Fläche)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.sprite.bounds.size.x * transform.localScale.x;
    }

    void LateUpdate()
    {
        // Wie weit sich die Ebene mit der Kamera mitbewegt
        float distance = cam.position.x * parallaxFactor;
        // Wie weit die Kamera der Ebene "davongelaufen" ist
        float moved = cam.position.x * (1f - parallaxFactor);

        // Ebene horizontal verschieben, vertikal immer mit der Kamera mitnehmen
        transform.position = new Vector3(startX + distance, cam.position.y + yOffset, transform.position.z);

        // Ist die Kamera eine Bildbreite weitergelaufen? Dann Ebene nachziehen -> endlos
        if (moved > startX + spriteWidth) startX += spriteWidth;
        else if (moved < startX - spriteWidth) startX -= spriteWidth;
    }
}