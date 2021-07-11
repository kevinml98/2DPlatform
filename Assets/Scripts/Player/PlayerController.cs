using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] [Range(1, 10)] private float speed = 6;
    [SerializeField] [Range(0, 1)] private float acceleration = 0.1f;

    private Vector2 targetVelocity; // Velocidad objetivo.
    private Vector3 currentVelocity; // Velocidad actual.

    [Header("Jump Raycast")]
    [SerializeField] private Transform groundCheck; // Punto de origen del Raycast.
    [SerializeField] private LayerMask layerMask; // Capas sobre las que actuará el Raycast.
    [SerializeField] private float groundCheckLength; // Longitud del Raycast.
    [SerializeField] private float jumpForce = 500;
    private bool isGrounded;
    private bool jumpPressed;
    private bool isJumping;

    // Componentes del objeto.
    private Rigidbody2D rb2D;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private UIManager uiManager;
    private PlayerHealth playerHealth;

    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    /**
     * Se ejecuta una vez por frame.
     * La recogida de Input se debería poner aquí.
     */
    private void Update() {
        HorizontalMovement();
        JumpMovement();

        // Si el jugador se cae, muere.
        if (transform.position.y <= -10) {
            playerHealth.TakeDamage(999);
        }
    }

    /**
     * Se ejecuta una vez cada X segundos (0.02 por defecto).
     * Los movimientos físicos se debería poner aquí.
     */
    private void FixedUpdate() {
        // Cambio gradual de la velocidad.
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref currentVelocity, acceleration);

        // Salto.
        if (jumpPressed) {
            jumpPressed = false;
            isJumping = true;
            rb2D.AddForce(Vector2.up * jumpForce);
            anim.SetTrigger("Jump");
        }
    }

    private void HorizontalMovement() {
        // Recuperamos la Input ahorizontal del usuario.
        float h = Input.GetAxisRaw("Horizontal");

        // Se calcula la velocidad objetivo. Eje X se calcula en funcion de la input y la velocidad, el eje Y no se modifica.
        targetVelocity = new Vector2(h * speed, rb2D.velocity.y);

        // Animación de correr y flip del sprite.
        anim.SetBool("isRunning", h != 0);
        if (h != 0) spriteRenderer.flipX = h < 0;
    }

    private void JumpMovement() {
        // Comprobamos si el personaje está tocando el suelo y si ha pulsado el espacio.
        // isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckLength, layerMask);
        isGrounded = Physics2D.CircleCast(groundCheck.position, groundCheckLength, Vector2.down, groundCheckLength, layerMask);

        // Se comprueba si se cumplen las condiciones del salto.
        if (rb2D.velocity.y <= 0 && isGrounded) isJumping = false;
        if (Input.GetKey(KeyCode.Space) && isGrounded && !isJumping) jumpPressed = true;

        // Animaciones de saltar y caer.
        anim.SetFloat("YVelocity", rb2D.velocity.y);
        anim.SetBool("isOnAir", !isGrounded);
    }

    /**
     * Cuando el jugador colisiona con una plataforma se pone como hijo y cuando deja de colisionar se le quita.
     */
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Platform")) {
            transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.CompareTag("Platform")) {
            transform.SetParent(null);
        }
    }

    /**
     * Cuando el jugador colisiona con unas cerezas se cambia la animación y se elimina.
     */
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Cherries")) {
            collision.GetComponent<Animator>().SetTrigger("Collected");
            uiManager.AddCherrie();
            Destroy(collision.gameObject, 0.5f);
        }
    }
}
