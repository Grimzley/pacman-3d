using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public static GhostManager ghosts;

    public TMP_Text scoreText;
    int score;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        score = 0;
        scoreText.text = score.ToString() + " / 150";
    }

    public void AddScore(int num) {
        score += num;
        scoreText.text = score.ToString() + " / 150";
    }
}
