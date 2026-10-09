using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    [Header("Question UI")]
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private TMP_Text questionText;

    [Header("Answer Buttons")]
    [SerializeField] private Button answerButton1;
    [SerializeField] private Button answerButton2;
    [SerializeField] private Button answerButton3;
    [SerializeField] private Button answerButton4;

    [Header("Amy Animation")]
    [SerializeField] private Animator amyAnimator;

    private readonly string[] questions =
    {
        "Ερώτηση 1\n\n" +
        "Για να νικήσεις τον πρώτο Ιό, πες μας:\n" +
        "Πόσο συχνά νιώθεις το σώμα σου ή τα κοκαλάκια σου να πονάνε;",

        "Ερώτηση 2\n\n" +
        "Ο κακός ιός προσπαθεί να σου χαλάσει την όρεξη!\n" +
        "Πόσο συχνά νιώθεις ανακάτωμα (ναυτία) στο στομάχι σου;",

        "Ερώτηση 3\n\n" +
        "Πόσο συχνά νιώθεις ότι η ενέργειά σου (power) " +
        "αδειάζει πολύ γρήγορα και κουράζεσαι;",

        "Ερώτηση 4\n\n" +
        "Πριν αντιμετωπίσεις τον Ιό:\n" +
        "Πόσο συχνά νιώθεις ανησυχία ή φόβο όταν σκέφτεσαι " +
        "τις επόμενες εξετάσεις ή τις βελόνες των γιατρών;",

        "Ερώτηση 5\n\n" +
        "Οι ιοί προσπαθούν να μας ρίξουν τη διάθεση.\n" +
        "Πόσο συχνά νιώθεις στεναχώρια ή θυμό επειδή πρέπει να είσαι " +
        "στο νοσοκομείο αντί να παίζεις με τους φίλους σου;"
    };

    private readonly string[] answerOptions =
    {
        "Πολύ συχνά",
        "Συχνά",
        "Όχι πολύ συχνά",
        "Καθόλου"
    };

    public static string[] selectedAnswers = new string[5];
    public static bool currentRunSaved = false;

    private int currentQuestionIndex;
    private Action answerSelectedCallback;
    private bool answerAlreadySelected;

    private void Start()
    {
        // Νέο gameplay run.
        currentRunSaved = false;

        // Καθαρίζουμε τις απαντήσεις του προηγούμενου run.
        for (int i = 0; i < selectedAnswers.Length; i++)
        {
            selectedAnswers[i] = "";
        }

        questionPanel.SetActive(false);

        SetButtonText(answerButton1, answerOptions[0]);
        SetButtonText(answerButton2, answerOptions[1]);
        SetButtonText(answerButton3, answerOptions[2]);
        SetButtonText(answerButton4, answerOptions[3]);

        answerButton1.onClick.AddListener(
            () => SelectAnswer(answerOptions[0])
        );

        answerButton2.onClick.AddListener(
            () => SelectAnswer(answerOptions[1])
        );

        answerButton3.onClick.AddListener(
            () => SelectAnswer(answerOptions[2])
        );

        answerButton4.onClick.AddListener(
            () => SelectAnswer(answerOptions[3])
        );
    }

    public void ShowQuestion(
        int questionIndex,
        Action onAnswerSelected)
    {
        if (questionIndex < 0 || questionIndex >= questions.Length)
        {
            Debug.LogError(
                "Question index " + questionIndex + " does not exist."
            );

            return;
        }

        currentQuestionIndex = questionIndex;
        answerSelectedCallback = onAnswerSelected;
        answerAlreadySelected = false;

        questionText.text = questions[currentQuestionIndex];
        questionPanel.SetActive(true);

        // Amy changes from Running to Breathing Idle.
        if (amyAnimator != null)
        {
            amyAnimator.updateMode =
                AnimatorUpdateMode.UnscaledTime;

            amyAnimator.Play("Breathing Idle");
        }

        // Pause the rest of the game.
        Time.timeScale = 0f;
    }

    private void SelectAnswer(string selectedAnswer)
    {
        if (answerAlreadySelected)
        {
            return;
        }

        answerAlreadySelected = true;

        selectedAnswers[currentQuestionIndex] = selectedAnswer;

        Debug.Log(
            "Question " +
            (currentQuestionIndex + 1) +
            " answer: " +
            selectedAnswer
        );

        questionPanel.SetActive(false);

        // Resume the game.
        Time.timeScale = 1f;

        // Amy returns to Running.
        if (amyAnimator != null)
        {
            amyAnimator.updateMode =
                AnimatorUpdateMode.Normal;

            amyAnimator.Play("Running");
        }

        Action callback = answerSelectedCallback;
        answerSelectedCallback = null;

        callback?.Invoke();
    }

    private void SetButtonText(Button button, string text)
    {
        TMP_Text buttonText =
            button.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            buttonText.text = text;
        }
    }
}