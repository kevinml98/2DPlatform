using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] private Transform player;
    private Vector3 offset;
    private float smoothTargetTime = 0.1f; // Velocidad con la que la cámara va a seguir al player.
    private Vector3 currentVelocity;
    private PlayerHealth playerHealth;
    private Vector3 targetPosition;

    public Transform limitRight;
    public Transform limitLeft;
    public Transform limitUp;
    public Transform limitDown;

    private void Start() {
        offset = transform.position - player.position;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void FixedUpdate() {
        // La cámara seguirá al jugador siempre y cuando no esté muerto.
        if (!playerHealth.isDead) {
            targetPosition = player.position + offset;
            CheckLimits();
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTargetTime);
        }
    }

    private void CheckLimits() {
        if (targetPosition.x < limitRight.position.x || targetPosition.x > limitLeft.position.x) targetPosition.x = transform.position.x;
        if (targetPosition.y < limitDown.position.y || targetPosition.y > limitUp.position.y) targetPosition.y = transform.position.y;
    }
}
