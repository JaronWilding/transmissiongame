using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPause : MonoBehaviour
{
    //Public Variables
    [Header("Jumping Variables")]
    [Tooltip("The Pause Menu Canvas")]
    [SerializeField] private GameObject gamePauseMenu;
    [Tooltip("Disables and Re-Enables PlayerLook when Pausing")]
    [SerializeField] private PlayerLook playerLookScript;
    [Tooltip("Disables and Re-Enables PlayerMove when Pausing")]
    [SerializeField] private PlayerMove PlayerMove;

    public static bool gameIsPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()

    {
        gamePauseMenu.SetActive(false);
        Time.timeScale = 1;
        playerLookScript.enabled = true;
        PlayerMove.enabled = true;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Pause()
    {
        gamePauseMenu.SetActive(true);
        Time.timeScale = 0;
        playerLookScript.enabled = false;
        PlayerMove.enabled = false;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

}