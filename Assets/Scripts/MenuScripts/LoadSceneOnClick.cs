using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{


    public void LoadByIndex(int sceneIndex)
    {
        
        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1.0f; // solves my problem of having an enemy hitting you after you have won
    }
}