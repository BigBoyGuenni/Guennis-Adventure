using UnityEngine;

// Animiert Guenni: Wippen im Stehen, Laufen, Springen und Fallen.
// Dieses Skript kommt auf Guenni, zusätzlich zum PlayerController.
// Die Bilder werden einfach im Inspector in die Listen gezogen, in der Reihenfolge, in der sie abgespielt werden sollen.
public class GuenniAnimation : MonoBehaviour
{
    [Header("Stehen (Wippen)")]
    public Sprite[] idleFrames;          // z. B. Normal, Zwischen, Gestaucht, Zwischen
    public float idleFrameTime = 0.25f;  // Sekunden pro Bild beim Wippen

    [Header("Laufen")]
    public Sprite[] runFrames;           // z. B. Schritt, Normal, Gekreuzt, Normal
    public float runFrameTime = 0.1f;    // Sekunden pro Bild beim Laufen

    [Header("In der Luft")]
    public Sprite jumpSprite;            // Bild beim Hochspringen
    public Sprite fallSprite;            // Bild beim Runterfallen

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private PlayerController player;

    private enum State { Idle, Run, Jump, Fall }
    private State currentState;
    private int frameIndex;
    private float frameTimer;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        // 1. Herausfinden, was Guenni gerade tut
        State newState;
        if (!player.IsGrounded)
        {
            newState = rb.linearVelocity.y > 0.1f ? State.Jump : State.Fall;
        }
        else if (player.enabled && Mathf.Abs(player.MoveInput) > 0.01f)
        {
            newState = State.Run;
        }
        else
        {
            newState = State.Idle; // steht still (oder hat das Ziel erreicht)
        }

        // 2. Neuer Zustand? Dann Animation von vorne beginnen
        if (newState != currentState)
        {
            currentState = newState;
            frameIndex = 0;
            frameTimer = 0f;
        }

        // 3. Passendes Bild anzeigen
        switch (currentState)
        {
            case State.Idle: PlayLoop(idleFrames, idleFrameTime); break;
            case State.Run:  PlayLoop(runFrames, runFrameTime); break;
            case State.Jump: if (jumpSprite != null) sprite.sprite = jumpSprite; break;
            case State.Fall: if (fallSprite != null) sprite.sprite = fallSprite; break;
        }
    }

    // Spielt eine Liste von Bildern immer wieder hintereinander ab
    void PlayLoop(Sprite[] frames, float frameTime)
    {
        if (frames == null || frames.Length == 0) return;

        frameTimer += Time.deltaTime;
        if (frameTimer >= frameTime)
        {
            frameTimer = 0f;
            frameIndex = (frameIndex + 1) % frames.Length;
        }
        sprite.sprite = frames[frameIndex];
    }
}