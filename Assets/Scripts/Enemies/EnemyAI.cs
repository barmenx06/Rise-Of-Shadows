using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1.2f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerHealth playerHealth;

    private bool isAttacking = false;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (isDead || player == null || playerHealth == null)
            return;

        // Oyuncu öldüyse düşman da durmalı!
        if (playerHealth.GetCurrentHealth() <= 0)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && !isAttacking)
        {
            StartCoroutine(AttackPlayer());
        }
        else if (distance > attackRange && !isAttacking)
        {
            MoveTowardsPlayer();
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Saldırı sırasında dur
            animator.SetFloat("Speed", 0f);
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
        if (animator != null)
            animator.SetFloat("Speed", rb.linearVelocity.magnitude);
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetFloat("Speed", 0f);

        // Saldırı animasyonunu başlat
        animator.SetTrigger("Attack");

        // Buradaki süreyi, Attack animasyonunda vuruş frame'ine göre ayarla (ör: 0.3f)
        yield return new WaitForSeconds(0.3f);

        if (playerHealth != null && playerHealth.GetCurrentHealth() > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        // Saldırı bitiş bekleme süresi
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    public void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetFloat("Speed", 0f);
        animator.SetTrigger("Die");
        // Destroy(gameObject, 1.5f); // istersen burada objeyi yok edebilirsin
    }
}
