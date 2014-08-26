using UnityEngine;
using System.Collections;

public class FingerRotation5 : MonoBehaviour {
	private EulerRotationMod rotateAngle;
	private float rotateToAngle;
	private Vector3 offset;

	private float currentAngle;

	// Use this for initialization
	void Start () {
		offset = transform.eulerAngles;
		currentAngle = 0;

	}
	
	// Update is called once per frame
	void Update () {
		rotateAngle = GameObject.FindWithTag ("HandController").GetComponent<EulerRotationMod> ();
		rotateToAngle = rotateAngle.F5()/3;
		transform.Rotate (rotateToAngle - currentAngle,0,0);
		currentAngle = rotateToAngle;

	}
}
