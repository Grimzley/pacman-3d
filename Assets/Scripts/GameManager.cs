using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Transform spawn;

    public TMP_Text scoreText;
    int score;

    public RectTransform rect;
    private float lifeImageWidth;
    private int maxLives;
    private int currLives;

    AudioManager sounds;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        score = 0;
        scoreText.text = score.ToString() + " / 150";

        lifeImageWidth = 100f;
        maxLives = 3;
        currLives = maxLives;
        rect.sizeDelta = new Vector2 (lifeImageWidth * currLives, rect.sizeDelta.y);

        sounds = FindObjectOfType<AudioManager>();
    }

    public void AddScore(int num) {
        score += num;
        scoreText.text = score.ToString() + " / 150";

        if (score == 1) {
            sounds.Play("Background");
            GhostManager.instance.WakeBlinky();
        }else if (score == 5) {
            GhostManager.instance.WakePinky();
        }else if (score == 10) {
            GhostManager.instance.WakeInky();
        }else if (score == 15) {
            GhostManager.instance.WakeClyde();
        }else if (score == 50) {
            sounds.Play("Background2");
        }else if (score == 100) {
            sounds.Pause("Background2");
            sounds.Play("Background3");
        }
    }

    public void TakeDamage(int num) {
        currLives -= num;
        rect.sizeDelta = new Vector2(lifeImageWidth * currLives, rect.sizeDelta.y);
        PlayerMovement.instance.transform.position = spawn.position;
        if (currLives <= 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
