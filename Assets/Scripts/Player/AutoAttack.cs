using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public enum AttackType { Melee, Fireball, Arrow }
    public AttackType currentAttackType = AttackType.Melee;

    [Header("Attack Ranges")]
    public float meleeRange = 1.1f;
    public float fireballRange = 6f;
    public float arrowRange = 8f;
    [HideInInspector]
    public float attackRange;

    [Header("Attack Settings")]
    public float attackCooldown = 1f;
    private float lastAttackTime;

    public Animator animator;
    public string attackTriggerName = "Attack";

    void Update()
    {
        // Saldýrý menzili güncelleniyor
        switch (currentAttackType)
        {
            case AttackType.Melee:
                attackRange = meleeRange;
                break;
            case AttackType.Fireball:
                attackRange = fireballRange;
                break;
            case AttackType.Arrow:
                attackRange = arrowRange;
                break;
        }

        if (Time.time < lastAttackTime + attackCooldown) return;

        // Saldýrý menzilinde düþman var mý kontrolü
        bool enemyInRange = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist <= attackRange)
            {
                enemyInRange = true;
                break;
            }
        }

        if (enemyInRange)
        {
            if (animator != null)
                animator.SetTrigger(attackTriggerName);

            lastAttackTime = Time.time;
        }
    }

    // Animasyon Event'inde çaðrýlacak!
    public void DealDamage()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist <= attackRange)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    int damage = 25;
                    enemyHealth.TakeDamage(damage);
                }
            }
        }
    }

    // Perk/upgrade seçildiðinde çaðýr:
    public void SetAttackType(AttackType newType)
    {
        currentAttackType = newType;
        // (Ýster buraya animasyon trigger, efekt vb. de ekleyebilirsin)
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // Mevcut saldýrý tipine göre menzili göster
        float rangePreview = meleeRange;
        switch (currentAttackType)
        {
            case AttackType.Melee: rangePreview = meleeRange; break;
            case AttackType.Fireball: rangePreview = fireballRange; break;
            case AttackType.Arrow: rangePreview = arrowRange; break;
        }
        Gizmos.DrawWireSphere(transform.position, rangePreview);
    }
}
