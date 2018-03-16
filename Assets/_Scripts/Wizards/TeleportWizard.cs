using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportWizard : WizardPiece
{
    private int playerLayer;

    protected override void Start()
    {
        base.Start();
        playerLayer = LayerMask.GetMask("Player");
    }


    public override void Penalty(PlayerPiece player)
    {
        base.Penalty(player);
        PlayerPiece lastPlayer = GameManager.Instance.ActivePlayer;
        int lastDistance = 0;
        foreach (PlayerPiece piece in GameManager.Instance.PlayerPieces)
        {
            int d = piece.DistanceFromEnd;
            if (d > lastDistance)
            {
                lastDistance = d;
                lastPlayer = piece;
            }
        }

        Die();
        swapPlayers(player, lastPlayer);
        //TODO: do something about about ties
    }

    private void swapPlayers(PlayerPiece p1, PlayerPiece p2)
    {
        Vector3 position2 = p2.GetComponent<Transform>().position;
        p2.Teleport(p1.GetComponent<Transform>().position);
        p1.Teleport(position2);
    }

    protected override IEnumerator persuasionReward()
    {
        PlayerPiece[] pickedPieces = new PlayerPiece[2];
        bool picking = true;
        while (picking)
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 400, playerLayer))
                {
                    PlayerPiece hitPlayer = hit.transform.GetComponent<PlayerPiece>();
                    if (hitPlayer != null)
                    {
                        int index = pickedPieces[0] == null ? 0 : 1;
                        pickedPieces[index] = hitPlayer;
                        if (index == 1)
                        {
                            swapPlayers(pickedPieces[0], pickedPieces[1]);

                            picking = false;
                            tile.Wizard = null;
                            Die();
                        }
                    }
                }
            }
            yield return null;
        }
    }
}
