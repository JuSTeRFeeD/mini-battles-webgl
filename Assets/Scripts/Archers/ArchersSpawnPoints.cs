using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Archers
{
    public class ArchersSpawnPoints : MonoBehaviour
    {
        [SerializeField] private Health firstPlayer;
        [SerializeField] private Health secondPlayer;
        
        private Vector3[] _spawnPoints;
        private int _firstSpawnedIndex;
        private int _secondSpawnedIndex;
        
        private int GetNextSpawnPointIndex(bool forFirst)
        {
            var nextPoint = Random.Range(0, _spawnPoints.Length);
            var foo = 0;
            while ((nextPoint == _firstSpawnedIndex || nextPoint == _secondSpawnedIndex) && foo < 10)
            {
                foo++;
                nextPoint = Random.Range(0, _spawnPoints.Length);
            }

            if (forFirst) _firstSpawnedIndex = nextPoint;
            else _secondSpawnedIndex = nextPoint;
            
            return nextPoint;
        }
        
        private void Start()
        {
            InitPoints();

            firstPlayer.onTakeDamage.AddListener(OnPlayerTakeDamage);
            secondPlayer.onTakeDamage.AddListener(OnPlayerTakeDamage);
            
            firstPlayer.transform.position = _spawnPoints[GetNextSpawnPointIndex(true)];
            secondPlayer.transform.position = _spawnPoints[GetNextSpawnPointIndex(false)];
        }

        private void OnDestroy()
        {
            firstPlayer.onTakeDamage.RemoveListener(OnPlayerTakeDamage);
            secondPlayer.onTakeDamage.RemoveListener(OnPlayerTakeDamage);
        }

        private void OnPlayerTakeDamage(Health health)
        {
            health.transform.position = _spawnPoints[GetNextSpawnPointIndex(health == firstPlayer)];
        }

        private void InitPoints()
        {
            var size = transform.childCount;
#if DEBUG
            if (size < 4)
            {
                throw new Exception("Must be 4 spawn point at least!");
            }
#endif
            _spawnPoints = new Vector3[size];
            for (var i = 0; i < size; i++)
            {
                _spawnPoints[i] = transform.GetChild(i).position;
            }
        }
    }
}
