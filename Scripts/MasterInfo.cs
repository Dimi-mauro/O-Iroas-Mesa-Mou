using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterInfo : MonoBehaviour
{
    public static int coinCount = 0;
    [SerializeField] GameObject coinDisplay;
    public static int gemCount = 0;
    [SerializeField] GameObject gemDisplay;


    void Start()
    {
        coinCount = 0;
        gemCount = 0;
    }

    void Update()
    {
        coinDisplay.GetComponent<TMPro.TMP_Text>().text = "" + coinCount;
        gemDisplay.GetComponent<TMPro.TMP_Text>().text = "" + gemCount;

    }
}

