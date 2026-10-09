using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField] GameObject fadeOut;
    [SerializeField] GameObject bounceText;
    [SerializeField] GameObject bigButton;
    [SerializeField] GameObject animCam;
    [SerializeField] GameObject mainCam;
    [SerializeField] GameObject menuControls;
    [SerializeField] AudioSource buttonSelect;
    public static bool hasClicked;
    [SerializeField] GameObject staticCam;
    [SerializeField] GameObject fadeIn;

    [SerializeField] int loadedCoins;
    [SerializeField] int loadedGems;
    [SerializeField] GameObject coinDisplay;
    [SerializeField] GameObject gemDisplay;

    [Header("Weekly Play UI")]
    [SerializeField] private GameObject weeklyLimitPanel;
    [SerializeField] private TMP_Text weeklyLimitText;



    void Start()
    {
        //loadedCoins = PlayerPrefs.GetInt("COINSAVE");
        //loadedGems = PlayerPrefs.GetInt("GEMSAVE");
        //coinDisplay.GetComponent<TMPro.TMP_Text>().text = "" + loadedCoins;
        //gemDisplay.GetComponent<TMPro.TMP_Text>().text = "" + loadedGems;


        StartCoroutine(FadeInTurnOff());
        if (hasClicked == true)
        {
            staticCam.SetActive(true);
            animCam.SetActive(false);
            menuControls.SetActive(true);
            bounceText.SetActive(false);
            bigButton.SetActive(false);
        }
    }

    void Update()
    {

    }

    public void MenuBeginButton()
    {
        StartCoroutine(AnimCam());
    }


    public void StartGame()
    {
        if (WeeklyPlayManager.CanPlay())
        {
            StartCoroutine(StartButton());
        }
        else
        {
            ShowWeeklyLimitMessage();
        }
    }
    public void OpenAmyStory()
    {
        StartCoroutine(AmyStoryButton());
    }

    IEnumerator StartButton()
    {
        buttonSelect.Play();
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }

    IEnumerator AmyStoryButton()
    {
        buttonSelect.Play();
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("AmyStory");
    }

    IEnumerator AnimCam()
    {
        animCam.GetComponent<Animator>().Play("AnimMenuCam");
        bounceText.SetActive(false);
        bigButton.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        fadeIn.SetActive(false);
        mainCam.SetActive(true);
        animCam.SetActive(false);
        menuControls.SetActive(true);
        hasClicked = true;
    }

    IEnumerator FadeInTurnOff()
    {
        yield return new WaitForSeconds(0.05f);
        loadedCoins = PlayerPrefs.GetInt("COINSAVE");
        loadedGems = PlayerPrefs.GetInt("GEMSAVE");
        coinDisplay.GetComponent<TMPro.TMP_Text>().text = "" + loadedCoins;
        gemDisplay.GetComponent<TMPro.TMP_Text>().text = "" + loadedGems;
        yield return new WaitForSeconds(1);
        fadeIn.SetActive(false);
    }

    public void OpenInformation()
    {
        StartCoroutine(InformationButton());
    }

    IEnumerator InformationButton()
    {
        buttonSelect.Play();
        fadeOut.SetActive(true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Information");
    }

    private void ShowWeeklyLimitMessage()
    {
        int daysRemaining =
            WeeklyPlayManager.GetDaysRemaining();

        weeklyLimitText.text =
            "H αποστολή αυτής της εβδομάδας ολοκληρώθηκε!\n\n" +
            "Mπορείς να ξεκινήσεις μια νέα αποστολή σε " +
            daysRemaining +
            (daysRemaining == 1 ? " ημέρα." : " ημέρες.");

        weeklyLimitPanel.SetActive(true);
    }

    public void CloseWeeklyLimitMessage()
    {
        weeklyLimitPanel.SetActive(false);
    }

    public void DeveloperResetWeeklyLimit()
    {
        WeeklyPlayManager.ResetWeeklyLimit();

        Debug.Log(
            "Developer reset completed. " +
            "The weekly limit has been removed."
        );
    }

}
