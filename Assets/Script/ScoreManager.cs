using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static float score;    
    public TMP_Text scoreText;
    public float scoreSpeed = 10f;
    public static bool isAlive = true;

    void Update()
    {
        if (!isAlive) return;

        score += scoreSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    void Awake()
    {
        score = 0;
        isAlive = true;
    }
}
