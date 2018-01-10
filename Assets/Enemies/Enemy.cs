using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

	[SerializeField] float viewRadius = 2f;
	[SerializeField] float pursueRadius = 3f;
	[SerializeField] float maxHealthPoint = 100f;
	private float currentHealthPoint = 100f;
	private AICharacterControl aiCharacterControl = null;
	private GameObject player= null;

	public float healthAsPercentage {
		get {
			return currentHealthPoint / maxHealthPoint;
		}
	}

    public void TakeDamage(float damage)
    {
        currentHealthPoint = Mathf.Clamp(currentHealthPoint, damage, 0);
    }

    // Use this for initialization
    void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		aiCharacterControl = GetComponent<AICharacterControl> ();
	}

	// Update is called once per frame
	void Update () {
		Vector3 vecToPlayer = transform.position - player.transform.position;
		if (vecToPlayer.magnitude <= viewRadius) {
			aiCharacterControl.SetTarget(player.transform);
		} else {
			if (aiCharacterControl.target != null) {
				if (vecToPlayer.magnitude > pursueRadius) {
					aiCharacterControl.SetTarget (null);
				}	
			} 
		}
	}
}
