using UnityEngine;

// Unsichtbare Todeszone: Fällt Guenni hinein, startet er von vorne.
// Dieses Skript kommt auf ein leeres Objekt mit einem Box Collider 2D (Is Trigger angehakt).
public class DeathZone : MonoBehaviour
{
    // Wird automatisch aufgerufen, wenn etwas in den Trigger hineinfällt
    void OnTriggerEnter2D(Collider2D other)
    {
        // Ist das, was hineingefallen ist, Guenni?
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.Respawn();
        }
    }

    // Zeigt die Zone im Scene-Fenster rot an (im Spiel unsichtbar)
    void OnDrawGizmos()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box == null) return;
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(box.offset, box.size);
    }
}