using Game.Services.Character;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class EnemySpawner : MonoBehaviour, IDisposable
    {
        private const int MAX_ATTEMPT = 50;

        [SerializeField, Tooltip("Distance from camera to spawn Enemy in random point")]
        private float _distanceToSpawn = 5;
        [SerializeField] private float _spawnOffset = 2f;

        [SerializeField] private int _countEnemyForSpawn = 15;
        [SerializeField] private List<EnemySpawnSettings> _enemySpawnSettings;

        [Space(5)]
        [SerializeField] private int _minSecondToRespawn = 3;
        [SerializeField] private int _maxSecondToRespawn = 8;

        [Space(5)]
        [SerializeField] private float _minDistanceBetweenEnemies = 5f;

        private Camera _mainCamera;
        private ITarget _target;

        private List<BaseEnemy> _enemyList;

        public void Init(Camera mainCamera, ITarget target)
        {
            _mainCamera = mainCamera;
            _target = target;

            _enemyList = new(_countEnemyForSpawn);
            SpawnEnemies();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void SpawnEnemies()
        {
            for (int i = 0, count = _enemySpawnSettings.Count; i < count; i++)
            {
                var spawnSettings = _enemySpawnSettings[i];

                for (int j = 0, amountSpawn = spawnSettings.CountSpawn; j < amountSpawn; j++)
                {
                    var enemy = Instantiate(spawnSettings.Enemy, transform);
                    enemy.Init(_target);
                    var randomPosition = GetRandomSpawnPosition();
                    enemy.transform.position = randomPosition;
                    enemy.OnEnemyDied += OnEnemyDied;

                    _enemyList.Add(enemy);
                }
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            for (int i = 0; i < MAX_ATTEMPT; i++)
            {
                Vector3 position = GenerateRandomPosition();

                if (!IsPositionValid(position))
                    continue;

                return position;
            }

            return GenerateRandomPosition();
        }

        private Vector3 GenerateRandomPosition()
        {
            float halfHeight = _mainCamera.orthographicSize;
            float halfWidth = halfHeight * _mainCamera.aspect;

            Vector3 cameraPos = _mainCamera.transform.position;

            float left = cameraPos.x - halfWidth;
            float right = cameraPos.x + halfWidth;
            float bottom = cameraPos.z - halfHeight;
            float top = cameraPos.z + halfHeight;

            int side = Random.Range(0, 4);

            float x, z;

            float randomOffset = Random.Range(-_spawnOffset, _spawnOffset);

            switch (side)
            {
                case 0:
                    x = Random.Range(left, right);
                    z = top + _distanceToSpawn + randomOffset;
                    break;
                case 1:
                    x = Random.Range(left, right);
                    z = bottom - _distanceToSpawn + randomOffset;
                    break;
                case 2:
                    x = left - _distanceToSpawn + randomOffset;
                    z = Random.Range(bottom, top);
                    break;
                case 3:
                    x = right + _distanceToSpawn + randomOffset;
                    z = Random.Range(bottom, top);
                    break;
                default:
                    x = cameraPos.x + Random.Range(-2f, 2f);
                    z = cameraPos.z + Random.Range(-2f, 2f);
                    break;
            }

            return new Vector3(x, 1f, z);
        }

        private bool IsPositionValid(Vector3 position)
        {
            if (_enemyList == null || _enemyList.Count == 0)
                return true;

            for (int i = 0, count = _enemyList.Count; i < count; i++)
            {
                var enemy = _enemyList[i];

                if (enemy == null)
                    continue;

                float distance = Vector3.Distance(position, enemy.transform.position);
                if (_minDistanceBetweenEnemies > distance)
                    return false;
            }

            return true;
        }

        private void OnEnemyDied(BaseEnemy enemy)
        {
            StartCoroutine(RespawnEnemy(enemy));
        }

        private IEnumerator RespawnEnemy(BaseEnemy enemy)
        {
            float randomTime = Random.Range(_minSecondToRespawn, _maxSecondToRespawn);

            yield return new WaitForSeconds(randomTime);

            var newSpawnPos = GetRandomSpawnPosition();
            enemy.transform.position = newSpawnPos;
            enemy.ResetEnemy();
        }

        public void Dispose()
        {
            StopAllCoroutines();

            if (_enemyList != null && _enemyList.Count > 0)
            {
                for (int i = 0, count = _enemyList.Count; i < count; i++)
                {
                    var enemy = _enemyList[i];
                    enemy.OnEnemyDied -= OnEnemyDied;
                    Destroy(enemy.gameObject);
                }

                _enemyList.Clear();
            }

            _enemyList = null;

            _target = null;
        }
    }
}