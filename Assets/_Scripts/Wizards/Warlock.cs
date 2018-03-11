using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warlock : WizardPiece
{
    private bool pickingPieces;
    private int playerLayer;

    protected override void Start()
    {
        base.Start();
        playerLayer = LayerMask.GetMask("Player");
    }

    public override void Penalty(PlayerPiece player)
    {
        base.Penalty(player);
        player.Teleport(player.LastCheckpoint.GetComponent<Transform>().position);
    }

    public override void PersuasionReward()
    {
        //Pick Player
        pickingPieces = true;
        
    }

    private void Update()
    {
        if (pickingPieces && Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 400, playerLayer))
            {
                PlayerPiece hitPlayer = hit.transform.GetComponent<PlayerPiece>();
                if (hitPlayer!=null)
                {
                    Penalty(hitPlayer);
                    pickingPieces = false;
                    base.PersuasionReward();
                }
            }
        }
    }
}
