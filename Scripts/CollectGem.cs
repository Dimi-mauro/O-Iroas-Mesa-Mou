using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGem : MonoBehaviour
{
    [SerializeField] AudioSource gemFX;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        gemFX.Play();

        // Total gems collected during the run.
        MasterInfo.gemCount++;

        // Progress toward the next Virus encounter.
        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.RegisterCollectedGem();
        }

        gameObject.SetActive(false);
    }

}

