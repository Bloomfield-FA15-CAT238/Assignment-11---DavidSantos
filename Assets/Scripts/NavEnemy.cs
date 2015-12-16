using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavEnemy : Enemy {
	protected float navPatrolDistance = 0.306f; // Hmm, the object always stops at about 0.3059... away...
	protected NavMeshAgent navMeshAgent;

	public int hitPoints = 1;

	protected override void Start () {
		if(patrolPoints==null) {
			patrolPoints = new List<GameObject>();
			foreach(GameObject go in GameObject.FindGameObjectsWithTag("NavPatrolPoints")) {
				Debug.Log("Adding NavEnemy Patrol Point: " + go.transform.position);
				patrolPoints.Add(go);
			}
		}
		GameObject hair = gameObject.transform.Find("Hair").gameObject;
		hair.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f));
		navMeshAgent = GetComponent<NavMeshAgent>();

		SwitchToPatrolling();
	}

	protected override void OnAttackingUpdate() {
		navMeshAgent.SetDestination(playerOfInterest.transform.position);
		
		float distance = Vector3.Distance(transform.position, playerOfInterest.transform.position);
		if(distance>attackingDistance) {
			SwitchToChasing(playerOfInterest);
		}
	}
	
	protected override void OnChasingUpdate() {
		navMeshAgent.SetDestination(playerOfInterest.transform.position);
		
		float distance = Vector3.Distance(transform.position, playerOfInterest.transform.position);
		if(distance<=attackingDistance) {
			SwitchToAttacking(playerOfInterest);
			GameController gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
			gc.DamagePlayer(damagePoints);

		}
	}
	
	protected override void OnPatrollingUpdate() {
		navMeshAgent.SetDestination(patrollingInterestPoint.transform.position);
		
		float distance = Vector3.Distance(transform.position, patrollingInterestPoint.transform.position);
		Debug.Log("Distance: " + distance);
		if(distance<=navPatrolDistance) {
			SelectRandomPatrolPoint();
		}
	}

	protected override void SelectRandomPatrolPoint() {
		int choice = Random.Range(0,patrolPoints.Count);
		patrollingInterestPoint = patrolPoints[choice];
		navMeshAgent.SetDestination(patrollingInterestPoint.transform.position);
		
		Debug.Log("Enemy navigating to patrol to point " + choice + " at " + patrolPoints[choice].transform.position.ToString());
	}
}
