using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    protected Transform target;
    protected Enemy targetEnemy;

    [Header("General")]
    public float range = 15f;

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    protected float fireCountdown = 0f;

    [Header("Use Laser")]
    public bool useLaser = false;
    public int damageOverTime = 30;
    public float slowPercent = 0.5f;
    public LineRenderer lineRenderer;
    public Animator anim;
    public GameObject particleEffect;
    private static readonly int IsFiringToIdle = Animator.StringToHash("isFiringToIdle");
    private static readonly int IsFiring = Animator.StringToHash("isFiring");
    private static readonly int IsGoingToFire = Animator.StringToHash("isIdleToFire");
    private static readonly int IsIdle = Animator.StringToHash("isIdle");


    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;
    public Transform firePoint;
    public List<Transform> firePointDual;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        if (particleEffect != null)
        {
            particleEffect.SetActive(false);
        }
    }

    protected virtual void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if(nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            if(useLaser)
            {
                if (useLaser)
                {
                    StartCoroutine(FiringToIdle());
                }

                if(lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                }
            }
            return;
        }

        LockOnTarget();

        if(useLaser)
        {
            if (useLaser)
            {
                StartCoroutine(StartFiring());
            }
        }
        else
        {
            if (fireCountdown <= 0)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }
    }

    private IEnumerator FiringToIdle()
    {
        if (!useLaser) yield break;
        
        anim.SetBool(IsFiring, false);
        anim.SetBool(IsFiringToIdle, true);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool(IsFiringToIdle, false);
        anim.SetBool(IsIdle, true);
        particleEffect.SetActive(false);
    }
    
    private IEnumerator StartFiring()
    {
       
        anim.SetBool(IsGoingToFire, true);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool(IsGoingToFire, false);
        anim.SetBool(IsFiring, true);
        Laser();
    }

    void LockOnTarget()
    {
        //Target Lock on
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Laser()
    {
        if(targetEnemy == null || target == null) return;

        if (particleEffect != null)
        {
            particleEffect.SetActive(true);
        }
        
        targetEnemy.GetComponent<Enemy>().TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowPercent);

        if(!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
    }

    protected virtual void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        CannonBall cannonBall = bulletGO.GetComponent<CannonBall>();
        
        if(bullet != null)
        {
            bullet.Seek(target);
        }

        if (cannonBall != null)
        {
            cannonBall.Seek(target);
        }
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
