using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notifications : MonoBehaviour
{
    public string Text
    {
        get
        {
            return text.text;
        }
        set
        {
            text.text = value;
        }
    }

    private Text text;

    private void Start()
    {
        text = GetComponent<Transform>().GetComponentInChildren<Text>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public Coroutine DisplayNotification(string notification, float time)
    {
        Text = notification;
        Show();
        return StartCoroutine(displayNotification(notification, time));
    }

    private IEnumerator displayNotification(string notification, float time)
    {
        yield return new WaitForSeconds(time);
        Hide();
    }
}
