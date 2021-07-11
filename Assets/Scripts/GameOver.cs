using UnityEngine;

public class GameOver : MonoBehaviour {

    private Camera camara;
    [SerializeField] private float minSizeValue = 2;
    [SerializeField] private float factor = 0.8f;

    public GameObject UIGameOver;

    private void Awake() {
        camara = GetComponent<Camera>();
    }

    private void FixedUpdate() {
        if (camara.orthographicSize > minSizeValue) {
            camara.orthographicSize -= Time.deltaTime * factor;
        }
        if (camara.orthographicSize <= minSizeValue && !UIGameOver.activeSelf) {
            UIGameOver.SetActive(true);
        }
    }
    public void LoadScene(string name) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }
}
