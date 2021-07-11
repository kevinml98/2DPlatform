using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [Header("Fired object")]
    [SerializeField] private Transform posRight;
    [SerializeField] private Transform posLeft;
    [SerializeField] private float timeBetweenProjectiles;
    private float timer;

    private SpriteRenderer spriteRenderer;
    private ObjectPool projectilePool;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        projectilePool = GetComponent<ObjectPool>();
    }

    private void Update() {
        // Timer (cadencia de disparo)
        timer += Time.deltaTime;

        // Si se presiona el botón izquierdo del ratón se activa un proyectil.
        if (Input.GetMouseButtonDown(0) && timer >= timeBetweenProjectiles) {
            timer = 0;
            if (spriteRenderer.flipX) {
                InstantiateProjectile(posLeft, -1);
            } else {
                InstantiateProjectile(posRight, 1);
            }

        }
    }

    /**
     * Se recoge un proyectil del pool, se le indica la posición y dirección que debe tomar y se activa.
     */
    private void InstantiateProjectile(Transform transform, int direction) {
        GameObject projectile = projectilePool.GetAvaliableObject();
        projectile.transform.position = transform.position;
        projectile.GetComponent<Projectile>().direction = direction;
        projectile.SetActive(true);
    }
}
