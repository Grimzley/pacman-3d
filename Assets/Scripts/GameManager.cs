using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public TMP_Text scoreText;
    int score;

    AudioManager sounds;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        score = 0;
        scoreText.text = score.ToString() + " / 150";
        sounds = FindObjectOfType<AudioManager>();
    }

    public void AddScore(int num) {
        score += num;
        scoreText.text = score.ToString() + " / 150";

        if (score == 1) {
            sounds.Play("Background");
        }else if (score == 10) {
            GhostManager.instance.WakeInky();
        }else if (score == 20) {
            GhostManager.instance.WakeClyde();
        }else if (score == 50) {
            sounds.Play("Background2");
        }else if (score == 100) {
            sounds.Pause("Background2");
            sounds.Play("Background3");
        }
    }

    public void TakeDamage() {

    }
}
