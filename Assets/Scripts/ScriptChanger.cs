using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScriptChanger : MonoBehaviour
{
    public Button nextButton;
    public Button backButton;
    public TextMeshProUGUI dialogBox;
    public Image npcImage;
    public GameObject imageHolder;
    public TMP_Text npcName;

    public GameObject question1Holder;
    public GameObject question2Holder;

    public GameObject[] questionHolders;

    public Animator animator;
    public Animator endScreenAnimator;

    public TMP_Text scoreText;
    public TMP_Text highestScoreText;
    public TMP_Text timeElapsedText;

    public EndActivityHandler endActivityHandler;

    public Button playAgainButton;
    public Button returnHomeButton;

    public NPCImageHandler npcImageHandler;

    public SFXHandler sfxHandler;

    private int textPosition = 0;

    string[] script = { "Good morning agent, at precisely o hundred hours today, the US President was kidnapped by traitor agents.",
        "We received intel that Doctor Exception is the mastermind behind the recent kidnapping of the President.",
        "Not only that, but he's also left a series of cryptic coding questions behind.",
        "We believe once these questions are solved, it will lead us to the location of the President.",
        "As our best agent, we need you to rescue the President and bring Doctor Exception and his agents to justice.",
        "Good luck agent, over and out.",
        "Greetings Agent, my name is Doctor Method and I am the lead scientist of the OOP Agency",
        "I've been tasked with helping you through these puzzles to save the President from that vile Doctor Exception. Let's look at the first one now.",
        "Ahh that tricky Doctor Exception, this line of code is creating what's known as a variable.",
        "Variables allow us to assign numeric or String values into a placeholder using the assignment (=) operator.",
        "This placeholder is known as a variable and can be whatever name you want it to be.",
        "We can then use that variable throughout the program. But a very important part is missing. Primitive Data Types.",
        "Primitive Data Types allow us to specify what data type a variable will hold in Java and go before the variable name.",
        "The major data types are int which hold whole numbers, double for decimal numbers, and String for strings or words/sentences.",
        @"Note that the word String needs to be written with a capital s and needs to have double quotes around the word eg: String word = ""hello"";",
        "An example of creating an int variable will be as follows: int num = 10;",
        "Lastly, an example of creating a double variable will be: double doubleNum = 5.4;",
        "With all this being said, solve the coding question below. Good luck agent.",
        "Great work, agent! Wait... what's this?",
        "Oh my god, looks like Dr. Exception wants $2000.75 in 48 hours in exchange for the President, and the swap is to take place at 404 Overflow drive at 6:00pm.",
        "Yeah right... We'll be getting him back without paying a dime.",
        "Um ahem…. Great work agent, we’ll get this information back to the boss.",
        "Hmmmmm…. I wonder what the .75 is for, so randomly specific…… Nonetheless, over and out.",
        "Later on...",
        "Welcome back, agent. My name is Miss Sentinel. I hear you did a great job at decoding the coding question left behind by Doctor Exception and his mercenaries.",
        "I've been sent here to report that there's more that needs to be done if we want to get the President back in one piece.",
        "If you want any shot at beating Doctor Exception, you'll need to learn this valuable skill next: Arithmetic operators.",
        "You can use programming languages to solve math just as a calculator would! And it’s just as simple.",
        "Let’s define a variable to do some math with.\nint ourNumber = 10;",
        "Notice how we wrote the word “int” in front of our variable name? Just like we talked about earlier, you need to define the type of the variable before you declare it!",
        "In our case, we’re dealing with whole numbers, so we will use an int! Then, we store the value of 10 inside it. This will surely help us against Doctor Exception so pay close attention!",
        "What if we want to add onto our 10? Not a problem, let’s see how to do it!\nourNumber = ourNumber + 5;",
        "I know this may look tricky at first, but it’s a lot simpler than you may think. Let’s take a look at what’s going on in that line of code.",
        "First, we have “ourNumber = “. This means we’ll be storing whatever’s on the right of that equals sign into our variable ourNumber.",
        "Now that we covered the left side, let’s cover the right side.",
        "On the right, we have “ourNumber + 5;”. As you already know, ourNumber starts out holding 10. So, the computer reads this line of code as “ourNumber = 10 + 5;”. Isn’t that much easier to understand?",
        "Now, our variable ourNumber will hold the value of 15!",
        "We can do a lot more than just addition, though. Let’s try subtraction now.\nourNumber = ourNumber - 10;",
        "Now, our variable stores the value of 5! Great!",
        "As you already know, there’s more to math than just addition and subtraction. Let’s try multiplication now.\nourNumber = ourNumber * 2;",
        "Now, our variable will hold the value of 10!",
        "The last one we’ll cover for this lesson is division. Let’s get our variable back to 5, shall we?\nourNumber = ourNumber / 2;",
        "And there we have it, ourNumber is once again holding the value of 5! Great work! It’s time for an exercise to prove you have what it takes to handle anything Doctor Exception has to throw at us!",
        "Amazing work, agent! Report back to HQ and we'll discuss what our next plan of action is!",
        "To be continued...",
    };

    int[] questionBarrierPoints =
    {
        17,
        42,
    };

    string[] question1Answers =
    {
        "int",
        "double",
        "int",
        "String",
        "double",
        "String",
    };

    string[] question2Answers =
    {
        "4",
        "4",
        "3",
        "3",
        "50",
        "100",
    };

    string[] transitionTexts =
    {
        "Later on...",
        "To be continued...",
    };

    // Start is called before the first frame update
    void Start()
    {
       
        nextButton.onClick.AddListener(ClickedNextButton);
        backButton.onClick.AddListener(ClickedBackButton);
        backButton.interactable = false;

        dialogBox.text = script[textPosition];
        Debug.Log(textPosition);

        npcImageHandler.ApplyTexture(npcImage, npcImageHandler.images[0]);
        npcName.text = "The Director";
    }

    void ClickedBackButton()
    {
        if (textPosition > 0)
        {
            textPosition--;
            dialogBox.text = script[textPosition];
            nextButton.interactable = true;
        }

        if (textPosition == 0) {
            backButton.interactable = false;
        }

        if (textPosition == 5) {
            npcImageHandler.ApplyTexture(npcImage, npcImageHandler.images[0]);
            npcName.text = "The Director";
        }

        if (textPosition == 23)
        {
            npcImageHandler.ApplyTexture(npcImage, npcImageHandler.images[1]);
            npcName.text = "Doctor Method";
        }

        if (transitionTexts.Contains(script[textPosition]))
        {
            imageHolder.SetActive(false);
            npcName.gameObject.SetActive(false);
        } else
        {
            imageHolder.SetActive(true);
            npcName.gameObject.SetActive(true);
        }

        if (textPosition == 7 || textPosition == 41) {
            animator.SetBool("ShowAnswer", false);
        }
    }

    void ClickedNextButton() {
        if (questionBarrierPoints.Contains(textPosition + 1) == true)
        {
            nextButton.interactable = false;
        }

        if (textPosition + 1 == 8)
        {
            foreach (GameObject obj in questionHolders)
            {
                obj.SetActive(false);
            }

            question1Holder.SetActive(true);
        }
        else if (textPosition + 1 == 42)
        {
            foreach (GameObject obj in questionHolders)
            {
                obj.SetActive(false);
            }

            question2Holder.SetActive(true);
        }

        if (textPosition == 0) {
            backButton.interactable = true;
        }

        if (textPosition == 7 || textPosition == 41) {
            Debug.Log("Here");
            animator.SetBool("ShowAnswer", true);
        }

        if (textPosition == 5) {
            npcImageHandler.ApplyTexture(npcImage, npcImageHandler.images[1]);
            npcName.text = "Doctor Method";
        }

        if (textPosition == 23)
        {
            npcImageHandler.ApplyTexture(npcImage, npcImageHandler.images[3]);
            npcName.text = "Miss Sentinel";
        }

        if (transitionTexts.Contains(script[textPosition + 1]))
        {
            imageHolder.SetActive(false);
            npcName.gameObject.SetActive(false);
        }
        else
        {
            imageHolder.SetActive(true);
            npcName.gameObject.SetActive(true);
        }

        if (textPosition == script.Length - 1)
        {
            //Debug.Log("End");
            //nextButton.interactable = false;
            //return;
        }
        else
        {
            //Debug.Log("Button Clicked");
            Debug.Log("Else TextPosition 1: " + textPosition);
            textPosition++;
            dialogBox.text = script[textPosition];
            Debug.Log(" ElseTextPosition 2: " + textPosition);

            if (textPosition == script.Length - 1)
            {
                Debug.Log("End");
                nextButton.interactable = false;

                StoryHandler.gameOver = true;
                scoreText.text = "20,000";
                timeElapsedText.text = Utils.ConvertToMS(StoryHandler.elapsedTime);
                highestScoreText.text = "20,000";

                endScreenAnimator.gameObject.SetActive(true);
                endScreenAnimator.SetTrigger("GameOver");

                playAgainButton.onClick.AddListener(() =>
                {
                    DatabaseHandler.IncrementStat("storyXp", 2000);
                    DatabaseHandler.IncrementStat("storiesRead", 1);
                    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                });

                returnHomeButton.onClick.AddListener(() =>
                {
                    endActivityHandler.EndActivity(endActivityHandler.gameObject, 2000, StoryHandler.elapsedTime);
                });
            }
        }
    }

    public void OnCommitButtonClicked()
    {
        Debug.Log(textPosition);
        List<GameObject> descendants = new List<GameObject>();
        bool correct = true;

        if (textPosition <= 17)
        {
            GetDescendantsByNameRecursive(question1Holder.transform, "Text", descendants);
            for (int i = 0; i < descendants.Count; i++)
            {
                TMP_Text text = descendants[i].GetComponent<TMP_Text>();
                if (text != null)
                {
                    if (text.text.Substring(0, text.text.Length - 1) != question1Answers[i])
                    {
                        correct = false;
                        break;
                    }
                }
            }
        } else if (textPosition <= 42)
        {
            GetDescendantsByNameRecursive(question2Holder.transform, "Text", descendants);
            for (int i = 0; i < descendants.Count; i++)
            {
                TMP_Text text = descendants[i].GetComponent<TMP_Text>();
                if (text != null)
                {
                    if (text.text.Substring(0, text.text.Length - 1) != question2Answers[i])
                    {
                        correct = false;
                        break;
                    }
                }
            }
        }

        if (correct == true)
        {
            SFXHandler.PlaySound(sfxHandler.success);
            Debug.Log("Congrats, you got it!");

            if (textPosition == 0)
            {
                backButton.interactable = true;
            }

            if (textPosition == 7)
            {
                Debug.Log("Here");
                animator.SetBool("ShowAnswer", true);
            }

            if (textPosition == 5)
            {
                npcImageHandler.ApplyTexture(npcImage, npcImageHandler.images[1]);
                npcName.text = "Doctor Method";
            }

            if (textPosition == 23)
            {
                npcImageHandler.ApplyTexture(npcImage, npcImageHandler.images[3]);
                npcName.text = "Miss Sentinel";
            }

            if (transitionTexts.Contains(script[textPosition + 1]))
            {
                imageHolder.SetActive(false);
                npcName.gameObject.SetActive(false);
            }
            else
            {
                imageHolder.SetActive(true);
                npcName.gameObject.SetActive(true);
            }

            if (textPosition == script.Length - 1)
            {
                //Debug.Log("End");
                //nextButton.interactable = false;
                //return;
            }
            else
            {
                //Debug.Log("Button Clicked");
                Debug.Log("Else TextPosition 1: " + textPosition);
                if (textPosition <= 17)
                {
                    textPosition = 17;
                }
                textPosition++;
                dialogBox.text = script[textPosition];
                animator.SetBool("ShowAnswer", false);
                nextButton.interactable = true;
                Debug.Log(" ElseTextPosition 2: " + textPosition);

                if (textPosition == script.Length - 1)
                {
                    Debug.Log("End");
                    nextButton.interactable = false;

                    StoryHandler.gameOver = true;
                    scoreText.text = "20,000";
                    timeElapsedText.text = Utils.ConvertToMS(StoryHandler.elapsedTime);
                    highestScoreText.text = "20,000";

                    endScreenAnimator.SetTrigger("GameOver");

                    playAgainButton.onClick.AddListener(() =>
                    {
                        DatabaseHandler.IncrementStat("storyXp", 2000);
                        DatabaseHandler.IncrementStat("storiesRead", 1);
                        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                    });

                    returnHomeButton.onClick.AddListener(() =>
                    {
                        endActivityHandler.EndActivity(endActivityHandler.gameObject, 2000, StoryHandler.elapsedTime);
                    });
                }
            }
        }
        else
        {
            Debug.Log("Sorry, that doesn't look quite right!");
            dialogBox.text = "Sorry, that doesn't look quite right!";
        }
    }
    private static void GetDescendantsByNameRecursive(Transform parent, string name, List<GameObject> descendants)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                descendants.Add(child.gameObject);
            }
            GetDescendantsByNameRecursive(child, name, descendants);
        }
    }
}