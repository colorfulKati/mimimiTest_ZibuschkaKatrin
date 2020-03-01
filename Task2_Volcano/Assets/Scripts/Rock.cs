using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
	private const float c_fDestroyDelay = 2f;
	private static WaitForSeconds s_WaitSeconds = new WaitForSeconds(c_fDestroyDelay);
	private static WaitForEndOfFrame s_WaitEndOfFrame = new WaitForEndOfFrame();

	[SerializeField]
	private bool m_bDestroyOnCollision = false;

	private Collider m_collider;
	
	private void Awake()
	{
		m_collider = this.GetComponent<Collider>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(m_bDestroyOnCollision)
			this.StartCoroutine(destroyDelayed());
	}

	private IEnumerator destroyDelayed()
	{
		m_collider.enabled = false;
		yield return s_WaitSeconds;

		Destroy(this.gameObject);
	}
}
