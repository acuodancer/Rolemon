using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

	[SerializeField] float maxHealthPoint = 100f;
	private float currentHealthPoint = 100f;

	public float healthAsPercentage {
		get {
			return currentHealthPoint / maxHealthPoint;
		}
	}

    public void TakeDamage(float damage)
    {
        currentHealthPoint = Mathf.Clamp(currentHealthPoint - damage, 0f, maxHealthPoint);
        if (currentHealthPoint <= 0)
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
