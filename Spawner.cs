using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Transform _container;
    [SerializeField] private int _distanceBeetwenFullLine;
    [SerializeField] private int _distanceBeetwenRandomLine;
    [SerializeField] private int _distanceBeetwenSideWall;
    [SerializeField] private int _repeatCount;

    [Header("Block")]
    [SerializeField] private Block _blockTemplate;
    [SerializeField] private int _blockSpawnChance;

    [Header("Wall")]
    [SerializeField] private Wall _wallTemplate;
    [SerializeField] private int _wallSpawnChance;

    [Header("Bonus")]
    [SerializeField] private Bonus _bonusTemplate;
    [SerializeField] private int _bonusSpawnChance;

    [Header("SideWall")]
    [SerializeField] private SideWall _sideWallTemplate;

    [SerializeField] private BlockSpawnPoint[] _blockSpawnPoints;
    [SerializeField] private WallSpawnPoint[] _wallSpawnPoints;
    [SerializeField] private BonusSpawnPoint[] _bonusSpawnPoints;
    [SerializeField] private SideWallSpawnPoint[] _sideWallSpawnPoints;

    private Vector3 _startSpawnerPosition;
    private int _repeatCountSideWall;

    private void Start()
    {
        _repeatCountSideWall = _repeatCount * 2;
        _startSpawnerPosition = transform.position;

        _blockSpawnPoints = GetComponentsInChildren<BlockSpawnPoint>();
        _wallSpawnPoints = GetComponentsInChildren<WallSpawnPoint>();
        _bonusSpawnPoints = GetComponentsInChildren<BonusSpawnPoint>();
        _sideWallSpawnPoints = GetComponentsInChildren<SideWallSpawnPoint>();

        for (int i = 0; i < _repeatCountSideWall; i++)
        {
            GenerateFullLine(_sideWallSpawnPoints, _sideWallTemplate.gameObject);
            MoveSpawner(_distanceBeetwenSideWall);
        }
        ResetSpawnerPosition();

        for (int i = 0; i < _repeatCount; i++)
        {
            MoveSpawner(_distanceBeetwenFullLine);
            GenerateRandomElement(_bonusSpawnPoints, _bonusTemplate.gameObject, _bonusSpawnChance);
            MoveSpawner(_distanceBeetwenFullLine);
            GenerateRandomElement(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance);
            GenerateFullLine(_blockSpawnPoints, _blockTemplate.gameObject);
            MoveSpawner(_distanceBeetwenRandomLine);
            GenerateRandomElement(_bonusSpawnPoints, _bonusTemplate.gameObject, _bonusSpawnChance);
            MoveSpawner(_distanceBeetwenRandomLine);
            GenerateRandomElement(_wallSpawnPoints, _wallTemplate.gameObject, _wallSpawnChance);
            GenerateRandomElement(_blockSpawnPoints, _blockTemplate.gameObject, _blockSpawnChance);
        }
    }

    private void GenerateFullLine(SpawnPoint[] spawnPoints, GameObject generatedElement)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GenerateElement(spawnPoints[i].transform.position , generatedElement);
        }
    }

    private void GenerateRandomElement(SpawnPoint[] spawnPoints, GameObject generatedElement, int spawnChance)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if(Random.Range(0,100) < spawnChance)
            {
                GameObject element = GenerateElement(spawnPoints[i].transform.position, generatedElement);
            }
        }
    }

    private GameObject GenerateElement(Vector3 spawnPoint, GameObject generatedElement)
    {
        spawnPoint.y -= generatedElement.transform.localScale.y;
        return Instantiate(generatedElement, spawnPoint, Quaternion.identity, _container);
    }

    private void MoveSpawner(int distanceY)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + distanceY, transform.position.z);
    }

    private void ResetSpawnerPosition()
    {
        transform.position = _startSpawnerPosition;
    }
}
