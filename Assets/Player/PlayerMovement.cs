using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
		{
			print("Cursor raycast hits layer: " + cameraRaycaster.layerHit);
			switch (cameraRaycaster.layerHit) {
			case Layer.Walkable:
				currentClickTarget = cameraRaycaster.hit.point;  // So not set in default case
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

