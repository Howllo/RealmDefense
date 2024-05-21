using System;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private Transform target;
    public float speed = 70;
    public int damage = 50;
    public float explosionRadius = 0f;
    public GameObject impactEffect;
    public SphereCollider sphereCollider;
    
    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Start()
    {
        sphereCollider.radius = explosionRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }
    
    void HitTarget()
    {
        Instantiate(impactEffect, target.transform.position, target.transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Damage(other.transform);
        }
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if(e != null)
        {
            e.TakeDamage(damage);
        }    
    }
}
