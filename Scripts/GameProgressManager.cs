using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    [Header("Managers")]
    [SerializeField] private VirusManager virusManager;

    [Header("Encounter Progress")]
    [SerializeField] private int gemsNeededForEncounter = 2;
    [SerializeField] private int totalVirusEncounters = 5;

    [Header("Level Complete")]
    [SerializeField]
    private string levelCompleteSceneName =
        "LevelComplete";

    [Header("Level Ending")]
    [SerializeField] private GameObject levelEndingPanel;
    [SerializeField] private GameObject fadeOut;

    [SerializeField] private float runningBeforeMessage = 1.5f;
    [SerializeField] private float messageDuration = 2f;
    [SerializeField] private float fadeDuration = 1f;

    private int encounterGemCount;
    private int currentVirusIndex;
    private bool encounterInProgress;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RegisterCollectedGem()
    {
        if (currentVirusIndex >= totalVirusEncounters)
        {
            return;
        }

        if (encounterInProgress)
        {
            return;
        }

        encounterGemCount++;

        Debug.Log(
            "Energy progress: " +
            encounterGemCount +
            "/" +
            gemsNeededForEncounter
        );

        if (encounterGemCount >= gemsNeededForEncounter)
        {
            encounterGemCount = 0;
            encounterInProgress = true;

            virusManager.StartVirusEncounter(
                currentVirusIndex,
                VirusEncounterFinished
            );
        }
    }

    private void VirusEncounterFinished()
    {
        currentVirusIndex++;
        encounterInProgress = false;

        Debug.Log(
            "Virus encounters completed: " +
            currentVirusIndex +
            "/" +
            totalVirusEncounters
        );

        if (currentVirusIndex >= totalVirusEncounters)
        {
            StartCoroutine(CompleteLevelSequence());
        }
    }

    private IEnumerator CompleteLevelSequence()
    {
        // Η Amy έχει ήδη αρχίσει να τρέχει ξανά.
        // Την αφήνουμε να συνεχίσει λίγο.
        yield return new WaitForSeconds(
            runningBeforeMessage
        );

        // Εμφάνιση μηνύματος ολοκλήρωσης.
        if (levelEndingPanel != null)
        {
            levelEndingPanel.SetActive(true);
        }

        yield return new WaitForSeconds(
            messageDuration
        );

        // Αποθήκευση Coins και Gems.
        int totalCoins =
            SaveLoad.loadedCoins + MasterInfo.coinCount;

        int totalGems =
            SaveLoad.loadedGems + MasterInfo.gemCount;

        PlayerPrefs.SetInt(
            "COINSAVE",
            totalCoins
        );

        PlayerPrefs.SetInt(
            "GEMSAVE",
            totalGems
        );

        PlayerPrefs.Save();

        // Fade Out.
        if (fadeOut != null)
        {
            fadeOut.SetActive(true);
        }

        yield return new WaitForSeconds(
            fadeDuration
        );

        // Μετάβαση στο LevelComplete.
        SceneManager.LoadScene(
            levelCompleteSceneName
        );
    }

}
