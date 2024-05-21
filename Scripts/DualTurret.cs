using System.Collections;
using UnityEngine;

public class DualTurret : Turret
{
    public Transform targetTwo;
    public Enemy targetEnemyTwo;
    public int totalTimeFire = 0;
    public readonly int maxFireTime = 5;
    public float timeToReload = 3.0f;
    private bool _isReloading;
    [SerializeField] private ReloadingAnimate reloadingAnimate;
    
    protected override void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        float largestDistance = Mathf.Infinity * -1;
        GameObject nearestEnemy = null;
        GameObject furthestEnemy = null;
        
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
            if (distanceToEnemy > largestDistance)
            {
                largestDistance = distanceToEnemy;
                furthestEnemy = enemy;
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

        if (furthestEnemy != null)
        {
            targetTwo = furthestEnemy.transform;
            targetEnemyTwo = furthestEnemy.GetComponent<Enemy>();
        }
        else
        {
            targetTwo = null;
        }
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(timeToReload);
        totalTimeFire = 0;
        _isReloading = false;
        reloadingAnimate.GetUI().gameObject.SetActive(false);
    }
    
    protected override void Shoot()
    {
        if (totalTimeFire == maxFireTime)
        {
            if (_isReloading == false)
            {
                reloadingAnimate.GetUI().gameObject.SetActive(true);
                _isReloading = true;
                StartCoroutine(Reload());
            }
            return;
        }
        
        GameObject bulletGOOne = Instantiate(bulletPrefab, firePointDual[0].position, firePointDual[0].rotation);
        GameObject bulletGOTwo = Instantiate(bulletPrefab, firePointDual[1].position, firePointDual[1].rotation);
        Bullet bullet = bulletGOOne.GetComponent<Bullet>();
        CannonBall cannonBall = bulletGOOne.GetComponent<CannonBall>();
        Bullet bulletTwo = bulletGOTwo.GetComponent<Bullet>();
        CannonBall cannonBallTwo = bulletGOTwo.GetComponent<CannonBall>();
        
        if(bullet != null)
        {
            bullet.Seek(target);
            bulletTwo.Seek(targetTwo);
        }

        if (cannonBall != null)
        {
            cannonBall.Seek(target);
            cannonBallTwo.Seek(targetTwo);
        }

        totalTimeFire++;
    }
}
