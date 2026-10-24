using UnityEngine;

// Ein Gegner, der auf seiner Plattform hin und her krabbelt und an Kanten und Wänden umdreht.
// Berührt Guenni ihn von der Seite -> Guenni startet neu.
// Springt Guenni von oben drauf -> Gegner ist besiegt und Guenni hüpft hoch.
// Dieses Skript kommt auf den Gegner (mit Box Collider 2D und Rigidbody 2D "Kinematic").
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Bewegung")]
    public float speed = 2f;           // Wie schnell der Gegner krabbelt
    public LayerMask groundLayer;      // Layer "Ground", damit er Kanten und Wände erkennt

    [Header("Animation")]
    public Sprite frame1;              // Erstes Bild (z. B. Ant1)
    public Sprite frame2;              // Zweites Bild (z. B. Ant2)
    public float frameTime = 0.15f;    // Wie schnell die Bilder wechseln (Sekunden)

    [Header("Draufspringen")]
    public float bounceForce = 10f;    // Wie hoch Guenni nach dem Draufspringen hüpft

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private BoxCollider2D box;
    private int direction = 1;         // 1 = nach rechts, -1 = nach links
    private float frameTimer;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isDead) return;

        // Krabbel-Animation: zwischen zwei Bildern hin und her wechseln
        if (frame1 != null && frame2 != null)
        {
            frameTimer += Time.deltaTime;
            if (frameTimer >= frameTime)
            {
                frameTimer = 0f;
                sprite.sprite = (sprite.sprite == frame1) ? frame2 : frame1;
            }
        }

        // In Laufrichtung schauen (das Bild schaut nach rechts)
        sprite.flipX = direction < 0;
    }

    void FixedUpdate()
    {
        if (isDead) return;

        Bounds b = box.bounds;

        // Kantenprüfung: Ist vor den Füßen noch Boden?
        Vector2 footFront = new Vector2(b.center.x + direction * (b.extents.x + 0.05f), b.min.y + 0.1f);
        bool groundAhead = Physics2D.Raycast(footFront, Vector2.down, 0.4f, groundLayer);

        // Wandprüfung: Ist direkt vor ihm eine Wand?
        Vector2 middle = b.center;
        bool wallAhead = Physics2D.Raycast(middle, Vector2.right * direction, b.extents.x + 0.1f, groundLayer);

        // Keine Kante voraus oder Wand im Weg -> umdrehen
        if (!groundAhead || wallAhead) direction *= -1;

        // Ein Stück weiterkrabbeln
        rb.MovePosition(rb.position + Vector2.right * direction * speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        PlayerController player = collision.collider.GetComponent<PlayerController>();
        if (player == null) return; // war nicht Guenni

        // Kommt Guenni von oben? (seine Füße sind höher als die Mitte des Gegners)
        bool fromAbove = collision.collider.bounds.min.y > box.bounds.center.y;

        if (fromAbove)
        {
            // Guenni hüpft hoch
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, bounceForce);
            Squash();
        }
        else
        {
            // Seitlich erwischt -> Guenni startet neu
            player.Respawn();
        }
    }

    // Gegner wird plattgedrückt und verschwindet kurz danach
    void Squash()
    {
        isDead = true;
        box.enabled = false; // nicht mehr anfassbar
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 0.4f, 1f);
        Destroy(gameObject, 0.4f);
    }

    // Zeigt die Prüf-Linien im Scene-Fenster an (nur zur Hilfe)
    void OnDrawGizmosSelected()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col == null) return;
        Bounds b = col.bounds;
        Gizmos.color = Color.magenta;
        Vector3 foot = new Vector3(b.center.x + direction * (b.extents.x + 0.05f), b.min.y + 0.1f, 0f);
        Gizmos.DrawLine(foot, foot + Vector3.down * 0.4f);
        Gizmos.DrawLine(b.center, b.center + Vector3.right * direction * (b.extents.x + 0.1f));
    }
}