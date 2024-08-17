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
    int maxScore;

    public RectTransform rect;
    private float lifeImageWidth;
    private int maxLives;
    private int currLives;

    public GameObject gameOverScreen;

    AudioManager sounds;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        Time.timeScale = 1;

        score = 0;
        maxScore = 150;
        scoreText.text = score.ToString() + " / " + maxScore.ToString();

        lifeImageWidth = 100f;
        maxLives = 3;
        currLives = maxLives;
        rect.sizeDelta = new Vector2 (lifeImageWidth * currLives, rect.sizeDelta.y);

        sounds = FindObjectOfType<AudioManager>();
    }

    public void AddScore(int num) {
        score += num;
        scoreText.text = score.ToString() + " / " + maxScore.ToString();

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
        }else if (score >= maxScore) {
            SceneManager.LoadScene("Credits");
        }
    }

    public void TakeDamage(int num) {
        currLives -= num;
        rect.sizeDelta = new Vector2(lifeImageWidth * currLives, rect.sizeDelta.y);
        PlayerMovement.instance.transform.position = spawn.position;
        if (currLives <= 0) {
            gameOverScreen.SetActive(true);
            PlayerMovement.instance.gameObject.SetActive(false);
            Time.timeScale = 0;
        }
    }
}
