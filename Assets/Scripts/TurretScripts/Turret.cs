using System.Collections.Generic;
using UnityEngine;
using EnemyScripts;
using Managers;

namespace TurretScripts
{
    public enum TurretType
    {
        Prototype,
    }
    
    public class Turret : MonoBehaviour
    {
        private Transform _target;
        
        // Used to keep track of the enemies within range of the turret to find a enemy to shoot at.
        private readonly Dictionary<GameObject, Enemy> _inRange = new Dictionary<GameObject, Enemy>();

        [Header("Turret Info")] 
        [SerializeField] private TurretType turretType;
        
        [Header("Attack Information")]
        [SerializeField] float range;
        [SerializeField] private float fireRate;
        [Tooltip("The total amount of bullets that will be used in the pooling.")]
        [SerializeField] private int totalBulletPooling;
        [SerializeField] private float fireCountdown;
        
        [Header("Turret Cost")] 
        [SerializeField] private int buyCost;
        [SerializeField] private int sellCost;

        [Header("Setup Fields")]
        [SerializeField] private Transform partToRotate;
        [SerializeField] private float turnSpeed = 10f;
        [SerializeField] private GameObject bulletPrefab; 
        [SerializeField] private Transform firePoint;
        
        [Tooltip("Used to set bullet parent to reduce clutter in inspector.")]
        [SerializeField] private GameObject childObject;
        
        // The bullet pooling to prevent bullets from having to constantly be created and destroy.
        private readonly List<Bullet> _bulletPooling = new List<Bullet>();

        private void Awake()
        {
            
            GameManager gm = GameManager.Instance;
            if (gm.GetPlayerGold() - buyCost < 0)
            {
                Destroy(gameObject);
                return;
            }
            
            gm.SetPlayerGold(buyCost, GameManagerEnum.Subtract);
        }

        protected virtual void Start()
        {
            GetComponent<SphereCollider>().radius = range;
            for (int i = 0; i < totalBulletPooling; i++)
            {
                Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation)
                    .GetComponent<Bullet>();
                bullet.transform.parent = childObject.transform;
                bullet.SetFiringPosition(firePoint);
                _bulletPooling.Add(bullet);
            }
        }
        
        protected virtual void Update()
        {
            NearestTarget();
            TargetLockOn();
        }
        
        private void TargetLockOn()
        {
            if(_target == null) return;
            Vector3 dir = _target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            if(fireCountdown <= 0)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }
    
        private void NearestTarget()
        {
            float shortestDistance = float.MaxValue;
            GameObject nearestEnemy = null;

            if (_inRange.Count > 0)
            {
                foreach(var enemy in _inRange.Keys)
                {
                    if(enemy == null)
                        continue;
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if(distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    }
                }
                if(nearestEnemy != null)
                {
                    _target = nearestEnemy.transform;
                }
                else
                {
                    _target = null;
                }
            }
        }

        void Shoot()
        {
            foreach (var bullet in _bulletPooling)
            {
                if (bullet.GetState() == BulletState.Ready)
                {
                    bullet.gameObject.SetActive(true);
                    bullet.BulletTarget(_target);
                    bullet.SetState(BulletState.InMotion);
                    break;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                _inRange.Add(other.gameObject, other.gameObject.GetComponent<Enemy>());
                _inRange[other.gameObject].eventDeathDisable += EventRemovedEnemy;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                _inRange.Remove(other.gameObject);
            }
        }

        void OnDrawGizmosSelected ()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    
        private void OnDestroy()
        {
            foreach (var bullet in _bulletPooling)
            {
                Destroy(bullet.gameObject);
            }
        }

        private void EventRemovedEnemy(GameObject obj)
        {
            _target = null;
            _inRange.Remove(obj);
        }

        public TurretType GetTurretType()
        {
            return turretType;
        }

        public int GetCostToBuild()
        {
            return buyCost;
        }
    }
}