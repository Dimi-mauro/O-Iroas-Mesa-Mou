using System;
using System.Collections.Generic;
using UnityEngine;

public static class RunHistoryManager
{
    private const string SaveKey = "RUN_HISTORY";

    [Serializable]
    public class RunData
    {
        public List<string> answers =
            new List<string>();
    }

    [Serializable]
    public class RunHistory
    {
        public List<RunData> runs =
            new List<RunData>();
    }

    public static void SaveCurrentRun(string[] answers)
    {
        if (answers == null || answers.Length == 0)
        {
            Debug.LogWarning(
                "No answers available to save."
            );

            return;
        }

        RunHistory history = LoadHistory();

        RunData newRun = new RunData();

        for (int i = 0; i < answers.Length; i++)
        {
            string answer = answers[i];

            if (string.IsNullOrEmpty(answer))
            {
                answer = "Δεν δόθηκε απάντηση";
            }

            newRun.answers.Add(answer);
        }

        history.runs.Add(newRun);

        string json =
            JsonUtility.ToJson(history);

        PlayerPrefs.SetString(
            SaveKey,
            json
        );

        PlayerPrefs.Save();

        Debug.Log(
            "Run saved. Total runs: " +
            history.runs.Count
        );
    }

    public static RunHistory LoadHistory()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            return new RunHistory();
        }

        string json =
            PlayerPrefs.GetString(SaveKey);

        RunHistory history =
            JsonUtility.FromJson<RunHistory>(json);

        if (history == null)
        {
            history = new RunHistory();
        }

        if (history.runs == null)
        {
            history.runs =
                new List<RunData>();
        }

        return history;
    }

    public static int GetRunCount()
    {
        RunHistory history = LoadHistory();

        return history.runs.Count;
    }

    public static void ClearHistory()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();

        Debug.Log(
            "Run history cleared."
        );
    }
}