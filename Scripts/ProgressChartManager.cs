using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgressChartManager : MonoBehaviour
{
    [Header("Chart")]
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private TMP_Text chartTitle;

    [Header("Buttons")]
    [SerializeField] private Button painButton;
    [SerializeField] private Button nauseaButton;
    [SerializeField] private Button fatigueButton;
    [SerializeField] private Button anxietyButton;
    [SerializeField] private Button sadnessButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button clearHistoryButton;

    [Header("Point Settings")]
    [SerializeField] private Sprite pointSprite;
    [SerializeField] private float pointSize = 18f;
    [SerializeField] private float lineThickness = 6f;

    private readonly string[] categoryNames =
    {
        "Πόνος",
        "Ναυτία",
        "Κόπωση",
        "Άγχος",
        "Στεναχώρια"
    };

    private readonly List<GameObject> createdObjects =
        new List<GameObject>();

    private void Start()
    {
        painButton.onClick.AddListener(() => ShowCategory(0));
        nauseaButton.onClick.AddListener(() => ShowCategory(1));
        fatigueButton.onClick.AddListener(() => ShowCategory(2));
        anxietyButton.onClick.AddListener(() => ShowCategory(3));
        sadnessButton.onClick.AddListener(() => ShowCategory(4));

        backButton.onClick.AddListener(ReturnToLevelComplete);
        clearHistoryButton.onClick.AddListener(ClearHistory);

        ShowCategory(0);
    }

    private void ShowCategory(int questionIndex)
    {
        ClearGraph();

        chartTitle.text =
            "H πορεία των απαντήσεών μου – " +
            categoryNames[questionIndex];

        RunHistoryManager.RunHistory history =
            RunHistoryManager.LoadHistory();

        if (history.runs == null || history.runs.Count == 0)
        {
            Debug.LogWarning("No saved runs found.");
            return;
        }

        float graphWidth = graphContainer.rect.width;
        float graphHeight = graphContainer.rect.height;

        float paddingX = 50f;
        float paddingY = 30f;

        int runCount = history.runs.Count;

        Vector2? previousPoint = null;

        for (int i = 0; i < runCount; i++)
        {
            if (history.runs[i].answers == null ||
                history.runs[i].answers.Count <= questionIndex)
            {
                continue;
            }

            string answer =
                history.runs[i].answers[questionIndex];

            int value = AnswerToValue(answer);

            float xPosition;

            if (runCount == 1)
            {
                xPosition = graphWidth / 2f;
            }
            else
            {
                xPosition =
                    paddingX +
                    i *
                    ((graphWidth - paddingX * 2f) /
                    (runCount - 1));
            }

            float normalizedValue =
                (value - 1) / 3f;

            float yPosition =
                paddingY +
                normalizedValue *
                (graphHeight - paddingY * 2f);

            Vector2 currentPoint =
                new Vector2(
                    xPosition,
                    yPosition
                );

            CreatePoint(currentPoint);

            CreateRunLabel(
                currentPoint,
                i + 1
            );

            if (previousPoint.HasValue)
            {
                CreateLine(
                    previousPoint.Value,
                    currentPoint
                );
            }

            previousPoint = currentPoint;
        }
    }

    private int AnswerToValue(string answer)
    {
        switch (answer)
        {
            case "Πολύ συχνά":
                return 4;

            case "Συχνά":
                return 3;

            case "Όχι πολύ συχνά":
                return 2;

            case "Καθόλου":
                return 1;

            default:
                return 1;
        }
    }

    private void CreatePoint(Vector2 position)
    {
        GameObject point =
            new GameObject(
                "Point",
                typeof(Image)
            );

        point.transform.SetParent(
            graphContainer,
            false
        );

        RectTransform rectTransform =
            point.GetComponent<RectTransform>();

        rectTransform.anchorMin =
            new Vector2(0f, 0f);

        rectTransform.anchorMax =
            new Vector2(0f, 0f);

        rectTransform.pivot =
            new Vector2(0.5f, 0.5f);

        rectTransform.anchoredPosition =
            position;

        rectTransform.sizeDelta =
            new Vector2(
                pointSize,
                pointSize
            );

        Image image =
            point.GetComponent<Image>();

        image.sprite = pointSprite;

        createdObjects.Add(point);
    }

    private void CreateLine(
        Vector2 start,
        Vector2 end)
    {
        GameObject line =
            new GameObject(
                "Line",
                typeof(Image)
            );

        line.transform.SetParent(
            graphContainer,
            false
        );

        RectTransform rectTransform =
            line.GetComponent<RectTransform>();

        Vector2 direction =
            end - start;

        float distance =
            direction.magnitude;

        rectTransform.anchorMin =
            new Vector2(0f, 0f);

        rectTransform.anchorMax =
            new Vector2(0f, 0f);

        rectTransform.sizeDelta =
            new Vector2(
                distance,
                lineThickness
            );

        rectTransform.anchoredPosition =
            start +
            direction * 0.5f;

        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x
            ) *
            Mathf.Rad2Deg;

        rectTransform.localEulerAngles =
            new Vector3(
                0f,
                0f,
                angle
            );

        createdObjects.Add(line);
    }

    private void CreateRunLabel(
        Vector2 pointPosition,
        int runNumber)
    {
        GameObject label =
            new GameObject(
                "RunLabel",
                typeof(TextMeshProUGUI)
            );

        label.transform.SetParent(
            graphContainer,
            false
        );

        RectTransform rectTransform =
            label.GetComponent<RectTransform>();

        rectTransform.anchorMin =
            new Vector2(0f, 0f);

        rectTransform.anchorMax =
            new Vector2(0f, 0f);

        rectTransform.pivot =
            new Vector2(0.5f, 1f);

        rectTransform.anchoredPosition =
            new Vector2(
                pointPosition.x,
                10f
            );

        rectTransform.sizeDelta =
            new Vector2(
                100f,
                40f
            );

        TextMeshProUGUI text =
            label.GetComponent<TextMeshProUGUI>();

        text.text =
            "Run " + runNumber;

        text.alignment =
            TextAlignmentOptions.Center;

        text.fontSize = 24f;

        createdObjects.Add(label);
    }

    private void ClearGraph()
    {
        foreach (GameObject obj in createdObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }

        createdObjects.Clear();
    }

    private void ReturnToLevelComplete()
    {
        SceneManager.LoadScene(
            "LevelComplete"
        );
    }
    private void ClearHistory()
    {
        RunHistoryManager.ClearHistory();

        ClearGraph();

        chartTitle.text = "Δεν υπάρχουν αποθηκευμένα παιχνίδια";

        Debug.Log("All saved runs have been deleted.");
    }
}