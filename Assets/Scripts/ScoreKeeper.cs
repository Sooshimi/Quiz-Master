using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int correctAnswers = 0;
    int questionsSeen = 0;

    public int GetCorrectAnswers()
    {
        return correctAnswers;
    }

    public void IncrementCorrectAnswers()
    {
        correctAnswers++;
    }

    public int GetQuestionsSeen()
    {
        return questionsSeen;
    }

    public void IncrementQuestionsSeen()
    {
        questionsSeen++;
    }

    public int CalculateScore()
    {
        // int/int will give us an int, and we can a proper % (e.g. 3/4 = 0.75, but as an int that will be 0)
        // so firstly, int/float = float
        // but CalculateScore() must return an int, so we use Mathf.RoundToInt() to round to an int
        return Mathf.RoundToInt(correctAnswers / (float)questionsSeen * 100);
    }
}
