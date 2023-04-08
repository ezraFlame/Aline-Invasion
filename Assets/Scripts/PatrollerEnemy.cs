using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerEnemy : Enemy
{
	Vector2[] patrolPoints;
	int nextPointIndex;
	[SerializeField]
	float pointDistanceThreshold;
	[SerializeField]
	float moveSpeed;

	protected override void Start() {
		base.Start();
		Vector2[] points = data.GetPointArray("Patrol_Points");
		patrolPoints = new Vector2[points.Length + 1];
		patrolPoints[0] = transform.position;

		for (int i = 0; i < points.Length; i++)
		{
			patrolPoints[i + 1] = points[i];
		}

		nextPointIndex = 0;
	}

	private void Update() {
		Vector2 nextPoint = patrolPoints[nextPointIndex];
		int directionToPoint = nextPoint.x > transform.position.x ? 1 : -1;
		rb.velocity = new Vector2(moveSpeed * directionToPoint, rb.velocity.y);
		if (Mathf.Abs(transform.position.x - nextPoint.x) < pointDistanceThreshold)
		{
			nextPointIndex++;
		}
		if (nextPointIndex >= patrolPoints.Length)
		{
			nextPointIndex = 0;
		}
	}

	public override void Damage(int damage) {
		base.Damage(damage);
		gameObject.SetActive(false);
	}
}
