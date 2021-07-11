using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour {

    // Movimiento
    public List<Transform> positions = new List<Transform>();
    private int speed = 6;
    private float timer = 0;
    private float timeBetweenChanges = 3.5f;
    private int currentPos;
    private int targetPos;

    // Componentes
    private Animator anim;
    private SpriteRenderer sprite;

    private void Awake() {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        currentPos = 0;
    }

    private void Update() {
        // Cada X tiempo se mueve.
        timer += Time.deltaTime;
        if (timer >= timeBetweenChanges) {
            timer = 0;
            ChangePosition();
        }

        // Cuando llega a su posición destino se activa la animación de posarse.
        if (transform.position == positions[targetPos].position) {
            anim.SetBool("isFlying", false);
        }
    }

    private void FixedUpdate() {
        // Movimiento de posición actual a la posición destino.
        transform.position = Vector2.MoveTowards(transform.position, positions[targetPos].position, speed * Time.deltaTime);
    }

    /**
     * Calcula la posición destino, activa la animación de vuelo y gira el sprite si es necesario.
     */
    private void ChangePosition() {
        anim.SetBool("isFlying", true);
        targetPos = GetTargetPosition();
        sprite.flipX = positions[targetPos].position.x > transform.position.x ? true : false;
        currentPos = targetPos;
    }

    /**
     * Recupera una posición aleatoria que no sea la actual de manera recursiva. Al ser una lista tan pequeña no es lo más eficiente.
     */
    private int GetTargetPosition() {
        int targetPosition = Random.Range(0, positions.Count);
        if (targetPosition == currentPos) {
            targetPosition = GetTargetPosition();
        }

        return targetPosition;
    }
}
