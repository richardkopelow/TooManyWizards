using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void Show()
    {
        GameManager.Instance.Paused = true;
        GameManager.Instance.HideUI();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        GameManager.Instance.RestoreUI();
        GameManager.Instance.Paused = false;
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
