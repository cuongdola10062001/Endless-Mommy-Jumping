using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState state;
    public Player player;
    public int startingPlatform; //Bắt đầu ta tạo ra bao nhiêu platform
    public float xSpawnOffset; 
    public float minYspawnPos;
    public float maxYspawnPos;
    public Platform[] platformPrefabs;
    public CollectableItem[] collectableItems;

    private Platform m_lastPlatformSpawned;
    private List<int> m_platformLandedIds; // những platform mà người chơi đã nhảy lên
    private float m_halfCamSizeX; //
    private int m_score;

    public Platform LastPlatformSpawned { get => m_lastPlatformSpawned; set => m_lastPlatformSpawned = value; }
    public List<int> PlatformLandedIds { get => m_platformLandedIds; set => m_platformLandedIds = value; }
    public int Score { get => m_score; }

    public override void Awake()
    {
        MakeSingleton(false);
        m_platformLandedIds = new List<int>();
        m_halfCamSizeX = Helper.Get2DCamSize().x / 2;
    }
    public override void Start()
    {
        base.Start();
        state = GameState.Starting;
        Invoke("PlatformInit", 0.5f);

        if(AudioController.Ins) {
            AudioController.Ins.PlayBackgroundMusic();
        }
    }
    public void PlayGame()
    {
        if (GUIManager.Ins)
        {
            GUIManager.Ins.ShowGameplay(true);
        }
        Invoke("PlayGameIvk", 1f);
    }
    private void PlayGameIvk()
    {
        state = GameState.Playing;
        if (player)
        {
            player.Jump();
        }
    }
    private void PlatformInit() //2
    {
        m_lastPlatformSpawned = player.PlatformLanded;
        for (int i = 0; i < startingPlatform; i++)
        {
            SpawnPlatform();
        }
    }
    public bool IsPlatformLanded(int id) //3 : Kiểm tra xem platform này đã được nhảy lên chưa
    {
        if (m_platformLandedIds == null || m_platformLandedIds.Count <= 0) return false;
        return m_platformLandedIds.Contains(id);
    }

    public void SpawnPlatform() //1
    {
        if (!player || platformPrefabs == null || platformPrefabs.Length <= 0) return;

        float spawnPosX = Random.Range(-(m_halfCamSizeX - xSpawnOffset), (m_halfCamSizeX - xSpawnOffset));
        float distBetweenPlat = Random.Range(minYspawnPos, maxYspawnPos);
        float spawnPosY = m_lastPlatformSpawned.transform.position.y + distBetweenPlat;
        Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, 0f);

        int randIdx = Random.Range(0, platformPrefabs.Length);
        var platformPrefab = platformPrefabs[randIdx];

        if (!platformPrefab) return;

        var platformClone = Instantiate(platformPrefab, spawnPos, Quaternion.identity);
        platformClone.Id = m_lastPlatformSpawned.Id + 1;
        m_lastPlatformSpawned = platformClone;
    }
    public void SpawnCollectable(Transform spawnPoint) //5
    {
        if (collectableItems == null || collectableItems.Length <= 0 || state != GameState.Playing) return;
        int randIdx = Random.Range(0, collectableItems.Length);
        var collecItem = collectableItems[randIdx];

        if (collecItem == null) return;

        float randCheck = Random.Range(0f, 1f);
        if (randCheck <= collecItem.spawnRate && collecItem.collectablePrefab)
        {
            var cClone = Instantiate(collecItem.collectablePrefab, spawnPoint.position, Quaternion.identity);
            cClone.transform.SetParent(spawnPoint);
        }
    }

    public void AddScore(int scoreToAdd) //4
    {
        if(state != GameState.Playing) return;
        m_score += scoreToAdd;
        Pref.bestScore = m_score;
        if(GUIManager.Ins) 
        {
            GUIManager.Ins.UpdateScore(m_score);
        }
    }
}
