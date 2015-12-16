using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavEnemyNonOPP : MonoBehaviour {
	protected EnemyAIStates state = EnemyAIStates.Partolling;
	static protected List<GameObject> patrolPoints = null;

	public float walkingSpeed = 3.0f;
	public float chasingSpeed = 5.0f;
	public float attackingSpeed = 1.5f;

	public float attackingDistance = 1.0f;

	protected GameObject patrollingInterestPoint;
	protected GameObject playerOfInterest;

	protected float navPatrolDistance = 0.306f; // Hmm, the object always stops at about 0.3059... away...
	protected NavMeshAgent navMeshAgent;

	void Start () {
		if(patrolPoints==null) {
			patrolPoints = new List<GameObject>();
			foreach(GameObject go in GameObject.FindGameObjectsWithTag("NavPatrolPoints")) {
				Debug.Log("Adding NavEnemy Patrol Point: " + go.transform.position);
				patrolPoints.Add(go);
			}
		}
		SwitchToPatrolling();
	}
	
	 void Update () {
		switch(state) {
			case EnemyAIStates.Attacking:
				OnAttackingUpdate();
				break;
			case EnemyAIStates.Chasing:
				OnChasingUpdate();
				break;
			case EnemyAIStates.Partolling:
				OnPatrollingUpdate();
				break;
		}
	}

	 void OnAttackingUpdate() {
		navMeshAgent.SetDestination(playerOfInterest.transform.position);
		
		float distance = Vector3.Distance(transform.position, playerOfInterest.transform.position);
		if(distance>attackingDistance) {
			SwitchToChasing(playerOfInterest);
		}
	}

	void OnChasingUpdate() {
		navMeshAgent.SetDestination(playerOfInterest.transform.position);
		
		float distance = Vector3.Distance(transform.position, playerOfInterest.transform.position);
		if(distance<=attackingDistance) {
			SwitchToAttacking(playerOfInterest);
		}
	}

	void OnPatrollingUpdate() {
		navMeshAgent.SetDestination(patrollingInterestPoint.transform.position);
		
		float distance = Vector3.Distance(transform.position, patrollingInterestPoint.transform.position);
		Debug.Log("Distance: " + distance);
		if(distance<=navPatrolDistance) {
			SelectRandomPatrolPoint();
		}
	}

	void OnTriggerEnter(Collider collider) {
		SwitchToChasing(collider.gameObject);
	}

	void OnTriggerExit(Collider collider) {
		SwitchToPatrolling();
	}

	void SwitchToPatrolling() {
		state = EnemyAIStates.Partolling;
		GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 0.0f);
		SelectRandomPatrolPoint();
		playerOfInterest = null;
	}

	void SwitchToAttacking(GameObject target) {
		state = EnemyAIStates.Attacking;
		GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
	}

	void SwitchToChasing(GameObject target) {
		state = EnemyAIStates.Chasing;
		GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 0.0f);
		playerOfInterest = target;
	}

	void SelectRandomPatrolPoint() {
		int choice = Random.Range(0,patrolPoints.Count);
		patrollingInterestPoint = patrolPoints[choice];
		navMeshAgent.SetDestination(patrollingInterestPoint.transform.position);
		Debug.Log("Enemy navigating to patrol to point " + choice + " at " + patrolPoints[choice].transform.position.ToString());
	}
}
