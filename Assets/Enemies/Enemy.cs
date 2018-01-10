using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable
{

    [SerializeField] float pursueRadius = 3f;
    [SerializeField] float maxHealthPoint = 100f;

    [SerializeField] float attackRadius = 2f;
    [SerializeField] float damagePerShot = 10f;
    [SerializeField] float secondsBetweenShot = 0.5f;
    [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

    [SerializeField] GameObject projectileToUse = null;
    [SerializeField] GameObject projectileSocket = null;

    private float currentHealthPoint = 100f;
    private AICharacterControl aiCharacterControl = null;
    private GameObject player = null;

    bool isAttacking;

    public float healthAsPercentage
    {
        get
        {
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
    void Start()
    {
        currentHealthPoint = maxHealthPoint;
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vecToPlayer = transform.position - player.transform.position;

        if (vecToPlayer.magnitude <= attackRadius && !isAttacking)
        {
            isAttacking = true;
            InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShot); // TODO switch to coroutine
            aiCharacterControl.SetTarget(player.transform);
        }
        
        if (vecToPlayer.magnitude > attackRadius)
        {
            isAttacking = false;
            CancelInvoke();
        }

        if (vecToPlayer.magnitude > pursueRadius && aiCharacterControl.target != null) { 
            aiCharacterControl.SetTarget(null);
        }
    }

    private void SpawnProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDamageCaused(damagePerShot);

        Vector3 unitVectorToPlayer = (player.transform.position - projectileSocket.transform.position + aimOffset).normalized;
        float projectileSpeed = projectileComponent.projectileSpeed;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
    }

    void OnDrawGizmos()
    {
        // Draw attack sphere
        Gizmos.color = new Color(255f, 0f, 0, .5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // Draw pursue sphere
        Gizmos.color = new Color(0f, 0f, 255, .5f);
        Gizmos.DrawWireSphere(transform.position, pursueRadius);
    }
}
