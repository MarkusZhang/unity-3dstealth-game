using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

	public float moveSpeed;

	private float maxX;
	private float maxZ;

	private bool disabled;

	public event System.Action onPlayerReachGoal;

	// Use this for initialization
	void Start () {
		GameObject plane = GameObject.FindGameObjectWithTag ("Plane");
		Vector3 planeSize = plane.GetComponent<Renderer> ().bounds.size;
		maxX = planeSize.x / 2;
		maxZ = planeSize.z / 2;
		disabled = false;
	}

	bool isInPlane(Vector3 pos){
		return pos.x < maxX - transform.localScale.x && pos.x > -maxX + transform.localScale.x
			&& pos.z < maxZ - transform.localScale.z && pos.z > -maxZ + transform.localScale.z;
	}
	
	// Update is called once per frame
	void Update () {
		// rotate the player upon arrow keys
		Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized;
		float inputMagnitude = inputDir.magnitude;
		if (inputMagnitude > 0 && !disabled) {
			float angle = 90 - Mathf.Atan2 (inputDir.z, inputDir.x) * Mathf.Rad2Deg;
			transform.eulerAngles = Vector3.up * angle;
		}

		// move the player forward if key is pressed
		if (inputMagnitude > 0 && !disabled) {
			Vector3 newPosition = transform.position + transform.forward * moveSpeed * Time.deltaTime;
			if (isInPlane (newPosition)) {
				transform.Translate (transform.forward * moveSpeed * Time.deltaTime, Space.World);
			}
		}
	}

	void OnTriggerEnter(Collider c){
		if (c.gameObject.name == "Goal") {
			if (onPlayerReachGoal != null) {
				disabled = true;
				onPlayerReachGoal ();
			}
		}
	}
}
