using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MimiBehaviour
{
	[SerializeField]
	private Transform m_transTarget;
	[SerializeField]
	private Vector3 m_v3CameraOffset;
	[SerializeField]
	private float m_fSmoothSpeed = 0.2f;

	private void FixedUpdate()
	{
		Vector3 v3TargetPosition = m_transTarget.position + m_v3CameraOffset;
		Vector3 v3SmoothPosition = Vector3.Lerp(m_transThis.position, v3TargetPosition, m_fSmoothSpeed);
		this.m_transThis.position = v3SmoothPosition;
	}

}
