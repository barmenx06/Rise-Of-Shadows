using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public delegate void EnemyDieHandler();
    public event EnemyDieHandler OnEnemyDie;

    public int maxHealth = 100;
    private int currentHealth;

    public GameObject healthBarRoot; // Can barýný kapsayan UI objesi
    public Image healthBarFill;

    private bool isDead = false;
    private Coroutine hideCoroutine;
    public float barShowDuration = 1.5f;

    [Header("EXP Drop Ayarlarý")]
    public GameObject expDropPrefab;      // Inspector’dan ata (mor yýldýz prefab’ý)
    public int expValue = 10;             // Vereceði exp miktarý

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        if (healthBarRoot != null)
            healthBarRoot.SetActive(false); // Baþlangýçta gizli
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();

        if (healthBarRoot != null)
        {
            healthBarRoot.SetActive(true);
            if (hideCoroutine != null)
                StopCoroutine(hideCoroutine);
            hideCoroutine = StartCoroutine(HideBarAfterDelay());
        }

        if (currentHealth == 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    IEnumerator HideBarAfterDelay()
    {
        yield return new WaitForSeconds(barShowDuration);
        if (healthBarRoot != null)
            healthBarRoot.SetActive(false);
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // EXP Drop Spawnla
        if (expDropPrefab != null)
        {
            GameObject expObj = Instantiate(expDropPrefab, transform.position, Quaternion.identity);

            // Eðer expAmount ExpDrop scriptinde varsa, burada deðeri güncelle:
            ExpDrop expDrop = expObj.GetComponent<ExpDrop>();
            if (expDrop != null)
                expDrop.expAmount = expValue;
        }

        // Ölüm iþlemleri
        if (OnEnemyDie != null)
            OnEnemyDie.Invoke();

        Destroy(gameObject, 0.5f);
    }
}
