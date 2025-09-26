using Game.Services.Character;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField, Tooltip("Distance from camera to spawn Enemy in random point")]
        private float _distanceToSpawn = 5;

        [SerializeField] private int _countEnemyForSpawn = 15;
        [SerializeField] private BaseEnemy _enemy;

        private Camera _mainCamera;
        private ITarget _target;

        private List<BaseEnemy> _enemyList;

        public void Init(Camera mainCamera, ITarget target)
        {
            _mainCamera = mainCamera;
            _target = target;

            _enemyList = new(1);
            SpawnEnemies();
        }

        private void OnDestroy()
        {
            for (int i = 0, count = _enemyList.Count; i < count; i++)
            {
                var enemy = _enemyList[i];
                enemy.OnEnemyDied -= OnEnemyDied;
            }
        }

        private void SpawnEnemies()
        {
            for (int i = 0; i < _countEnemyForSpawn; i++)
            {
                var enemy = Instantiate(_enemy, transform);
                enemy.Init(_target);
                var randomPosition = GetRandomSpawnPosition();
                enemy.transform.position = randomPosition;
                enemy.OnEnemyDied += OnEnemyDied;

                _enemyList.Add(enemy);
            }
        }

        private Vector3 GetRandomSpawnPosition()
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

            switch (side)
            {
                case 0:
                    x = Random.Range(left, right);
                    z = top + _distanceToSpawn;
                    break;
                case 1:
                    x = Random.Range(left, right);
                    z = bottom - _distanceToSpawn;
                    break;
                case 2:
                    x = left - _distanceToSpawn;
                    z = Random.Range(bottom, top);
                    break;
                case 3:
                    x = right + _distanceToSpawn;
                    z = Random.Range(bottom, top);
                    break;
                default:
                    x = cameraPos.x;
                    z = cameraPos.z;
                    break;
            }

            return new Vector3(x, 1f, z);
        }

        private void OnEnemyDied(BaseEnemy enemy)
        {
            // TODO: get new point and spawn
        }
    }
}