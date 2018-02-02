using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPiece : MonoBehaviour
{
    public int Movement = 0;
    public int PersuasionToken = 0;
    public int CombatTokens = 0;

    private NavMeshAgent nav;
    private Animator anim; 
    private BoardTile currentTile;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat("Speed",nav.velocity.magnitude);
    }

    public void SetPosition(Vector3 position)
    {
		nav.SetDestination(position);
    }

    public void RegisterToTile(BoardTile tile)
    {
        currentTile = tile;
        if (currentTile.Wizard!=null)
        {
            EnterCombat();
        }
        else
        {
            GameManager.Instance.EndTurn();
        }
    }

    public void DeregisterToTile()
    {
        currentTile = null;
    }

    public void Rolled(int movement)
    {
        Movement = movement;
        currentTile.MovePiece(this);
    }

    public void EnterCombat()
    {

    }
}
