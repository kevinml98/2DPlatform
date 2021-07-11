using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    // Health data
    private float currentHealth;
    private float maxHealth = 4f;
    private bool damaged;
    private float melonLife = 1f;

    // Death data
    [SerializeField] private float jumpDeathForce = 200f;
    public bool isDead;

    // Bullet data
    [SerializeField] private int damagePerBullet = 1;

    [Header("UI")]
    public Image lifeUI;

    void Start() {
        currentHealth = maxHealth;
    }

    /**
     * El jugador recibe daño. Se calcula la vida que le queda y se comprueba si ha muerto.
     */
    public void TakeDamage(int amount) {
        currentHealth -= amount;
        lifeUI.fillAmount = currentHealth / maxHealth;
        if (currentHealth <= 0) Death();
    }

    /**
     * El jugador recupera vida nunca mayor a la máxima posible.
     */
    private void Recover(int amount) {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        lifeUI.fillAmount = currentHealth / maxHealth;
    }

    /**
     * Se le quita el control al usuario sobre el personaje y se ejecuta la animación de muerte.
     */
    private void Death() {
        isDead = true;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpDeathForce);

        Camera.main.GetComponent<GameOver>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Melon")) {
            if (currentHealth >= maxHealth) return;
            Recover((int)melonLife);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("PlantBullet")) {
            TakeDamage(damagePerBullet);
            collision.gameObject.SetActive(false);
        }
    }
}
