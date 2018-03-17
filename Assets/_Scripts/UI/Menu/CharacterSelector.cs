using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public PlayerPiece.ClassEnum[] Classes;
    public Camera[] CharacterCameras;
    public int PlayerIndex;
    public int CharacterIndex;

    private RawImage characterImage;
    public Text acceptButtonText;
    private bool accepted;

    private void Start()
    {
        Transform trans = transform;
        characterImage = trans.Find("CharacterImage").GetComponent<RawImage>();
        acceptButtonText = trans.Find("AcceptButton/Text").GetComponent<Text>();
        SetupDisplay();
    }

    public void Left()
    {
        CharacterIndex--;
        if (CharacterIndex < 0)
        {
            CharacterIndex = Classes.Length - 1; ;
        }
        SetupDisplay();
    }

    public void Right()
    {
        CharacterIndex++;
        if (CharacterIndex >= Classes.Length)
        {
            CharacterIndex = 0;
        }
        SetupDisplay();
    }

    public void OnReadyClick()
    {
        if (accepted)
        {
            GlobalVals.Instance.PlayerClasses[PlayerIndex] = PlayerPiece.ClassEnum.None;
            acceptButtonText.text = "Ready";
            accepted = false;
        }
        else
        {
            if (!GlobalVals.Instance.PlayerClasses.Contains(Classes[CharacterIndex]))
            {
                GlobalVals.Instance.PlayerClasses[PlayerIndex] = Classes[CharacterIndex];
                acceptButtonText.text = "Unready";
                accepted = true;
            }
        }
    }

    public void SetupDisplay()
    {
        RectTransform imageTrans = characterImage.GetComponent<RectTransform>();
        RenderTexture rt = new RenderTexture((int)imageTrans.rect.width, (int)imageTrans.rect.height, 1);
        characterImage.texture = rt;
        CharacterCameras[CharacterIndex].targetTexture = rt;
    }
}
