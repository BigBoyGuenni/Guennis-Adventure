using UnityEngine;

// Steuert Guenni: links/rechts laufen und springen.
// Dieses Skript kommt auf Guenni (der schon Rigidbody2D und BoxCollider2D hat).
public class PlayerController : MonoBehaviour
{
    [Header("Bewegung")]
    public float moveSpeed = 5f;      // Wie schnell Guenni läuft
    public float jumpForce = 12f;     // Wie hoch Guenni springt

    [Header("Sprung-Gefühl")]
    public float fallMultiplier = 2.5f;     // Beim Fallen wird die Schwerkraft so viel stärker -> weniger "schwebend"
    public float lowJumpMultiplier = 2f;    // Leertaste früh loslassen -> kleiner Sprung
    public float maxFallSpeed = 20f;        // Höchste Fallgeschwindigkeit, damit er nicht endlos schneller wird
    public float coyoteTime = 0.12f;        // So lange (Sekunden) darf Guenni nach der Kante noch springen
    public float jumpBufferTime = 0.12f;    // Sprung kurz vor der Landung gedrückt? Wird trotzdem ausgeführt

    [Header("Bodenprüfung")]
    public Transform groundCheck;     // Leeres Objekt an Guennis Füßen
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;     // Layer "Ground" (Baum, Boden, Plattformen)

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private float moveInput;
    private bool isGrounded;
    private bool jumpHeld;            // Wird die Sprungtaste gerade gehalten?
    private float coyoteCounter;      // Restzeit für den "Kanten-Sprung"
    private float jumpBufferCounter;  // Restzeit für einen gemerkten Sprung
    private Vector3 startPosition;    // Hier startet Guenni, hierhin kommt er beim Respawn zurück
    private MovingPlatform currentPlatform; // Plattform, auf der Guenni gerade steht (oder null)

    // NEU: Damit das Animations-Skript nachfragen kann, ob Guenni am Boden steht und ob er laufen will
    public bool IsGrounded => isGrounded;
    public float MoveInput => moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
    }

    // Setzt Guenni an den Start zurück. Wird von der DeathZone aufgerufen.
    public void Respawn()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        // Eingabe lesen: A/D oder Pfeiltasten -> Wert zwischen -1 und 1
        moveInput = Input.GetAxisRaw("Horizontal");
        jumpHeld = Input.GetButton("Jump");

        // Steht Guenni auf dem Boden? Wir merken uns auch, WORAUF er steht
        Collider2D ground = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isGrounded = ground != null;

        // Ist das, worauf er steht, eine bewegliche Plattform?
        currentPlatform = isGrounded ? ground.GetComponent<MovingPlatform>() : null;

        // Coyote Time – solange er am Boden ist, Zähler voll; danach läuft er ab
        if (isGrounded) coyoteCounter = coyoteTime;
        else coyoteCounter -= Time.deltaTime;

        // Jump Buffer – Sprungdruck kurz "merken"
        if (Input.GetButtonDown("Jump")) jumpBufferCounter = jumpBufferTime;
        else jumpBufferCounter -= Time.deltaTime;

        // Springen, wenn ein Sprung gemerkt ist UND Guenni (fast) am Boden ist
        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
            coyoteCounter = 0f; // verhindert Doppelsprung
        }

        // Guenni in Laufrichtung drehen
        if (moveInput > 0) sprite.flipX = false;
        else if (moveInput < 0) sprite.flipX = true;
    }

    void FixedUpdate()
    {
        // Physik-Bewegung gehört in FixedUpdate
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput * moveSpeed;

        // Bessere Schwerkraft
        float gravity = Physics2D.gravity.y * rb.gravityScale;
        if (velocity.y < 0f)
        {
            // Beim Fallen: zusätzliche Schwerkraft
            velocity.y += gravity * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (velocity.y > 0f && !jumpHeld)
        {
            // Steigt, aber Taste losgelassen: früher abbremsen -> kleiner Sprung
            velocity.y += gravity * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }

        // Fallgeschwindigkeit begrenzen
        if (velocity.y < -maxFallSpeed) velocity.y = -maxFallSpeed;

        // Auf einer beweglichen Plattform fährt Guenni mit
        if (currentPlatform != null)
        {
            velocity.x += currentPlatform.Velocity.x;

            // Fährt die Plattform nach unten, bleibt Guenni drauf, statt kurz hinterherzufallen
            // (gilt nicht, wenn er gerade abspringt)
            if (velocity.y <= currentPlatform.Velocity.y)
                velocity.y = currentPlatform.Velocity.y;
        }

        rb.linearVelocity = velocity;
    }

    // Zeigt den Bodenprüfungs-Kreis im Scene-Fenster an (nur zur Hilfe)
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}