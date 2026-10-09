using TMPro;
using UnityEngine;

public class ResultCardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text questionTitle;
    [SerializeField] private TMP_Text answerText;

    public void SetResult(string title, string answer)
    {
        questionTitle.text = title;
        answerText.text = "Απάντηση: " + answer;
    }
}