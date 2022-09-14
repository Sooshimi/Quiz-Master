using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header ("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header ("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true; // set to true, so that line 54 fails on first Update()

    [Header ("Button Colours")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header ("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header ("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header ("Progress Bar")]
    [SerializeField] Slider progressBar;

    public bool isQuizComplete;

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    private void Start() 
    {
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction; // timer filler decreases per frame

        // quiz timer flow
        if (timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue) // checks if quiz is completed
            {
                isQuizComplete = true;
                return;
            }

            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnswerSelected) // if question not answered
        {
            DisplayAnswer(-1); // displays incorrect answer
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }

    void DisplayAnswer(int index) // answer button click logic and sprite changes
    {
        Image buttonImage;

        if (index == currentQuestion.GetCorrectAnswerIndex()) // if correct answer chosen
        {
            questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>(); // get correct button sprite
            buttonImage.sprite = correctAnswerSprite; // change correct button sprite to indicate correct answer
            scoreKeeper.IncrementCorrectAnswers();
        }
        else // if incorrect answer chosen
        {
            correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex(); // get index of correct answer
            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex); // get correct answer string
            questionText.text = "Sorry, the correct answer was:\n" + correctAnswer; // change button text to the correct answer string
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>(); // get the correct button sprite
            buttonImage.sprite = correctAnswerSprite; // change correct button sprite to indicate correct answer button
        }
    }

    void GetNextQuestion()
    {
        if (questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
            
    }

    void DisplayQuestion() // assigns texts to question and button boxes
    {
        TextMeshProUGUI buttonText;
        questionText.text = currentQuestion.GetQuestion(); // assigns the question text to the TMP question

        for (int i = 0; i < answerButtons.Length; i++)
        {
            // gets the TMP component of the child of the button
            buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            // assigns the answer text to the child TMP component of the button
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    void SetButtonState(bool state) // activate/deactivate buttons upon answer selection
    {
        Button button;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    private void SetDefaultButtonSprites() // resets button sprites so correct answer button sprite doesn't persist
    {
        Image buttonImage;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
}
