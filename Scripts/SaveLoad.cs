using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public static int loadedCoins;
    public static int loadedGems;

    public static bool saveData;

    [SerializeField] int internalCoin;
    [SerializeField] int internalGem;

    void Start()
    {
        loadedCoins = PlayerPrefs.GetInt("COINSAVE");
        loadedGems = PlayerPrefs.GetInt("GEMSAVE");
    }

    // Update is called once per frame
    void Update()
    {
        internalCoin = loadedCoins + MasterInfo.coinCount;
        internalGem = loadedGems + MasterInfo.gemCount;

        if (saveData == true)
        {
            saveData = false;

            loadedCoins = internalCoin;
            loadedGems = internalGem;

            PlayerPrefs.SetInt("COINSAVE", loadedCoins);
            PlayerPrefs.SetInt("GEMSAVE", loadedGems);
            PlayerPrefs.Save();
        }
    }
}
