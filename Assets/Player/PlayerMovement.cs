using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{


	[SerializeField] float walkMoveStopRadius = 0.2f;

    private ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    private CameraRaycaster cameraRaycaster;
    private Vector3 currentClickTarget;
	private bool isInDirectMode = false;
	private bool m_Jump;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

	private void Update() {
		if (!m_Jump)
		{
			m_Jump = Input.GetButtonDown("Jump");
		}
	}

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
		if (Input.GetKeyDown (KeyCode.G)) { 
			// G for gamepad. TODO allow player to map later
			currentClickTarget = transform.position;
			isInDirectMode = !isInDirectMode; // toggle mode
		}
		if (isInDirectMode) {
			ProcessDirectMovement();
		} else {
			ProcessMouseMovement();

		}
    }

	private void ProcessDirectMovement () {
		// Evaluate movement vectors
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
		Vector3 movement = v*cameraForward + h * Camera.main.transform.right;
		// Pass movement vector to chracter model
		m_Character.Move(movement, false, m_Jump);
		m_Jump = false;
	}

	private void ProcessMouseMovement ()
	{
		if (Input.GetMouseButton (0)) {
			switch (cameraRaycaster.layerHit) {
			case Layer.Walkable:
				currentClickTarget = cameraRaycaster.hit.point;
				// So not set in default case
				break;
			case Layer.Enemy:
				print ("NOT MOVING TO ENEMY");
				break;
			default:
				// Stand still
				return;
			}
		}
		// Handle movement outside
		Vector3 playerToClickPoint = currentClickTarget - transform.position;
		if (playerToClickPoint.magnitude < walkMoveStopRadius) {
			playerToClickPoint = Vector3.zero;
		}
		m_Character.Move (playerToClickPoint, false, false);
	}
}

