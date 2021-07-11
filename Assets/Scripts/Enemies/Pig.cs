using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour {
    
    // Basic attributes.
    private int speed;
    private int direction = 1;
    private float limit = 2;
    private float limitPosRight;
    private float limitPosLeft;
    private float distanceToPlayer = 2.5f;

    // Collision.
    private float forceUp = 100;
    private float forceRight = 100;
    private bool hitted;
    private int damagePerHit = 1;
    private int autodamagePerHit = 1;
    private int projectileDamage = 2;

    // Objects.
    private SpriteRenderer spriteRenderer;
    private GameObject player;
    private Animator pigAnimator;
    private Rigidbody2D rb;
    private EnemyHealth health;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        pigAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();
        health.maxHealth = 6f;
    }

    private void Start() {
        limitPosRight = transform.position.x + limit;
        limitPosLeft = transform.position.x - limit;
    }

    private void FixedUpdate() {
        if (hitted) return;
        if (Vector2.Distance(transform.position, player.transform.position) > distanceToPlayer) {
            Patrol();
        } else {
            FollowPlayer();
        }
    }

    private void Patrol() {
        // Animación de patrulla.
        pigAnimator.SetBool("isRunning", false);

        // En base a la posición se establece la dirección.
        if (transform.position.x >= limitPosRight) direction = -1;
        else if (transform.position.x <= limitPosLeft) direction = 1;

        // Movimiento del enemigo
        speed = 1;
        spriteRenderer.flipX = direction == 1 ? true : false;
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);
    }

    private void FollowPlayer() {
        // Animación de correr.
        pigAnimator.SetBool("isRunning", true);

        // Si el jugador está cerca se mueve hacia él.
        speed = 2;
        Vector2 posToGo = new Vector2(player.transform.position.x, transform.position.y);
        spriteRenderer.flipX = posToGo.x > transform.position.x ? true : false;
        transform.position = Vector2.MoveTowards(transform.position, posToGo, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) {
            // Se calcula la dirección de la fuerza a aplicar en base a la orientación del sprite.
            int directionForce = 1;
            if (spriteRenderer.flipX) directionForce = -1;
            // Se activa la animación de golpeo.
            SetHitted();
            // Corrutina para que el enemigo vuelva a su estado normal pasado un tiempo.
            StartCoroutine(BackToNormal());
            // Se aplica fuerza tanto al jugador como al enemigo y se le quita vida al enemigo.
            CollisionForce(rb, directionForce);
            CollisionForce(player.GetComponent<Rigidbody2D>(), -directionForce);
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damagePerHit);
            health.TakeDamage(autodamagePerHit);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Pineapple")) {
            // Se calcula la dirección de la fuerza en base a la posición del proyectil respecto al enemigo.
            int directionForce = 1;
            if (collision.gameObject.GetComponent<Transform>().position.x > transform.position.x) directionForce = -1;
            // Animación de golpeo y vuelta a normalidad.
            SetHitted();
            StartCoroutine(BackToNormal());
            // Se aplica fuerza al enemigo, se le quita vida y se desactiva el proyectil.
            CollisionForce(rb, directionForce);
            health.TakeDamage(projectileDamage);
            collision.gameObject.SetActive(false);
        }
    }

    private void SetHitted() {
        hitted = true;
        pigAnimator.SetBool("Hitting", true);
    }

    private void CollisionForce(Rigidbody2D rb, int direction) {
        rb.AddForce(Vector2.up * forceUp);
        rb.AddForce(Vector2.right * forceRight * direction);
    }

    private IEnumerator BackToNormal() {
        yield return new WaitForSeconds(2);
        hitted = false;
        pigAnimator.SetBool("Hitting", false);
    }
}
