using EnemyScripts;
using UnityEngine;

namespace TurretScripts
{
    public enum BulletState
    {
        Ready,
        InMotion
    }
    
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private GameObject impactEffect;
        [SerializeField] private BulletState state;
        [SerializeField] private float bulletDamage;
        private Transform _firingPosition;
        private Transform _target;

        private void Start()
        {
            state = BulletState.Ready;
            transform.position = _firingPosition.position;
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (state == BulletState.Ready || _target == null)
            {
                Restart();
                return;
            }
            Seek();
        }

        protected virtual void Seek()
        {
            Vector3 dir = _target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;
            if(dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }

        public void BulletTarget(Transform target)
        {
            _target = target;
        }

        protected virtual void HitTarget()
        {
            GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 2f);
            _target.GetComponent<Enemy>().DealDamage(bulletDamage);
            Restart();
        }

        protected virtual void Restart()
        {
            state = BulletState.Ready;
            transform.position = _firingPosition.position;
            gameObject.SetActive(false);
        }

        public void SetFiringPosition(Transform position)
        {
            _firingPosition = position;
        }

        public BulletState GetState()
        {
            return state;
        }

        public void SetState(BulletState newState)
        {
            state = newState;
        }
    }
}
