using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _spawnActive = true;
    [Header("Powerups")]
    [SerializeField]
    private GameObject[] _Powerups;
    [SerializeField]
    private GameObject _powerupContainer;
    // Start is called before the first frame update
    void Start()
    {
        if (!_enemyPrefab) {
            Debug.LogError("SpawnManager: No Enemy prefab defined");
        }
        if (_Powerups == null)
        {
            Debug.LogError("SpawnManager: No Powerup prefab defined");
        }
        if (!_enemyContainer || !_powerupContainer)
        {
            Debug.LogError("SpawnManager: No Containers defined");
        }
        

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning() {

        
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());

    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_spawnActive == true) {

            yield return new WaitForSeconds(3.0f);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);

        }
      
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_spawnActive == true)
        {

            yield return new WaitForSeconds(3.0f);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7.5f, 0);
            GameObject newPowerup = Instantiate(_Powerups[Random.Range(0, 5)], posToSpawn, Quaternion.identity);
            newPowerup.transform.parent = _powerupContainer.transform;
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));

        }

    }

    public void OnPlayerDeath() {

        _spawnActive = false;
    }
}
