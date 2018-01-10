using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{

    [SerializeField] int enemyLayer = 9;
    [SerializeField] float maxHealthPoint = 100f;
    [SerializeField] float damagePerHit = 15f;
    [SerializeField] float minTimeBetweenHits = 0.5f;
    [SerializeField] float maxAttackRange = 2f;

    private float currentHealthPoint = 100f;
    private GameObject currentTarget = null;
    private CameraRaycaster cameraRaycaster = null;
    private float lastHitTime = 0f;

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
        //    if (currentHealthPoint <= 0)
        //    {
        //        Destroy(gameObject);
        //    }
    }

    // Use this for initialization
    void Start()
    {
        currentHealthPoint = maxHealthPoint;
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick; // registering as layer change obeserver
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseClick(RaycastHit raycastHit, int layerHit)
    {
        if (layerHit == enemyLayer)
        {
            var enemy = raycastHit.collider.gameObject;

            if ((enemy.transform.position - transform.position).magnitude <= maxAttackRange)
            {
                currentTarget = enemy;
                var enemyComponent = enemy.GetComponent<Enemy>();
                if (Time.time - lastHitTime > minTimeBetweenHits)
                {
                    enemyComponent.TakeDamage(damagePerHit);
                    lastHitTime = Time.time;
                }
            }
            else
            {
                // ignore
            }

        }
    }
}
