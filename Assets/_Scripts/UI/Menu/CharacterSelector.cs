using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public PlayerPiece.ClassEnum[] Classes;
    public Texture[] CharacterRenderTextures;
    public int PlayerIndex;

    private int characterIndex;
    private RawImage characterImage;

    private void Start()
    {
        characterImage = transform.Find("CharacterImage").GetComponent<RawImage>();
    }

    public void Left()
    {
        characterIndex--;
        if (characterIndex < 0)
        {
            characterIndex = Classes.Length - 1; ;
        }
        SetupDisplay();
    }

    public void Right()
    {
        characterIndex++;
        if (characterIndex >= Classes.Length)
        {
            characterIndex = 0;
        }
        SetupDisplay();
    }

    public void Accept()
    {
        if (!GlobalVals.Instance.PlayerClasses.Contains(Classes[characterIndex]))
        {
            GlobalVals.Instance.PlayerClasses[PlayerIndex] = Classes[characterIndex];
        }
    }

    public void UnAccept()
    {
        GlobalVals.Instance.PlayerClasses[PlayerIndex] = PlayerPiece.ClassEnum.None;
    }

    public void SetupDisplay()
    {
        characterImage.texture = CharacterRenderTextures[characterIndex];
    }
}
