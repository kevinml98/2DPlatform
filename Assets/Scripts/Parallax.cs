using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase para hacer un efecto Parallax. Se debe poner como componente al fondo.
/// Efecto Parallax: El fondo se mueve a una velocidad diferente al player.
/// </summary>
public class Parallax : MonoBehaviour {

    // Camara
    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    // Movimiento
    private float distanceToBackground = 10;
    private float parallaxSmooth = 1;
    private Vector3 backgroundPosition;

    private void Awake() {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
    }

    private void Update() {
        // Calculamos la posición del fondo.
        float parallaxValue = (previousCameraPosition.x - cameraTransform.position.x) * distanceToBackground;
        backgroundPosition = new Vector3(transform.position.x + parallaxValue, transform.position.y, transform.position.z);

        previousCameraPosition = cameraTransform.position;
    }

    private void FixedUpdate() {
        // Movemos el fondo.
        transform.position = Vector3.Lerp(transform.position, backgroundPosition, parallaxSmooth * Time.deltaTime);
    }
}
