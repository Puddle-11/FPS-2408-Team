using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class BaseEnemy : BaseEntity
{

    [SerializeField] private DetectionType DetectPlayerType;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] protected Weapon weaponScr;
    private bool inRange;
    [SerializeField] private GameObject target;
    [SerializeField] private float sightRadius;
    [SerializeField] private float detectionRange;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform headPos;
    [SerializeField] private int sneakDamageMultiplyer = 2;
    public enum DetectionType
    {
        InRange,
        Vision,
        Sound,
        Vision_Sound,
        Continuous,
    }

    // Start is called before the first frame update
    
    public override void Start()
    {
        base.Start();
   
        GameManager.instance.updateGameGoal(1);

    }
    public override void Update()
    {
        if (GetEnemyAlertStatus())
        {
            SetNavmeshTarget();
            weaponScr.Attack();
            if (agent.remainingDistance <= agent.stoppingDistance)  // Rotate enemy view to adjust to player location
            {
                FacePlayer();
            }
        }
        base.Update();
    }
    public bool GetEnemyAlertStatus()
    {
       
            switch (DetectPlayerType)
        {
            case DetectionType.Continuous:
                return true;
            case DetectionType.InRange:
                if (getInrange())
                {
                    return true;
                }
                break;
            case DetectionType.Vision:
                if ((inRange && GetLineOfSight()))
                {
                    return true;
                }
                break;
            case DetectionType.Sound:
                break;
            case DetectionType.Vision_Sound:
                break;

        }
        return false;
    }
    public override void UpdateHealth(int _amount)
    {
        if (!GetEnemyAlertStatus())
        {
            _amount = _amount * sneakDamageMultiplyer;
            SetNavmeshTarget();
        }
 

        base.UpdateHealth(_amount);
      
    }
    private bool GetLineOfSight()
    {
        Vector3 targetDir = (GetTarget().transform.position - transform.position).normalized;
        targetDir.y = -targetDir.y;
        float angle = Vector3.Angle(targetDir, transform.forward);
        

        if (angle < sightRadius / 2)
        {
            RaycastHit hit;
            if(Physics.Raycast(headPos.position, targetDir, out hit, detectionRange, ~weaponScr.ignoreMask))
            {
                if (hit.collider.gameObject == GetTarget())
                {
                    return true;
                }
            }
            Debug.DrawRay(headPos.position, targetDir * 50);
        }

        return false;
    }
    private bool getInrange()
    {
        Vector3 targetDir = (GetTarget().transform.position - transform.position).normalized;
        targetDir.y = -targetDir.y;
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, targetDir, out hit, detectionRange, ~weaponScr.ignoreMask))
        {
            if (hit.collider.gameObject == GetTarget() && inRange)
            {
                return true;
            }
        }
        return false;
    }
    private void SetNavmeshTarget()
    {
        if (agent != null)
        {
            agent.SetDestination(GetTarget().transform.position);
        }
    }
    public void FacePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(GetTarget().transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);


    }
    public GameObject GetTarget()
    {
        if(target == null) return GameManager.instance.playerRef;
        
        return target;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GetTarget())
        {
            inRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GetTarget())
        {
            inRange = false;
        }
    }
    public override void Death()
    {
        GameManager.instance.updateGameGoal(-1);

        base.Death(); //base death contains destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
    }

}
