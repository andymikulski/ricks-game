using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour {
	private LineRenderer _lineRenderer;
	private TrackPlayer trackPlayer;
	private MouseOrbitZoom mainCameraControls;
	private MouseOrbitZoom bgCameraControls;
	public float magnitude = 10f;

	private Vector3 _currentPosition;
	private Vector3 bgFgOffset;
	private bool hasFollowButton = false;
	private ParticleSystem engineBlast;


	public void Start()
	{
		_lineRenderer = gameObject.AddComponent<LineRenderer>();

//		Material yourMaterial = (Material)Resources.Load("ray", typeof(Material));
//		_lineRenderer.GetComponent<Renderer>().sharedMaterial = yourMaterial;

		_lineRenderer.startWidth = 1f;
		_lineRenderer.endWidth = 1f;
		_lineRenderer.enabled = false;

		trackPlayer = Camera.main.GetComponent<TrackPlayer> ();

		mainCameraControls = Camera.main.GetComponent<MouseOrbitZoom> ();
		Camera bgCamera = GameObject.Find ("BGCamera").GetComponent<Camera>();
		bgCameraControls = bgCamera.GetComponent<MouseOrbitZoom> ();

		engineBlast = GameObject.Find ("Dope Fire Trail").GetComponent<ParticleSystem>();

		bgFgOffset = bgCamera.transform.position - Camera.main.transform.position;

	}

	public void Update()
	{
		if (Camera.main != null)
		{
			hasFollowButton = Input.GetButton ("Jump"); 
			trackPlayer.enabled = hasFollowButton;
			if (hasFollowButton) {
				Vector3 newCamPosition = Camera.main.transform.position;
				newCamPosition.y = mainCameraControls.target.transform.position.y;

				mainCameraControls.target.transform.position = new Vector3 (newCamPosition.x, mainCameraControls.target.transform.position.y, newCamPosition.z);
				bgCameraControls.target.transform.position = new Vector3 (newCamPosition.x - bgFgOffset.x, bgCameraControls.target.transform.position.y - bgFgOffset.y, newCamPosition.z - bgFgOffset.z);
			}
		}

		if (Input.GetMouseButtonDown(0))
		{
			_lineRenderer.SetPosition(0, transform.position);
			_lineRenderer.numPositions = 1;
			_lineRenderer.enabled = true;

			Time.timeScale = 0.2f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		} 
		else if (Input.GetMouseButton(0))
		{
			_currentPosition = GetCurrentMousePosition();
			_lineRenderer.numPositions = 2;
			_lineRenderer.SetPosition (0, transform.position);
			_lineRenderer.SetPosition (1, _currentPosition);
			gameObject.transform.rotation = Quaternion.LookRotation(_currentPosition);

			Time.timeScale = 0.2f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}
	}

	public void FixedUpdate()
	{
		if (Input.GetMouseButtonUp(0))
		{

			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;

			_lineRenderer.enabled = false;
			Vector3 releasePosition = GetCurrentMousePosition();
			Vector3 direction = releasePosition - transform.position;
			Vector3 impulse = direction * magnitude;

			gameObject.transform.rotation = Quaternion.LookRotation(releasePosition);
			gameObject.GetComponent<Rigidbody> ().AddForce (impulse);
		
			BlastEngines ();
			ShakeCamera (impulse.magnitude / 2000);
		}
	}

	private Vector3 GetCurrentMousePosition()
	{
		if (Camera.main != null) {
			Vector3 foundPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			foundPoint.y = 0;

			return foundPoint;
		} else {
			return new Vector3 ();
		}
	}

	public void ShakeCamera(float amount)
	{
		ShakeObject cameraShake = GameObject.Find ("CameraManager").GetComponent<ShakeObject> ();
		cameraShake.smooth = false;
		cameraShake.smoothAmount = 5f;
		cameraShake.ShakeCamera (amount, 0.025f);
	}

	public void BlastEngines()
	{
		//			engineBlast.Stop ();
		//			float engineBlastDuration = engineBlast.main.duration;
		//			engineBlastDuration = direction.magnitude;
		engineBlast.Play ();
	}
}