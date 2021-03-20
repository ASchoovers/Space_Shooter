using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private float _speed = 4f;
    private int _Damage = 1;
    private int _scoreValue = 10;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (!_audioSource)
        {

            Debug.LogError("Enemy: No AudioSource defined");

        }
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        if (!_player) {

            Debug.LogError("Enemy: Player not asigned");
        }

        if (!_animator)
        {

            Debug.LogError("Enemy: animator not asigned");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemy();
            }
            
        }
    }

    public void CalculateMovement() {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.0f)
        {
            float randomX = Random.Range(-2.5f, 2.5f);
            transform.position = new Vector3(randomX, 8.5f, transform.position.z);
        }

    }

    public void FireWeapons() { 
    
    
    
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enemy hit by:" + other.transform.name);

        if (other.tag == "Player")
        {
            Debug.Log("Triggered by Player");

            Player player = other.transform.GetComponent<Player>();
           
            if (player != null) { 
            
                player.TakeDamage(_Damage);
            }
            _animator.SetTrigger("TriggerDestroy");
            _speed = 0f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Projectile")
        {
            Debug.Log("Triggered by Projectile");
            
            if (_player != null) {

                _player.UpdateScore(_scoreValue);

            }
            _animator.SetTrigger("TriggerDestroy");
            _speed = 0f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Beam")
        {
            Debug.Log("Triggered by Beam");

            if (_player != null)
            {

                _player.UpdateScore(_scoreValue);

            }
            _animator.SetTrigger("TriggerDestroy");
            _speed = 0f;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
}
