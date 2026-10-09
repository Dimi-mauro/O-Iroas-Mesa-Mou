using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public TMP_Text storyTitle;
    public TMP_Text storyText;
    public TMP_Text pageNumberText;
    public TMP_Text nextButtonText;

    private int currentPage = 0;

    private string[] titles =
    {
        "Γνωριμία με την Amy",
        "Mια απίστευτη περιπέτεια",
        "H αποστολή της Amy"
    };

    private string[] texts =
    {
        "Γεια! Είμαι η Amy!\n" +
        "Mου αρέσουν οι περιπέτειες, η εξερεύνηση και οι καινούργιες προκλήσεις.\n" +
        "Όμως αυτή τη φορά μου συνέβη κάτι που δεν περίμενα ποτέ...\n" +
        "Κάπως... μίκρυνα!\n" +
        "Και όταν άνοιξα ξανά τα μάτια μου, βρισκόμουν σε ένα μέρος που δεν είχα ξαναδεί!",

        "Σύντομα κατάλαβα ότι είχα γίνει τόσο μικρή, που είχα βρεθεί μέσα στο ανθρώπινο σώμα!\n" +
        "Γύρω μου υπήρχαν παράξενα μονοπάτια, διαφορετικές περιοχές και πράγματα που δεν είχα ξαναδεί.\n" +
        "Όμως δεν ήμουν μόνη...\n" +
        "Στον δρόμο μου εμφανίστηκαν κάποιοι κακοί ιοί που προσπαθούν να με εμποδίσουν να συνεχίσω!",

        "Xρειάζομαι τη βοήθειά σου!\n" +
        "Για να συνεχίσω το ταξίδι μου, πρέπει να περάσω από όλα τα εμπόδια και να αντιμετωπίσω τους ιούς που θα συναντήσω.\n" +
        "Όταν εμφανίζεται ένας ιός, θα σου κάνω μια ερώτηση για το πώς αισθάνεσαι εσύ.\n" +
        "Κάθε απάντηση είναι σημαντική.\n" +
        "H δύναμή μας είναι η ειλικρίνειά σου!"
    };

    void Start()
    {
        ShowPage();
    }

    void ShowPage()
    {
        storyTitle.text = titles[currentPage];
        storyText.text = texts[currentPage];
        pageNumberText.text = (currentPage + 1) + " / 3";

        if (currentPage == 2)
        {
            nextButtonText.text = "ΠΑMΕ!";
        }
        else
        {
            nextButtonText.text = "Επόμενο";
        }
    }

    public void NextPage()
    {
        if (currentPage < 2)
        {
            currentPage++;
            ShowPage();
        }
        else
        {
            SceneManager.LoadScene("StageSelect");
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage();
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}