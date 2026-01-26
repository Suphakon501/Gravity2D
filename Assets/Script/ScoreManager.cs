using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public float score;
    public float scoreSpeed = 10f;
    public bool isAlive = true;

    void Update()
    {
        if (!isAlive) return;

        score += scoreSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

}

