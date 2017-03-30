using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGravity : MonoBehaviour {
	public LayerMask m_MagneticLayers;
	public Vector3 m_Position;
	public float m_Radius;
	public float m_Force;

	ParticleSystem.Particle[] m_Particles;

	void Start ()
	{
		m_Radius = GetComponent<Collider> ().bounds.size.x * 6;
		m_Force = m_Radius / 4;
	}

	void FixedUpdate ()
	{
		Collider[] colliders;
		Rigidbody rigidbody;
		ParticleSystem particleEmitter;

		colliders = Physics.OverlapSphere (transform.position + m_Position, m_Radius, m_MagneticLayers);
		foreach (Collider collider in colliders)
		{
			rigidbody = collider.gameObject.GetComponent<Rigidbody> ();
			particleEmitter = collider.gameObject.GetComponent<ParticleSystem> ();

			if (rigidbody != null && rigidbody != gameObject.GetComponent<Rigidbody> ()) {
				rigidbody.AddExplosionForce (m_Force * -1, transform.position + m_Position, m_Radius);
			} else if (particleEmitter != null) {
				m_Particles = new ParticleSystem.Particle[particleEmitter.main.maxParticles];

				for (int i = 0; i < particleEmitter.GetParticles(m_Particles); i++)
				{
					m_Particles[i].velocity += Vector3.up * 1;
				}
			}

		}
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position + m_Position, m_Radius);
	}
}
