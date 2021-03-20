using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 4.0f;
    [SerializeField] private GameObject _explosion;
    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (!_spawnManager) {
            Debug.LogError("Asteroid: Spawnmanager not defined");
        
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile")) {

            Debug.Log("Triggered by Projectile");
            Instantiate(_explosion, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(collision.gameObject);
            Destroy(this.gameObject);

        }
    }
}
