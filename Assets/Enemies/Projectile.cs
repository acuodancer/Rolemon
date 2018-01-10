using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    // TODO consider hiding these
    public float projectileSpeed;

    float damageCaused;

    public void SetDamageCaused(float dam)
    {
        damageCaused = dam;
    }

    private void OnTriggerEnter(Collider other)
    {
        Component damageableComponent = other.gameObject.GetComponent(typeof(IDamageable));
        if (damageableComponent)
        {
            (damageableComponent as IDamageable).TakeDamage(damageCaused);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
