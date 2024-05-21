using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    
    [HideInInspector]
    public float speed;
    public float startHealth = 100;
    private float health;
    public int moneyGained = 50;
    public GameObject deathEffect;

    [Header("Unity Stuff")]
    public Image healthBar;
    private bool isDead = false;
    public SkeletonAnimation skeletonAnimation;

    private void Start()
    {
        speed = startSpeed;
        health = startHealth;
        if(skeletonAnimation != null)
            skeletonAnimation.AnimationState.SetAnimation(4, "Walk", true);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;
        if(health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Slow(float percent)
    {
        speed = startSpeed * (1f - percent);
    }

    void Die()
    {
        skeletonAnimation.AnimationState.SetAnimation(3, "Dead", false);
        isDead = true;
        PlayerStats.Money += moneyGained;
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        WaveSpawner.enemiesAlive--;
        Destroy(gameObject);
    }
}
