using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
	private const float c_fDestroyDelay = 2f;
	private static WaitForSeconds s_WaitSeconds = new WaitForSeconds(c_fDestroyDelay);
	private Coroutine m_DestroyRoutine;

	[SerializeField]
	private int m_iDestroyOnCollisionCount = 0;
	private int m_iCollisionCount = 0;

	private Collider m_collider;
	
	private void Awake()
	{
		m_collider = this.GetComponent<Collider>();
	}

	private void OnCollisionEnter(Collision _Collision)
	{
		m_iCollisionCount++;

		if(m_iDestroyOnCollisionCount > 0 && m_iCollisionCount >= m_iDestroyOnCollisionCount && m_DestroyRoutine == null)
			m_DestroyRoutine = this.StartCoroutine(destroyDelayed());
	}

	private IEnumerator destroyDelayed()
	{
		m_collider.enabled = false;
		yield return s_WaitSeconds;

		m_DestroyRoutine = null;
		Destroy(this.gameObject);
	}
}
