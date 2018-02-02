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
    private PlayerPiece activePiece;

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
        int positionOffset = playerPieces.Count % 2 == 0 ? 1 : 0;
        Vector3 lookat = Wizard != null || NextTiles.Length == 0 ? wizardNode.position : NextTiles[0].GetComponent<Transform>().position;
        for (int i = 0; i < playerPieces.Count; i++)
        {
            playerPieces[i].SetPosition(playerNodes[i + positionOffset].position);
            playerPieces[i].SetLookAt(lookat);
        }
    }

    public void MovePiece(PlayerPiece piece)
    {
        if (NextTiles.Length == 1)
        {
            movePiece(piece, 0);
        }
        else
        {
            activePiece = piece;
            GameManager.Instance.GetDirection(this);
        }
    }

    private void movePiece(PlayerPiece piece, int index)
    {
        if (NextTiles.Length > index)
        {
            piece.SetPosition(NextTiles[index].GetComponent<Transform>().position);
            piece.DeregisterToTile();
            piece.Movement--;
        }
    }

    public void DirectionPicked(int direction)
    {
        movePiece(activePiece, direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerPiece")
        {
            PlayerPiece piece = other.GetComponent<PlayerPiece>();
            if (piece.Movement > 0)
            {
                if (NextTiles.Length == 0)
                {
                    //End Game
                }
                else
                {
                    MovePiece(piece);
                }
            }
            else
            {
                RegisterPlayerPiece(piece);
            }
        }
    }
}
