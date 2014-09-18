using UnityEngine;
using System.Collections;

public class OSH_Finger : MonoBehaviour
{
		public string m_HandTag;
		private float m_rotateAngle = 0;
		private float m_currentAngle = 0;
		private Vector3 m_eulerRotation;

		private Transform[] m_digitTransforms = new Transform[3];
		// Use this for initialization
		void Start ()
		{
				OSH_Hand hand = GameObject.FindGameObjectWithTag (m_HandTag).GetComponent<OSH_Hand> ();
				hand.registerFinger (this);
				m_digitTransforms [0] = transform;
				m_digitTransforms [1] = transform.GetChild (0);
				m_digitTransforms [2] = transform.GetChild (0).GetChild (0);
		}
	
		// Update is called once per frame
		void Update ()
		{
				m_eulerRotation.Set(0,0, (m_rotateAngle - m_currentAngle)/3);
				foreach (Transform digitTransform in m_digitTransforms) {
					digitTransform.Rotate(m_eulerRotation);
				}
				m_currentAngle = m_rotateAngle;
		}

		public float getRotateAngle ()
		{
				return m_rotateAngle;
		}

		public void setRotateAngle (float rotateAngle)
		{
				m_rotateAngle = rotateAngle;
		}
}
