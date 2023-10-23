using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RunnerCollision : MonoBehaviour
{
    public textChange textChangeObject;
    public AnswerPositionHandler answerPositionHandler;
    public static int score = 0;
    private int lives = 3; 
    public TextMeshProUGUI scoreValue;

    //TODO: Change with hearts if get the prefab hearts 
    public TextMeshProUGUI livesValue;

    bool debounce = false;

    public TMP_Text correctIncorrect;
    public Animator correctIncorrectAnimator;

    public EndActivityHandler endActivityHandler;

    private Dictionary<string, Color> textColors = new Dictionary<string, Color>()
    {
        { "Correct", new Color(0, 203, 255)},
        { "Incorrect", new Color(255, 0, 12) }
    };

    public Animator endScreenAnimator;
    public Animator playerAnimator;
    public Animator roadAnimator;

    public GameObject answers;

    public TMP_Text scoreText;
    public TMP_Text highestScoreText;
    public TMP_Text newHighestScoreText;
    public TMP_Text timeElapsedText;
    public Button playAgainButton;
    public Button returnHomeButton;

    public SFXHandler sfxHandler;

    private void Start()
    {
        score = 0;
        scoreValue.text = score.ToString();
        livesValue.text = lives.ToString();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (debounce == true) return;

        StartCoroutine(ToggleDebounce());

        answerPositionHandler.ResetPositions();

        if (col.gameObject.tag == "Answer")
        {
            SFXHandler.PlaySound(sfxHandler.success);
            score += 100;
            correctIncorrect.color = textColors["Correct"];
            FlashText(correctIncorrect, "CORRECT!");
            Debug.Log("You got it! score: " +score);
            scoreValue.text = score.ToString();
        }
        else
        {
            correctIncorrect.color = textColors["Incorrect"];
            FlashText(correctIncorrect, "INCORRECT!");
            Debug.Log("Oops, wrong answer :(");
            lives--;
            livesValue.text = lives.ToString();

            if (lives == 0)
            {
                Debug.Log("Game over");
                CodeRunnerGameHandler.gameOver = true;
                //Time.timeScale = 0; //Remember to re init this to 1 to make stuff moving again

                int highestScore = 0;
                if (PlayerPrefs.HasKey("CodeRunner_HighestScore"))
                {
                    highestScore = PlayerPrefs.GetInt("CodeRunner_HighestScore");
                    if (score > highestScore)
                    {
                        newHighestScoreText.text = "NEW!";
                        highestScore = score;
                        PlayerPrefs.SetInt("CodeRunner_HighestScore", score);
                    }
                } else
                {
                    highestScore = score;
                }

                scoreText.text = score.ToString();
                timeElapsedText.text = Utils.ConvertToMS(CodeRunnerGameHandler.elapsedTime);
                highestScoreText.text = highestScore.ToString();

                playerAnimator.SetTrigger("GameOver");
                roadAnimator.SetTrigger("GameOver");
                endScreenAnimator.SetTrigger("GameOver");

                answers.SetActive(false);

                playAgainButton.onClick.AddListener(() =>
                {
                    DatabaseHandler.IncrementStat("gameXp", score / 10);
                    DatabaseHandler.IncrementStat("gamesPlayed", 1);
                    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                });

                returnHomeButton.onClick.AddListener(() =>
                {
                    endActivityHandler.EndActivity(endActivityHandler.gameObject, RunnerCollision.score / 10, CodeRunnerGameHandler.elapsedTime);
                });

                //Game Over screen here.
            }
        }

    }

    void FlashText(TMP_Text textLabel, string textToDisplay)
    {
        textLabel.text = textToDisplay;
        correctIncorrectAnimator.SetTrigger("Flash");
    }

    IEnumerator ToggleDebounce()
    {
        debounce = true;
        yield return new WaitForSeconds(1f);
        debounce = false;
    }
}
