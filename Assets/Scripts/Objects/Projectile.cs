using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] [Range(1, 10)] private float speed = 7;
    public int direction;

    [SerializeField] private float timeAlive = 3;
    private float timer;

    private void Update() {
        // Cada cierto tiempo se desactiva el objeto.
        timer += Time.deltaTime;
        if (gameObject.activeInHierarchy && timer >= timeAlive) {
            timer = 0;
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate() {
        // Movimiento constante en el eje X.
        transform.Translate(direction * Vector2.right * speed * Time.deltaTime);
    }
}
