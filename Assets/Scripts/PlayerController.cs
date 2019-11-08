using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float boostSpeed;

	private Rigidbody rb;
	private float extraSpeed;
	private int verticalDirection;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		GameManager.player = this;
	}

	private void Update()
	{
		extraSpeed = Input.GetKey(KeyCode.LeftControl) ? boostSpeed : 0;
		verticalDirection = Input.GetKey(KeyCode.Space) ? 1 : Input.GetKey(KeyCode.LeftShift) ? -1 : 0;
	}

	private void FixedUpdate()
	{
		var rotation = GameManager.camera.rotation.eulerAngles;
		var forward = Quaternion.Euler(0, rotation.y, rotation.z);
		var direction = new Vector3(Input.GetAxisRaw("Horizontal"), verticalDirection, Input.GetAxisRaw("Vertical"));
		if (direction != Vector3.zero)
		{
			rb.AddForce(forward * direction * (speed + extraSpeed) * Time.fixedDeltaTime, ForceMode.VelocityChange);
		}
	}
}