using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class VirusManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform playerTransform;

    [Header("Question System")]
    [SerializeField] private QuestionManager questionManager;

    [Header("Virus Setup")]
    [SerializeField] private Transform virusSpawnPoint;
    [SerializeField] private GameObject[] virusPrefabs;

    [Header("Announcement UI")]
    [SerializeField] private CanvasGroup announcementCanvasGroup;
    [SerializeField] private TMP_Text announcementText;
    [SerializeField]
    private string announcementMessage =
        "A Virus Appeared!";

    [Header("Announcement Timing")]
    [SerializeField] private float announcementFadeDuration = 0.4f;
    [SerializeField] private float announcementVisibleTime = 1.2f;

    [Header("Virus Audio")]
    [SerializeField] private AudioSource virusAudioSource;
    [SerializeField] private AudioClip[] virusSpawnSounds;

    [Header("Default Animation Names")]
    [SerializeField] private string tauntAnimationName = "Taunt";
    [SerializeField] private string idleAnimationName = "IdleBattle";
    [SerializeField] private string getHitAnimationName = "GetHit";
    [SerializeField] private string dieAnimationName = "Die";

    [Header("Animation Timing")]
    [SerializeField] private float appearDuration = 0.5f;
    [SerializeField] private float tauntDuration = 2f;
    [SerializeField] private float getHitDuration = 0.7f;
    [SerializeField] private float dieDuration = 2f;
    [SerializeField] private float disappearDuration = 0.4f;

    [Header("Rotation")]
    [SerializeField] private float rotationOffsetY = 0f;

    private bool encounterRunning;
    private bool waitingForAnswer;

    private int activeVirusIndex;

    private GameObject activeVirus;
    private Animator activeVirusAnimator;
    private Vector3 activeVirusNormalScale;

    private Action encounterFinishedCallback;

    private void Start()
    {
        if (announcementCanvasGroup != null)
        {
            announcementCanvasGroup.alpha = 0f;
        }
    }

    public void StartVirusEncounter(
        int virusIndex,
        Action encounterFinished)
    {
        if (encounterRunning)
        {
            return;
        }

        if (virusIndex < 0 || virusIndex >= virusPrefabs.Length)
        {
            Debug.LogError("Virus index does not exist.");
            return;
        }

        StartCoroutine(
            VirusEncounter(virusIndex, encounterFinished)
        );
    }

    private IEnumerator VirusEncounter(
        int virusIndex,
        Action encounterFinished)
    {
        encounterRunning = true;
        waitingForAnswer = false;
        activeVirusIndex = virusIndex;
        encounterFinishedCallback = encounterFinished;

        // Wait until Amy has landed.
        while (!playerMovement.IsGrounded)
        {
            yield return null;
        }

        // Stop Amy.
        playerMovement.enabled = false;

        // Show the announcement and play the Virus sound.
        StartCoroutine(ShowAnnouncement());
        PlayVirusSound(activeVirusIndex);

        // Spawn the correct Virus.
        activeVirus = Instantiate(
            virusPrefabs[activeVirusIndex],
            virusSpawnPoint.position,
            virusSpawnPoint.rotation
        );

        FaceVirusTowardsPlayer(activeVirus.transform);

        activeVirusNormalScale = activeVirus.transform.localScale;
        activeVirus.transform.localScale = Vector3.zero;

        activeVirusAnimator =
            activeVirus.GetComponentInChildren<Animator>();

        // Scale the Virus into view.
        yield return StartCoroutine(
            ChangeScale(
                activeVirus.transform,
                Vector3.zero,
                activeVirusNormalScale,
                appearDuration
            )
        );

        // Play Taunt.
        PlayAnimation(GetTauntAnimationName());

        yield return new WaitForSeconds(tauntDuration);

        // Switch to IdleBattle.
        PlayAnimation(GetIdleAnimationName());

        waitingForAnswer = true;

        // Keep the Virus animation running while Time.timeScale is 0.
        if (activeVirusAnimator != null)
        {
            activeVirusAnimator.updateMode =
                AnimatorUpdateMode.UnscaledTime;
        }

        // Show the corresponding question.
        if (questionManager != null)
        {
            questionManager.ShowQuestion(
                activeVirusIndex,
                AnswerQuestion
            );
        }
        else
        {
            Debug.LogError(
                "QuestionManager has not been assigned."
            );
        }
    }

    public void AnswerQuestion()
    {
        if (!encounterRunning || !waitingForAnswer)
        {
            return;
        }

        waitingForAnswer = false;

        // Return the Animator to normal game time.
        if (activeVirusAnimator != null)
        {
            activeVirusAnimator.updateMode =
                AnimatorUpdateMode.Normal;
        }

        StartCoroutine(DefeatVirus());
    }

    private IEnumerator DefeatVirus()
    {
        // Play GetHit.
        PlayAnimation(GetHitAnimationName());

        yield return new WaitForSeconds(getHitDuration);

        // Play Die.
        PlayAnimation(GetDieAnimationName());

        yield return new WaitForSeconds(dieDuration);

        // Shrink and remove the Virus.
        if (activeVirus != null)
        {
            yield return StartCoroutine(
                ChangeScale(
                    activeVirus.transform,
                    activeVirusNormalScale,
                    Vector3.zero,
                    disappearDuration
                )
            );

            Destroy(activeVirus);
        }

        activeVirus = null;
        activeVirusAnimator = null;

        // Allow Amy to continue.
        playerMovement.enabled = true;
        encounterRunning = false;

        Action callback = encounterFinishedCallback;
        encounterFinishedCallback = null;

        callback?.Invoke();
    }

    private string GetTauntAnimationName()
    {
        if (activeVirusIndex == 2)
        {
            return "Taunt_BlueSlime";
        }

        if (activeVirusIndex == 3)
        {
            return "Taunt_StarFish";
        }

        return tauntAnimationName;
    }

    private string GetIdleAnimationName()
    {
        if (activeVirusIndex == 2)
        {
            return "IdleBattle_BlueSlime";
        }

        if (activeVirusIndex == 3)
        {
            return "IdleBattle_StarFish";
        }

        return idleAnimationName;
    }

    private string GetHitAnimationName()
    {
        if (activeVirusIndex == 2)
        {
            return "GetHit_BlueSlime";
        }

        if (activeVirusIndex == 3)
        {
            return "GetHit_StarFish";
        }

        return getHitAnimationName;
    }

    private string GetDieAnimationName()
    {
        if (activeVirusIndex == 2)
        {
            return "Die_BlueSlime";
        }

        if (activeVirusIndex == 3)
        {
            return "Die_StarFish";
        }

        return dieAnimationName;
    }

    private IEnumerator ShowAnnouncement()
    {
        if (announcementCanvasGroup == null)
        {
            yield break;
        }

        if (announcementText != null)
        {
            announcementText.text = announcementMessage;
        }

        yield return StartCoroutine(
            FadeCanvasGroup(
                announcementCanvasGroup,
                0f,
                1f,
                announcementFadeDuration
            )
        );

        yield return new WaitForSeconds(
            announcementVisibleTime
        );

        yield return StartCoroutine(
            FadeCanvasGroup(
                announcementCanvasGroup,
                1f,
                0f,
                announcementFadeDuration
            )
        );
    }

    private void PlayVirusSound(int virusIndex)
    {
        if (virusAudioSource == null)
        {
            return;
        }

        if (virusSpawnSounds == null ||
            virusIndex >= virusSpawnSounds.Length)
        {
            return;
        }

        AudioClip selectedSound =
            virusSpawnSounds[virusIndex];

        if (selectedSound != null)
        {
            virusAudioSource.PlayOneShot(selectedSound);
        }
    }

    private void PlayAnimation(string animationName)
    {
        if (activeVirusAnimator == null)
        {
            return;
        }

        activeVirusAnimator.Play(
            animationName,
            0,
            0f
        );
    }

    private void FaceVirusTowardsPlayer(
        Transform virusTransform)
    {
        Vector3 targetPosition =
            playerTransform.position;

        targetPosition.y =
            virusTransform.position.y;

        virusTransform.LookAt(targetPosition);

        virusTransform.Rotate(
            0f,
            rotationOffsetY,
            0f,
            Space.Self
        );
    }

    private IEnumerator ChangeScale(
        Transform target,
        Vector3 startingScale,
        Vector3 endingScale,
        float duration)
    {
        if (target == null)
        {
            yield break;
        }

        if (duration <= 0f)
        {
            target.localScale = endingScale;
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / duration
            );

            target.localScale = Vector3.Lerp(
                startingScale,
                endingScale,
                progress
            );

            yield return null;
        }

        target.localScale = endingScale;
    }

    private IEnumerator FadeCanvasGroup(
        CanvasGroup canvasGroup,
        float startingAlpha,
        float endingAlpha,
        float duration)
    {
        if (duration <= 0f)
        {
            canvasGroup.alpha = endingAlpha;
            yield break;
        }

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / duration
            );

            canvasGroup.alpha = Mathf.Lerp(
                startingAlpha,
                endingAlpha,
                progress
            );

            yield return null;
        }

        canvasGroup.alpha = endingAlpha;
    }
}