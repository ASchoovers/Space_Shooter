using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    
    [SerializeField] private int _speed = 5;
    private int _speedMultiplier = 1;
    [SerializeField][Range(0, 5)] private int _lives = 3;
    [SerializeField][Range(0, 100)] private int _Hitpoints = 100;
    [SerializeField] private int _score;

    [Header("Player Components")]

    [SerializeField] private GameObject _thruster;

    [Header("Weapon System")]
    
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleLaserPrefab;
    [SerializeField] private bool _isTripleShotActive = false;
    private bool _isShieldActive = false;
    [SerializeField] private float _fireRate = 0.5f;
    private float _canFire = -1f;
    private int _maxAmmoPrimary = 15;
    private int _currentAmmoPrimary = 15;


    [Header("Misc")]

    [SerializeField] private GameObject _shield;
    [SerializeField]private int _shieldStrength = 3;
    [SerializeField] private GameObject[] _engines;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;

    [SerializeField] private AudioClip _laserClip;
    private AudioSource _audioSource;

    [SerializeField]
    private Color[] _shieldState;

    private SpriteRenderer _spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = _shield.GetComponent<SpriteRenderer>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();
        if (!_audioSource)
        {

            Debug.LogError("Player: No AudioSource defined");

        }
        else
        {
            _audioSource.clip = _laserClip;
        }
        if (!_spawnManager) {

            Debug.LogError("Player: No spawnmanager defined");

        }
        transform.position = new Vector3(0, 0, 0);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (!_uiManager)
        {

            Debug.LogError("Player: No UI Manager defined");

        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();        

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {

        _canFire = Time.time + _fireRate;
        _audioSource.Play();
        

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleLaserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
            
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
            
        }
        
    }


    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);



        transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);

        

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11)
        {

            transform.position = new Vector3(-11, transform.position.y, transform.position.z);

        }
        else if (transform.position.x < -11)
        {

            transform.position = new Vector3(11, transform.position.y, transform.position.z);

        }
    }


    public void TakeDamage(int hitpoints) {

        if (_isShieldActive) {

            DamageShields();
            return;

        }
        _lives -= hitpoints;
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _engines[0].SetActive(true);

        }
        else if (_lives == 1) {

            _engines[1].SetActive(true);

        }

        if (_lives < 1) {

            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }

    }

    public void TogglePowerup(int type)
    {
        // 0: Tripleshot | 1: Speed | 2: Shield
        
        switch (type) {
            case 0:
                _isTripleShotActive = true;
                StartCoroutine(PowerupPowerDownRoutine(0));
                break;
            case 1:
                _speedMultiplier = 2;
                StartCoroutine(PowerupPowerDownRoutine(1));
                break;
            case 2:                
                _spriteRenderer.color = _shieldState[0];
                _shield.SetActive(true);
                _isShieldActive = true;
                _shieldStrength = 3;
                //_uiManager.UpdateShield(_shieldStrength);
                break;
            default:
                break;

        }


    }

    public void DamageShields() {

        
                   
            _shield.SetActive(false);
            
            _isShieldActive = false;
            



    }

    public void UpdateScore(int value) {

        _score += value;
        _uiManager.UpdateScore(_score);
    }

    IEnumerator PowerupPowerDownRoutine(int type)
    {       
            
        switch (type)
        {
            case 0:
                yield return new WaitForSeconds(5.0f);
                _isTripleShotActive = false;
                break;
            case 1:
                yield return new WaitForSeconds(7.0f);
                _speedMultiplier = 1;
                break;
            default:
                break;

        }
    }
}
