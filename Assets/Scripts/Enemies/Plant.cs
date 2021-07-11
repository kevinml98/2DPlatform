using UnityEngine;

public class Plant : MonoBehaviour {

    [Header("Player")]
    public Transform player;

    [Header("Bullet")]
    private GameObject bullet;
    private Animator anim;
    private SpriteRenderer sprite;
    private ObjectPool bulletPool;

    [Header("Positions")]
    public Transform posRight;
    public Transform posLeft;
    private Transform bulletPos;

    // Shoot control
    [SerializeField] private float distanceToPlayer = 4;
    private int direction = 1; // 1: Player a la derecha; -1: Player a la izquierda.
    private bool isAttacking;
    // Self damage
    private EnemyHealth health;
    [SerializeField] private int projectileDamage = 2;

    private void Awake() {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        bulletPool = GetComponent<ObjectPool>();
        health = GetComponent<EnemyHealth>();
        health.maxHealth = 4f;
    }

    private void Update() {
        WherePlayerIs();
        if (Vector2.Distance(player.position, transform.position) <= distanceToPlayer) {
            isAttacking = true;
            anim.SetBool("Attack", true);
        } else if (isAttacking){
            isAttacking = false;
            anim.SetBool("Attack", false);
        }
    }

    /**
     * Este método se invoca por una evento en un frame determinado de la animación de ataque.
     */
    private void Shoot() {
        anim.SetBool("Attack", true);
        bullet = bulletPool.GetAvaliableObject();
        bullet.transform.position = bulletPos.position;
        bullet.GetComponent<Projectile>().direction = direction;
        bullet.SetActive(true);
    }

    private void WherePlayerIs() {
        if (player.position.x < transform.position.x) {
            direction = -1;
            sprite.flipX = false;
            bulletPos = posLeft;
        } else if (player.position.x > transform.position.x) {
            direction = 1;
            sprite.flipX = true;
            bulletPos = posRight;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Pineapple")) {
            // Animación de golpeo.
            anim.SetTrigger("Hitted");
            // Se aplica fuerza al enemigo, se le quita vida y se desactiva el proyectil.
            health.TakeDamage(projectileDamage);
            collision.gameObject.SetActive(false);
        }
    }
}
