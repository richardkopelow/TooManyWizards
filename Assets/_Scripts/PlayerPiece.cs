using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPiece : MonoBehaviour
{
    public int Movement = 0;
    private NavMeshAgent nav;
    private BoardTile currentTile;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {

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
    }

    public void DeregisterToTile()
    {
        currentTile = null;
    }

    public void EnterCombat()
    {

    }
}
