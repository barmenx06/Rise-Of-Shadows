using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dalga bazlý düþman spawn akýþýný yönetir. 
/// Ýlk dalga: 10 sn, sonraki dalgalar: 30 sn bekleyerek baþlar.
/// Her bir EnemySpawnInfo için “amount” ve “enemyPrefab” bilgisini kullanýr.
/// Önce SessionManager.StartSession() çaðrýlýr, ardýndan dalga geri sayýmý ve spawn akýþý gelir.
/// </summary>
public class WaveSpawner : MonoBehaviour
{
    [Header("Dalga Ayarlarý")]
    [Tooltip("Tüm dalga tanýmlarýný içeren liste")]
    public List<EnemyWave> waves;

    [Tooltip("Her düþman spawn’ý arasýnda geçecek süre (saniye)")]
    public float timeBetweenSpawns = 0.5f;

    [Header("Countdown Süreleri (saniye)")]
    [Tooltip("Ýlk dalga baþlamadan önce bekleme süresi")]
    public int firstCountdownTime = 10;

    [Tooltip("Ýlk dalgadan sonraki her dalga için bekleme süresi")]
    public int regularCountdownTime = 30;

    [Header("Spawn Noktalarý")]
    [Tooltip("Düþmanlarýn spawn edileceði Transform dizisi")]
    public Transform[] spawnPoints;

    [Header("UI")]
    [Tooltip("Dalga geri sayýmý metnini yöneten controller")]
    public WaveUIController waveUIController;

    private Coroutine spawnRoutine;
    private bool isSpawning = false;
    private bool sessionStarted = false;
    private SessionManager sessionManager;

    private void Start()
    {
        // SessionManager referansýný al
        sessionManager = FindObjectOfType<SessionManager>();

        // Dalga akýþýný baþlat
        StartSpawning();
    }

    /// <summary>
    /// Dalga spawn akýþýný baþlatýr (ilk kez tetiklemek için).
    /// </summary>
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            spawnRoutine = StartCoroutine(SpawnAllWaves());
        }
    }

    /// <summary>
    /// Dalga akýþýný durdurur (örneðin, oyun bitince).
    /// </summary>
    public void StopSpawning()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
        isSpawning = false;
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int i = 0; i < waves.Count && isSpawning; i++)
        {
            // —— Ýlk dalga öncesi, timer'ý baþlat —— 
            if (!sessionStarted)
            {
                if (sessionManager != null)
                    sessionManager.StartSession();
                sessionStarted = true;
            }

            // Bekleme süresini belirle (ilk dalga mý kontrolü)
            int waitTime = (i == 0) ? firstCountdownTime : regularCountdownTime;
            yield return StartCoroutine(WaveCountdown(i, waitTime));

            // Dalga içerisindeki düþmanlarý sýrayla spawn et
            yield return StartCoroutine(SpawnWave(waves[i]));
        }
    }

    private IEnumerator WaveCountdown(int waveIndex, int waitTime)
    {
        int t = waitTime;
        string waveName = $"Wave {waveIndex + 1}";

        if (waveUIController != null)
            waveUIController.ShowWaveCountdown(waveName, t);

        while (t > 0 && isSpawning)
        {
            yield return new WaitForSeconds(1f);
            t--;
            if (waveUIController != null)
                waveUIController.ShowWaveCountdown(waveName, t);
        }

        // Geri sayým sýfýrlandýktan sonra son mesajý 3 sn daha göster
        if (waveUIController != null)
            waveUIController.ShowWaveCountdown(waveName, 0);

        yield return new WaitForSeconds(3f);

        if (waveUIController != null)
            waveUIController.HideWaveText();
    }

    private IEnumerator SpawnWave(EnemyWave wave)
    {
        // Her EnemySpawnInfo için "amount" kadar "enemyPrefab" spawn et
        foreach (EnemySpawnInfo info in wave.enemiesInWave)
        {
            for (int i = 0; i < info.amount && isSpawning; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Vector3 spawnPosition = spawnPoint.position;
                spawnPosition.z = 0f; // 2D oyun için z = 0
                GameObject enemy = Instantiate(info.enemyPrefab, spawnPosition, Quaternion.identity);

                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
    }
}
