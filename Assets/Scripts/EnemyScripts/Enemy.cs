using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace EnemyScripts
{
    public enum EnemyState
    {
        Alive,
        Dead
    }

    public enum EnemyType
    {
        None,
        Normal
    }
    
    public class Enemy : MonoBehaviour
    {
        // Death Event Listener
        public delegate void EventDeathDisable(GameObject obj);
        public EventDeathDisable eventDeathDisable;

        [SerializeField] private int goldOnKill;
        [SerializeField] private int heartDamage;
        [SerializeField] private float speed;
        [SerializeField] private float totalHealth;
        [SerializeField] private float currentHealth;
        [SerializeField] private EnemyType type;
        [SerializeField] private EnemyState state; 
        [SerializeField] private Transform startLocation;
        [SerializeField] private Transform target;
        private int _wavePointIndex;
        private bool _isPooling = true;

        protected virtual void Start()
        {
            currentHealth = totalHealth;
            type = EnemyType.Normal;
            state = EnemyState.Dead;
            gameObject.SetActive(false);
            target = Waypoints.points[0];
        }

        private void Update()
        {
            if(state == EnemyState.Dead) return;
            
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
            if (Vector3.Distance(transform.position, target.position) <= 0.2f)
            {
                GetNextWayPoint();
            }
        }
    
        public void GetNextWayPoint()
        {
            if(_wavePointIndex >= Waypoints.points.Length - 1)
            {
                DealHeartDamage();
                Restart();
                return;
            }
            _wavePointIndex++;
            target = Waypoints.points[_wavePointIndex];
        }

        public void DealDamage(float damage)
        {
            if (currentHealth - damage < 0.0f)
            {
                gameObject.SetActive(false);
                return;
            }
            currentHealth -= damage;
        }

        public void EnemyStart()
        {
            state = EnemyState.Alive;
            gameObject.SetActive(true);
            transform.position = startLocation.position;
        }
        
        /// <summary>
        /// Once the enemy is killed the character state will be reset waiting for the
        /// wave spawner to send the enemy out again.
        /// </summary>
        private void Restart()
        {
            if (startLocation == null) return;

            state = EnemyState.Dead;
            transform.position = startLocation.position;
            _wavePointIndex = 0;
            currentHealth = totalHealth;
            target = Waypoints.points[_wavePointIndex];
        }

        /// <summary>
        /// When the game object is gameObject.SetActive(false) this will be automatically called.
        /// </summary>
        private void OnDisable()
        {
            if (_isPooling)
            {
                _isPooling = false;
                return;
            }
            
            Restart();
            eventDeathDisable?.Invoke(gameObject);
            GameManager.Instance.SetPlayerGold(goldOnKill, GameManagerEnum.Add);
        }

        public EnemyState GetState()
        {
            return state;
        }

        public EnemyType GetEnemyType()
        {
            return type;
        }

        public void SetStartLocation(Transform transform)
        {
            startLocation = transform;
        }

        private void DealHeartDamage()
        {
            GameManager.Instance.SetPlayerHeart(heartDamage);
        }
    }
}
