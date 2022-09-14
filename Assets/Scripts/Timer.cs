using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToCompleteQuestion = 30f;
    [SerializeField] float timeToShowCorrectAnswer = 10f;

    public bool loadNextQuestion;
    public bool isAnswerSelected;
    public float fillFraction;

    float timerValue;

    void Update()
    {
        UpdateTimer();
    }

    public void CancelTimer()
    {
        timerValue = 0;
    }

    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;

        if (isAnswerSelected) // if user selected an answer
        {
            if (timerValue > 0) // timer sprite filler to decrease
            {
                fillFraction = timerValue / timeToCompleteQuestion;
            }
            else // timer ran out
            {
                isAnswerSelected = false;
                timerValue = timeToShowCorrectAnswer;
            }
        }
        else // if user is still waiting to select an answer
        {
            if (timerValue > 0)
            {
                fillFraction = timerValue / timeToShowCorrectAnswer;
            }
            else
            {
                isAnswerSelected = true;
                timerValue = timeToCompleteQuestion;
                loadNextQuestion = true;
            }
        }
    }
}
