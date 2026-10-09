using System;
using UnityEngine;

public static class WeeklyPlayManager
{
    private const string LastCompletedDateKey = "LAST_COMPLETED_DATE";
    private const int DaysBetweenRuns = 7;

    // Ελέγχει αν επιτρέπεται νέο run.
    public static bool CanPlay()
    {
        if (!PlayerPrefs.HasKey(LastCompletedDateKey))
        {
            return true;
        }

        string savedDate =
            PlayerPrefs.GetString(LastCompletedDateKey);

        DateTime lastCompletedDate;

        if (!DateTime.TryParseExact(
                savedDate,
                "yyyy-MM-dd",
                null,
                System.Globalization.DateTimeStyles.None,
                out lastCompletedDate))
        {
            return true;
        }

        int daysPassed =
            (DateTime.Today - lastCompletedDate.Date).Days;

        return daysPassed >= DaysBetweenRuns;
    }

    // Πόσες ημέρες απομένουν.
    public static int GetDaysRemaining()
    {
        if (!PlayerPrefs.HasKey(LastCompletedDateKey))
        {
            return 0;
        }

        string savedDate =
            PlayerPrefs.GetString(LastCompletedDateKey);

        DateTime lastCompletedDate;

        if (!DateTime.TryParseExact(
                savedDate,
                "yyyy-MM-dd",
                null,
                System.Globalization.DateTimeStyles.None,
                out lastCompletedDate))
        {
            return 0;
        }

        int daysPassed =
            (DateTime.Today - lastCompletedDate.Date).Days;

        return Mathf.Max(
            0,
            DaysBetweenRuns - daysPassed
        );
    }

    // Καλείται ΜΟΝΟ όταν ολοκληρωθεί ένα run.
    public static void MarkRunCompleted()
    {
        string today =
            DateTime.Today.ToString("yyyy-MM-dd");

        PlayerPrefs.SetString(
            LastCompletedDateKey,
            today
        );

        PlayerPrefs.Save();

        Debug.Log(
            "Weekly run completed on: " + today
        );
    }

    // Μόνο για development/testing.
    public static void ResetWeeklyLimit()
    {
        PlayerPrefs.DeleteKey(
            LastCompletedDateKey
        );

        PlayerPrefs.Save();

        Debug.Log(
            "Weekly play limit has been reset."
        );
    }
}