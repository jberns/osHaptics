using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;

public class EulerRotationMod : MonoBehaviour {
	
	public float speed;
	public float rotateAmount;
	private float amountToMove;
	public string sendCommand = "U";
	private float offsetRoll;
	private float offsetPitch;
	private float offsetHeading;

		
	SerialPort sp = new SerialPort ("COM6", 115200 ,Parity.None, 8, StopBits.One);


	private string[] eAngles;
	
	// Use this for initialization
	void Start () {
		sp.Open ();
		sp.ReadTimeout = 20;
		sp.DtrEnable = true;
		print ("Start");
				
	}

	public float F1(){
		return rotateAmount;
	}

	public float F2(){
		return rotateAmount;
	}

	public float F3(){
		return rotateAmount;
	}

	public float F4(){
		return rotateAmount;
	}

	public float F5(){
		return rotateAmount;
	}

	void FixedUpdate(){

	}

	// Update is called once per frame
	void Update () {
		amountToMove = speed * Time.deltaTime;
		
		if (sp.IsOpen) {
			try {

				sp.Write (sendCommand);
				eAngles = sp.ReadLine ().Split (' ');
				float roll = float.Parse (eAngles[0]);
				float pitch = float.Parse (eAngles[1]);
				float heading = float.Parse (eAngles[2]);
				rotateAmount = float.Parse (eAngles[3]);

				if(Input.GetKeyDown (KeyCode.R)){
					offsetRoll = roll - 90;
					offsetPitch = pitch+180;
					offsetHeading = heading;
				}

				transform.eulerAngles = new Vector3(pitch - offsetPitch,

				                                    heading - offsetHeading,

				                                    roll - offsetRoll
				                                    );


				
			} 
			catch (System.Exception) {
				
			}
		}
	}
}


