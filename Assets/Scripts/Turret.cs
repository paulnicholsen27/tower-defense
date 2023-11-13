using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Turret : MonoBehaviour
{

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 500;
    [SerializeField] private float bps = 2f; 

    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    private Transform target;
    private float timeUntilFire;

    void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (target == null) {
            FindTarget();
            return;
        }
        RotateTowardTarget();
        if (!CheckTargetIsInRange()) {
            target = null;
        } else {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bps) {
                Shoot();
                timeUntilFire = 0f;
            }
        };
    }

    private void Shoot() {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }

    private bool CheckTargetIsInRange() {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void FindTarget() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            transform.position,
            targetingRange,
            (Vector2) transform.position,
            0f,
            enemyMask); 
        if (hits.Length > 0) {
            target = hits[0].transform;
        }
    }

    private void RotateTowardTarget() {
        float angle = Mathf.Atan2(
            target.position.y - transform.position.y,
            target.position.x - transform.position.x
        ) * Mathf.Rad2Deg - 90f;
        Debug.Log("Angle= " + angle);
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        Debug.Log("Rotation= " + targetRotation);
        float maxDegreesDelta = Math.Abs(angle) < 180 ? rotationSpeed : -rotationSpeed;
        turretRotationPoint.rotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation,
            targetRotation,
            maxDegreesDelta * Time.deltaTime);
    }
}
