using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public bool gameStarted;
    public BeatScroller rhythmScrolling;
    public static GameManager instance;
    public int playerScore;
    public int scorePerNote = 100;
    public int playerMultiplier = 1;
    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;
    public int multiplierThreshold;
    public int[] multiplierThresholds;
    public Text scoreDisplay;
    public Text multiplierDisplay;
    public float totalNotes;
    public float regularHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;
    public GameObject resultsPanel;
    public Text hitPercentageText, regularHitsText, goodHitsText, perfectHitsText, missedHitsText, rankText, finalScoreText;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        scoreDisplay.text = "Score: 0";
        totalNotes = FindObjectsOfType<NoteObject>().Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted)
        {
            if (Input.anyKeyDown)
            {
                gameStarted = true;
                rhythmScrolling.isScrolling = true;
                backgroundMusic.Play();
            }
        }
        else
        {
            if (!backgroundMusic.isPlaying && !resultsPanel.activeInHierarchy)
            {
                resultsPanel.SetActive(true);
                regularHitsText.text = "" + regularHits;
                goodHitsText.text = goodHits.ToString();
                perfectHitsText.text = perfectHits.ToString();
                missedHitsText.text = "" + missedHits;

                float totalHit = regularHits + goodHits + perfectHits;
                float percentHit = (totalHit / totalNotes) * 100f;

                hitPercentageText.text = percentHit.ToString("F1") + "%";

                string rankValue = "F";
                if (percentHit > 40)
                {
                    rankValue = "D";
                    if (percentHit < 55)
                    {
                        rankValue = "C";
                        if (percentHit < 70)
                        {
                            rankValue = "B";
                            if (percentHit < 85)
                            {
                                rankValue = "A";
                                if (percentHit < 95)
                                {
                                    rankValue = "S";
                                }
                            }
                        }
                    }
                }

                rankText.text = rankValue;

                finalScoreText.text = playerScore.ToString();
            }
        }
    }

    public void NoteHit()
    {
        if (playerMultiplier - 1 < multiplierThresholds.Length)
        {
            multiplierThreshold++;

            if (multiplierThresholds[playerMultiplier - 1] <= multiplierThreshold)
            {
                multiplierThreshold = 0;
                playerMultiplier++;
            }
        }

        multiplierDisplay.text = "Multiplier: x" + playerMultiplier;

        playerScore += scorePerNote * playerMultiplier;
        scoreDisplay.text = "Score: " + playerScore;
    }

    public void RegularHit()
    {
        playerScore += scorePerNote * playerMultiplier;
        NoteHit();
        regularHits++;
    }

    public void GoodHit()
    {
        playerScore += scorePerGoodNote * playerMultiplier;
        NoteHit();
        goodHits++;
    }

    public void PerfectHit()
    {
        playerScore += scorePerPerfectNote * playerMultiplier;
        NoteHit();
        perfectHits++;
    }

    public void NoteMissed()
    {
        if (multiplierThresholds[playerMultiplier - 1] <= multiplierThreshold)
        {
            multiplierThreshold = 0;
            playerMultiplier++;
        }

        playerMultiplier = 1;
        multiplierThreshold = 0;
        multiplierDisplay.text = "Multiplier: x" + playerMultiplier;
        missedHits++;
    }
}
