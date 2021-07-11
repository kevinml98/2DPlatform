using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    private float currentHealth;
    public float maxHealth;

    public GameObject deathAnim;

    private void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;
        if (currentHealth <= 0) Death();
    }

    private void Death() {
        deathAnim.SetActive(true);
        Destroy(gameObject, 0.5f);
    }
}
