using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

//	Gravity thisGravity;

	void Start()
	{
//		thisGravity = gameObject.GetComponent<Gravity> ();
	}

	void OnCollisionEnter(Collision collision)
	{
		if (!gameObject.active || !gameObject.activeInHierarchy || !gameObject.activeSelf) {
			return;
		}

		GameObject other = collision.collider.gameObject;

		//transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

//		thisGravity.UpdateVariables ();

		Destroy (other);
	}

}
