using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageControls : MonoBehaviour
{
    [SerializeField] AudioSource buttonSelect;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PressPlay()
    {
        buttonSelect.Play();
        SceneManager.LoadScene("Level01");
    }
}
