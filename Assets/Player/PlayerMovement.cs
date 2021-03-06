using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

	[SerializeField] float walkMoveStopRadius = 0.2f;
	[SerializeField] float attackMoveStopRadius = 2.0f;

    private ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
    private CameraRaycaster cameraRaycaster = null;
	private Vector3 currentDestination, clickpoint;
	private bool m_Jump;
    private AICharacterControl aiCharacterControl = null;
    GameObject walkTarget = null;

    // TODO solve fight between serialize and const
    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    private bool isInDirectMode = false;


    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
		currentDestination = transform.position;
        aiCharacterControl = GetComponent<AICharacterControl>();
        walkTarget = new GameObject("walkTarget");

        cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
    }

	private void Update() {
		if (!m_Jump)
		{
			m_Jump = Input.GetButtonDown("Jump");
		}
	}

    private void ProcessMouseClick(RaycastHit raycastHit, int layerHit) {
        switch(layerHit)
        {
            case enemyLayerNumber:
                GameObject enemy = raycastHit.collider.gameObject;
                aiCharacterControl.SetTarget(enemy.transform);
                break;
            case walkableLayerNumber:
                walkTarget.transform.position = raycastHit.point;
                aiCharacterControl.SetTarget(walkTarget.transform);
                break;
            default:
                Debug.LogWarning("Don't know how to handle mouse click for: " + layerHit);
                return;
        }
    }

    //TODO make this get called again
	private void ProcessDirectMovement () {
		// Evaluate movement vectors
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
		Vector3 movement = v*cameraForward + h * Camera.main.transform.right;
        // Pass movement vector to chracter model
        thirdPersonCharacter.Move(movement, false, m_Jump);
		m_Jump = false;
	}

}

