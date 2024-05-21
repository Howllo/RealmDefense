using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int wavepointIndex = 0;
    private Enemy enemy;
    public ParticleSystem particleSystem;

    void Start()
    {
        enemy = GetComponent<Enemy>();

        target = Waypoints.points[0];
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextWayPoint();
        }

        enemy.speed = enemy.startSpeed;
    }
    void GetNextWayPoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            PathEnded();
            return;
        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }

    void PathEnded()
    {
        PlayerStats.lives--;
        WaveSpawner.enemiesAlive--;
        var transform1 = transform;
        ParticleSystem particle = Instantiate(particleSystem, transform1.position, transform1.rotation);
        particle.Play();
        Destroy(gameObject);
    }
}
