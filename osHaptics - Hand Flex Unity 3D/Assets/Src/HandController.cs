using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class HandController : MonoBehaviour {

	public string m_queryStateCommand = "U";
	public float testAngle = 60;
	private string m_portName = "COM6";
	private List<OSH_Hand> m_registeredHands = new List<OSH_Hand>(1);

	/* Orientation angles are stored as:		
	*		pitch
	*		heading
	*		roll
	*		finger Rot 1
	*		finger Rot 2
	*		...
	**/
	private List<float> m_currentOrientation;

	SerialPort m_serialPort;

	// Use this for initialization
	void Start () {
		try {
		m_serialPort = new SerialPort (m_portName, 115200 ,Parity.None, 8, StopBits.One);
		m_serialPort.Open ();
		m_serialPort.ReadTimeout = 20;
		m_serialPort.DtrEnable = true;
		}
		catch (System.IO.IOException )
		{
			Debug.Log("IOException thrown");
		}
		m_currentOrientation = new List<float>{-180, 0, 90, 0};
	}
	
	// Update is called once per frame
	void Update () {
		queryStateFromDevice ();
		updateRegisteredHands ();
	}

	public void registerHand(OSH_Hand hand) {
		m_registeredHands.Add (hand);
	}

	private void queryStateFromDevice () {
		try {
			if (m_serialPort.IsOpen) {
				m_serialPort.Write (m_queryStateCommand);
				// Reads as roll, pitch, heading, finger1, finger2 ...
				//string[] queryResult = {"-180","0","90","60","60","60","60","60"};
				string[] queryResult = m_serialPort.ReadLine ().Split (' ');
				m_currentOrientation[0] = float.Parse(queryResult[1]);
				m_currentOrientation[1] = float.Parse(queryResult[2]);
				m_currentOrientation[2] = float.Parse(queryResult[0]);
				for(int i = 3; i < queryResult.Length; i++) {
					m_currentOrientation[i] = float.Parse(queryResult[i]);
				}
			}				
		} 
		catch (System.IO.IOException) {
			Debug.Log("IOException thrown");
		}
		catch (System.NullReferenceException) {
						Debug.Log ("Null Reference Exception thrown");
				}
	}

	public void updateRegisteredHands () {
		foreach (OSH_Hand hand in m_registeredHands) {
			hand.setPitch(m_currentOrientation[0]);
			hand.setHeading(m_currentOrientation[1]);
			hand.setRoll(m_currentOrientation[2]);
			//for (int i = 3; i < Mathf.Min(hand.getFingerCount(), (m_currentOrientation.Count - 2)) ; i++)
			for (int i = 3; i < 8 ; i++)			{
				hand.getFinger(i - 3).setRotateAngle(m_currentOrientation[i]);
				//hand.getFinger(i - 3).setRotateAngle(m_currentOrientation);
			}
		}
	}
	
}
