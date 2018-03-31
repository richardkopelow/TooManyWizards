using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public PlayerPiece[] Characters;
    public Camera[] CharacterCameras;
    public int PlayerIndex;
    public int CharacterIndex;

    private RawImage characterImage;
    private Text acceptButtonText;
    private Text statBlock;
    private bool accepted;

    private void Start()
    {
        Transform trans = transform;
        characterImage = trans.Find("CharacterImage").GetComponent<RawImage>();
        acceptButtonText = trans.Find("AcceptButton/Text").GetComponent<Text>();
        statBlock = trans.Find("StatBlock").GetComponent<Text>();
        SetupDisplay();
    }

    public void Left()
    {
        CharacterIndex--;
        if (CharacterIndex < 0)
        {
            CharacterIndex = Characters.Length - 1; ;
        }
        SetupDisplay();
    }

    public void Right()
    {
        CharacterIndex++;
        if (CharacterIndex >= Characters.Length)
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
            if (!GlobalVals.Instance.PlayerClasses.Contains(Characters[CharacterIndex].Class))
            {
                GlobalVals.Instance.PlayerClasses[PlayerIndex] = Characters[CharacterIndex].Class;
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
        statBlock.text = string.Format("Combat Tokens:\t{0}\nPersuasion Tokens:\t{1}", Characters[CharacterIndex].CombatTokens, Characters[CharacterIndex].PersuasionTokens);
    }
}
