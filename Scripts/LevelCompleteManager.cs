using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteManager : MonoBehaviour
{
    [Header("Result Cards")]
    [SerializeField] private ResultCardUI[] resultCards;

    [Header("Scene Settings")]
    [SerializeField]
    private string mainMenuSceneName = "MainMenu";

    private readonly string[] questionTitles =
    {
        "Ερώτηση 1 – Πόνος",
        "Ερώτηση 2 – Ναυτία",
        "Ερώτηση 3 – Κόπωση",
        "Ερώτηση 4 – Άγχος για το Νοσοκομείο",
        "Ερώτηση 5 – Θλίψη / Απομόνωση"
    };

    private void Start()
    {
        SaveRun();
        ShowResults();
    }

    private void SaveRun()
    {
        if (QuestionManager.currentRunSaved)
        {
            return;
        }

        RunHistoryManager.SaveCurrentRun(
            QuestionManager.selectedAnswers
        );

        WeeklyPlayManager.MarkRunCompleted();
        QuestionManager.currentRunSaved = true;
    }

    private void ShowResults()
    {
        if (resultCards == null ||
            resultCards.Length < questionTitles.Length)
        {
            Debug.LogError(
                "Not enough result cards have been assigned."
            );

            return;
        }

        for (int i = 0;
             i < questionTitles.Length;
             i++)
        {
            string answer =
                QuestionManager.selectedAnswers[i];

            if (string.IsNullOrEmpty(answer))
            {
                answer =
                    "Δεν δόθηκε απάντηση";
            }

            resultCards[i].SetResult(
                questionTitles[i],
                answer
            );
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(
            mainMenuSceneName
        );
    }
    public void OpenProgressChart()
    {
        SceneManager.LoadScene("ProgressChart");
    }
}