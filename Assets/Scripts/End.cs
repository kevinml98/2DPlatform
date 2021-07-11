using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{

    public GameObject player;
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            anim.SetTrigger("Picked");
            player.GetComponent<PlayerController>().enabled = false;
            Camera.main.GetComponent<GameOver>().enabled = true;
        }
    }
}
