using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_InGame_Manager : MonoBehaviour
{

    public GameObject pausePanel;

    public GameObject pauseButton;
    public GameObject exitButton;
    public GameObject continueButton;


    public void pauseButtonTurnOn()
    {
        if (!pausePanel.active)
        {
            pausePanel.SetActive(true);
            setPauseButtonState(false);
            Time.timeScale = 0;
        }
    }

    public void pauseButtonTurnOff()
    {
        if (pausePanel.active)
        {
            pausePanel.SetActive(false);
            setPauseButtonState(true);
            Time.timeScale = 1;
        }
    }

    public void setPauseButtonState(bool newState)
    {
        pauseButton.SetActive(newState);
    }

    public void setPauseScreenModeToPractise()
    {
        exitButton.SetActive(false);
        continueButton.transform.localPosition = new Vector3(0, -62.9f, 0);
    }

    public void exitButtonPressed()
    {
        Time.timeScale = 1;

        if(SessionManager.instance != null)
            SessionManager.instance.clearSuccesionList();

        SceneManager.LoadScene("MainMenu");
    }
}
