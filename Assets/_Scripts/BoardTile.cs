using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    public BoardTile[] NextTiles;
    public WizardPiece Wizard;

    private Transform trans;

    private Transform wizardNode;
    private Transform[] playerNodes;
    private List<PlayerPiece> playerPieces;

    void Start()
    {
        trans = GetComponent<Transform>();
        wizardNode = trans.Find("WizardNode");
        playerNodes = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            playerNodes[i] = trans.Find("PlayerNode" + i);
        }
        playerPieces = new List<PlayerPiece>(GlobalVals.Instance.PlayerCount);
    }

    public void RegisterPlayerPiece(PlayerPiece piece)
    {
        playerPieces.Add(piece);
        piece.RegisterToTile(this);
        for (int i = 0; i < playerPieces.Count - 1; i++)
        {
            playerPieces[i].SetPosition(playerNodes[i].position);
        }
    }

    public void MovePiece(PlayerPiece piece)
    {
        if (NextTiles.Length == 1)
        {
            piece.SetPosition(NextTiles[0].GetComponent<Transform>().position);
            piece.DeregisterToTile();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerPiece")
        {
            PlayerPiece piece = other.GetComponent<PlayerPiece>();
            if (piece.Movement>0)
            {
                piece.Movement--;
                MovePiece(piece);
            }
            else
            {
                RegisterPlayerPiece(piece);
            }
        }
    }
}
