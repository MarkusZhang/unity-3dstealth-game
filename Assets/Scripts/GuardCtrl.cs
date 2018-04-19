using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardCtrl : MonoBehaviour {

	public Transform pathHolder;

	public static event System.Action onSpotPlayer;

	public float speed = 5;

	public float waitTime = 0.5f;

	public float turnSpeed = 90;

	public Light spotlight;

	Color originalLightColor;

	public float viewDist = 10;

	public float viewAngle;

	public LayerMask viewMask;

	Transform player;

	void OnDrawGizmos(){
		foreach (Transform waypoint in pathHolder) {
			Gizmos.DrawSphere (waypoint.position, .3f);
		}

		Gizmos.color = Color.red;
		Gizmos.DrawRay (transform.position, transform.forward * viewDist);

	}

	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player").transform;

		originalLightColor = spotlight.color;

		Vector3[] waypoints = new Vector3[pathHolder.childCount];
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints [i] = pathHolder.GetChild (i).position;
			waypoints [i] = new Vector3 (waypoints [i].x, transform.position.y, waypoints [i].z);
		}
			
		StartCoroutine (FollowPath(waypoints));
	}

	void Update(){
		if (CanSeePlayer ()) {
			spotlight.color = Color.red;
			if (onSpotPlayer != null) {
				onSpotPlayer ();
			}
		} else {
			spotlight.color = originalLightColor;
		}
	}

	IEnumerator FollowPath(Vector3[] waypoints){
		// move along the way points 
		transform.position = waypoints[0];

		// set the next target point
		int targetWaypointIndex = 1;
		Vector3 targetWaypoint = waypoints [targetWaypointIndex];

		while (true) {
			// move toward next waypoint
			transform.position = Vector3.MoveTowards (transform.position, targetWaypoint, speed * Time.deltaTime);
			if (transform.position == targetWaypoint) {
				targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
				targetWaypoint = waypoints [targetWaypointIndex];
				StartCoroutine (TurnToFace (targetWaypoint));
				yield return new WaitForSeconds (waitTime);
			}
			yield return null;
		}
	}


	IEnumerator TurnToFace(Vector3 lookTarget){
		Vector3 targetDir = (lookTarget - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2 (targetDir.z, targetDir.x) * Mathf.Rad2Deg;

		while (Mathf.Abs(Mathf.DeltaAngle (targetAngle, transform.eulerAngles.y)) > .05f) {
			// turning angle
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y,targetAngle,Time.deltaTime * turnSpeed);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}
	}


	bool CanSeePlayer(){
		// 1. check whether player is within view distance
		float distance = Vector3.Distance(transform.position,player.position);
		if (distance <= viewDist){
			// 2. check whether the player is within view angle
			Vector3 direction = (player.position - transform.position).normalized;
			float angle = Vector3.Angle (direction, transform.forward);
			if (angle < viewAngle / 2) {
				// 3. check whether the light is blocked by an obstacle
				if (!Physics.Linecast (transform.position, player.position,viewMask)) {
					return true;
				}
			}
		}

		return false;
	}
}
