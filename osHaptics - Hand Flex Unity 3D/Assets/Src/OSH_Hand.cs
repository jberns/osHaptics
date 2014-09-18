using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OSH_Hand : MonoBehaviour
{
		public string m_HandControllerTag;
		/*	Orientation vector arranged as:
		 * 	pitch
		 * 	heading
		 * 	roll
		 * */
		public Vector3 m_orientation;
	public Vector3 m_prevOrientation;
		private List<OSH_Finger> m_fingers = new List<OSH_Finger> (5);

		// Use this for initialization
		void Start ()
		{
				HandController controller = GameObject.FindWithTag (m_HandControllerTag).GetComponent <HandController> ();
				controller.registerHand (this);
				m_prevOrientation = m_orientation;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.R)) {
						resetOrientation ();
				}
				transform.Rotate (m_orientation - m_prevOrientation);
				m_prevOrientation = m_orientation;
		}

		/* Resets hand to original orientation **/
		public void resetOrientation ()
		{	
				m_orientation.x = -180;
				m_orientation.y = 0;
				m_orientation.z = 90;
		}

		/* Registers finger to hand **/
		public void registerFinger (OSH_Finger finger)
		{
				m_fingers.Add (finger);
		}

#region Getters Setters
		public float getPitch ()
		{
				return m_orientation [0];
		}

		public void setPitch (float pitch)
		{
				m_orientation [0] = pitch;
		}

		public float getHeading ()
		{
				return m_orientation [1];
		}

		public void setHeading (float heading)
		{
				m_orientation [1] = heading;
		}

		public float getRoll ()
		{
				return m_orientation [2];
		}
		
		public void setRoll (float roll)
		{
				m_orientation [2] = roll;
		}

		public OSH_Finger getFinger (int index)
		{
				return m_fingers [index];
		}

		public void setFinger (OSH_Finger finger, int index)
		{
				m_fingers [index] = finger;
		}

		public int getFingerCount ()
		{
				return m_fingers.Count;
		}
#endregion
}
