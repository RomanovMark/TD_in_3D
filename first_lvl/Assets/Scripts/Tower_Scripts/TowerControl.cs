using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    // Needed variables
    [Header("--- Tower Variables ---")]

    [SerializeField] private float attackRadius;
    public float AttackRadius
    {
        get { return attackRadius; }
        set { attackRadius = value; }
    }

    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject ProjectileRespawnPoint;

    [SerializeField] private float timeBetweenAttack;
    public float TimeBetweenAttack
    {
        get { return timeBetweenAttack; }
        set { timeBetweenAttack = value; }
    }

    [SerializeField] private float towerPrice;
    public float TowerPrice
    {
        get { return towerPrice; }
        set { towerPrice = value; }
    }

    public TowerTypeEnum TowerType;

    [Header("--- Target Variables ---")]
    [SerializeField] private List<GameObject> targetList;
    [SerializeField] private GameObject currentTarget;

    private float attackCounter;
    private bool isAttack;
    private Transform pivot;

    private void Start()
    {
        GetComponent<SphereCollider>().radius = attackRadius;
        pivot = gameObject.transform.GetChild(0).GetChild(0).transform;
    }

    // Register targets
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Add enemy to target list
            targetList.Add(other.gameObject);

            // Check current target
            if (currentTarget == null)
                currentTarget = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targetList.Remove(other.gameObject);

            if (currentTarget == other.gameObject)
                currentTarget = null;
        }
    }

    private void Update()
    {
        attackCounter -= Time.deltaTime;

        if (currentTarget == null && targetList.Count > 0)
            currentTarget = targetList.First();

        if (currentTarget != null)
        {
            // Look at target
            pivot.LookAt(currentTarget.transform.localPosition);

            if (attackCounter <= 0)
            {
                isAttack = true;
                attackCounter = timeBetweenAttack;
            }
            else
                isAttack = false;
        }

    }

    private void FixedUpdate()
    {
        if (isAttack)
            Attack();
    }

    // Custome methods
    void Attack()
    {
        isAttack = false;

        // TODO: Attack logic,
        // Instantiate the projectile
        Instantiate(projectile, ProjectileRespawnPoint.transform.position, ProjectileRespawnPoint.transform.rotation);

    }
}
