using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField] float maxHealthPoint = 100f;
	private float currentHealthPoint = 100f;

	public float healthAsPercentage {
		get {
			return currentHealthPoint / maxHealthPoint;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
