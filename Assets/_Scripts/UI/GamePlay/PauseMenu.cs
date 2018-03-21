using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
