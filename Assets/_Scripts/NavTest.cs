using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTest : MonoBehaviour {
	public Transform Target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<NavMeshAgent>().SetDestination(Target.position);
	}
}
