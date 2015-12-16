using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {
	protected EnemyAIStates state = EnemyAIStates.Partolling;
	static protected List<GameObject> patrolPoints = null;

	public int damagePoints = 1;
	#region Enemy Options
	public float walkingSpeed = 3.0f;
	public float chasingSpeed = 5.0f;
	public float attackingSpeed = 1.5f;

	public float attackingDistance = 1.0f;
	#endregion

	protected GameObject patrollingInterestPoint;
	protected GameObject playerOfInterest;

	protected virtual void Start () {
		if(patrolPoints==null) {
			patrolPoints = new List<GameObject>();
			foreach(GameObject go in GameObject.FindGameObjectsWithTag("PatrolPoints")) {
				Debug.Log("Adding Enemy Patrol Point: " + go.transform.position);
				patrolPoints.Add(go);
			}
		}
		SwitchToPatrolling();
	}
	
	protected void Update () {
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

	protected virtual void OnAttackingUpdate() {
		float step = attackingSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, playerOfInterest.transform.position, step);

		float distance = Vector3.Distance(transform.position, playerOfInterest.transform.position);
		if(distance>attackingDistance) {
			SwitchToChasing(playerOfInterest);
		}
	}

	protected virtual void OnChasingUpdate() {
		float step = chasingSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, playerOfInterest.transform.position, step);

		float distance = Vector3.Distance(transform.position, playerOfInterest.transform.position);
		if(distance<=attackingDistance) {
			SwitchToAttacking(playerOfInterest);
		}
	}

	protected virtual  void OnPatrollingUpdate() {
		float step = walkingSpeed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, patrollingInterestPoint.transform.position, step);

		float distance = Vector3.Distance(transform.position, patrollingInterestPoint.transform.position);
		if(distance==0) {
			SelectRandomPatrolPoint();
		}
	}

	protected void OnTriggerEnter(Collider collider) {
		SwitchToChasing(collider.gameObject);
	}

	protected void OnTriggerExit(Collider collider) {
		SwitchToPatrolling();
	}

	protected void SwitchToPatrolling() {
		state = EnemyAIStates.Partolling;
		GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 0.0f);
		SelectRandomPatrolPoint();
		playerOfInterest = null;
	}

	protected void SwitchToAttacking(GameObject target) {
		state = EnemyAIStates.Attacking;
		GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f);
		GameController gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		gc.DamagePlayer(damagePoints);

	}

	protected void SwitchToChasing(GameObject target) {
		state = EnemyAIStates.Chasing;
		GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 0.0f);
		playerOfInterest = target;
	}

	protected virtual void SelectRandomPatrolPoint() {
		int choice = Random.Range(0,patrolPoints.Count);
		patrollingInterestPoint = patrolPoints[choice];
		Debug.Log("Enemy going to patrol to point " + patrollingInterestPoint.name);
		
	}
}