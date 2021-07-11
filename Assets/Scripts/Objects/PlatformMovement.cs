using UnityEngine;

public class PlatformMovement : MonoBehaviour {

    [Header("Platform movement")]
    [SerializeField] private float yTopLimit = 3f;
    [SerializeField] private float yBotLimit = -3f;
    [SerializeField] [Range(1, 10)] private float speed = 2.5f;
    private int direction = 1;

    private void FixedUpdate() {
        // Cuando la plataforma llegue a un límite se le cambia la dirección.
        if (transform.transform.position.y >= yTopLimit) direction = -1;
        else if (transform.transform.position.y <= yBotLimit) direction = 1;

        // Movimiento vertical constante.
        transform.Translate(direction * Vector2.up * speed * Time.deltaTime);
    }
}
