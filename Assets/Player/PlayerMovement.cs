using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{


	[SerializeField] float walkMoveStopRadius = 0.2f;
	[SerializeField] float attackMoveStopRadius = 2.0f;

    private ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    private CameraRaycaster cameraRaycaster;
	private Vector3 currentDestination;
	private bool isInDirectMode = false;
	private bool m_Jump;
	private Vector3 clickPoint;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
		currentDestination = transform.position;
    }

	private void Update() {
		if (!m_Jump)
		{
			m_Jump = Input.GetButtonDown("Jump");
		}
	}

//    // Fixed update is called in sync with physics
//    private void FixedUpdate()
//    {
//		if (Input.GetKeyDown (KeyCode.G)) { 
//			// G for gamepad. TODO allow player to map later
//			currentDestination = transform.position;
//			isInDirectMode = !isInDirectMode; // toggle mode
//		}
//		if (isInDirectMode) {
//			ProcessDirectMovement();
//		} else {
//			ProcessMouseMovement();
//
//		}
//    }

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

//	private void ProcessMouseMovement ()
//	{
//		if (Input.GetMouseButton (0)) {
//			clickPoint = cameraRaycaster.hit.point;
//			switch (cameraRaycaster.layerHit) {
//			case Layer.Walkable:
//				currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
//				// So not set in default case
//				break;
//			case Layer.Enemy:
//				currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
//				break;
//			default:
//				// Stand still
//				return;
//			}
//		}
//		MoveTo (currentDestination);
//	}

	Vector3 ShortDestination(Vector3 destination, float shortening) {
		Vector3 reductionVector = (destination - transform.position).normalized * shortening;
		return destination - reductionVector;
	}

	void MoveTo (Vector3 dest)
	{
		// Handle movement outside
		Vector3 playerToClickPoint = dest - transform.position;
		m_Character.Move (playerToClickPoint, false, false);
	}

	// Called every frame when Gizoms is turned on
	void OnDrawGizmos() {
		// Draw movement lines gizmos
		Gizmos.color = Color.black;
		Gizmos.DrawLine (transform.position, clickPoint);
		Gizmos.DrawSphere (currentDestination, 0.1f);
		Gizmos.DrawSphere (clickPoint, 0.15f);

		// Draw attack sphere
		Gizmos.color = new Color(255f, 0f, 0f, 0.15f);
		Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
	}
}

