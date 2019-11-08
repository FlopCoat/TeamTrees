using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float sensitivity = 6f;

	private float rotationX;
	private float rotationY;
	private bool follow = true;

	private void Awake()
	{
		GameManager.camera = transform;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			follow = !follow;
			Cursor.lockState = follow ? CursorLockMode.Locked : CursorLockMode.None;
		}

		if (!follow)
		{
			return;
		}

		rotationX += sensitivity * Input.GetAxis("Mouse X");
		rotationY -= sensitivity * Input.GetAxis("Mouse Y");
		rotationY = Mathf.Clamp(rotationY, -90f, 90f);

		transform.localEulerAngles = new Vector3(rotationY, 0, 0);
	}

	private void FixedUpdate()
	{
		if (!GameManager.gm.started)
		{
			return;
		}

		// Needs to be updated in Fixed update just like player's movement so there is no jitter while moving & rotation at the same time
		GameManager.player.transform.localEulerAngles = new Vector3(0, rotationX, 0);		
	}
}