using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

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
    [SerializeField] private float _fireRate = 1f;
    private float _canFire = -1f;
    private int _maxAmmoPrimary = 15;
    private int _currentAmmoPrimary = 15;
    [SerializeField]private GameObject _deathRay;
    private bool _deathRayActive = false;
    [SerializeField]
    private GameObject _homingMissile;


    [Header("Misc")]

    [SerializeField] private GameObject _shield;
    [SerializeField]private int _shieldStrength = 3;
    [SerializeField] private GameObject[] _engines;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField] private AudioClip _laserClip;
    private AudioSource _audioSource;

    [SerializeField]
    private Color[] _shieldState;

    private SpriteRenderer _spriteRenderer;
    
    [SerializeField]
    private int _thrusterCooldown = 0;
    [SerializeField]
    private Slider _thrustSlide;


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

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _currentAmmoPrimary > 0)
        {
            FireLaser();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            FireMissile();
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
            _currentAmmoPrimary--;
        }
        _uiManager.UpdateAmmo(_currentAmmoPrimary);
    }

    private void FireMissile()
    {
        GameObject newMissile = Instantiate(_homingMissile, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        newMissile.GetComponent<Homing_Missile_2D>().setEnemyContainer(_enemyContainer);
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        StartCoroutine(ThrusterCoolRoutine());



        transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift) && _thrusterCooldown < 100)
        {
            _speedMultiplier = 2;
            _thruster.transform.localScale = new Vector3(0.7f, 0.5f, 0.7f);
            StartCoroutine(ThrusterBoostRoutine());
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speedMultiplier = 1;
            _thruster.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Debug.LogError("BoostReset");
        }

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
        // 0: Tripleshot | 1: Speed | 2: Shield | 3: Ammo Primary | 4: Health | 5: DeathRay (beam)

        switch (type)
        {
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
                _uiManager.UpdateShield(_shieldStrength);
                break;
            case 3:
                _currentAmmoPrimary = _maxAmmoPrimary;
                _uiManager.UpdateAmmo(_currentAmmoPrimary);
                break;
            case 4:
                _lives++;
                if (_lives == 2) {
                    _engines[0].SetActive(false);
                } else
                {
                    _engines[0].SetActive(false);
                    _engines[1].SetActive(false);
                }
                
                _uiManager.UpdateLives(_lives);
                break;
            case 5:
                _deathRayActive = true;
                _deathRay.SetActive(true);
                StartCoroutine(PowerupPowerDownRoutine(5));

                break;
            default:
                break;

        }


    }

    public void DamageShields()
    {


        if (_shieldStrength == 3)
        {
            _spriteRenderer.color = _shieldState[1];
            _shieldStrength--;
            _uiManager.UpdateShield(_shieldStrength);

        }
        else if (_shieldStrength == 2)
        {

            _spriteRenderer.color = _shieldState[2];
            _shieldStrength--;
            _uiManager.UpdateShield(_shieldStrength);
        }
        else if (_shieldStrength == 1)
        {
            _shield.SetActive(false);
            _spriteRenderer.color = _shieldState[0];
            _isShieldActive = false;
            _shieldStrength--;
            _uiManager.UpdateShield(_shieldStrength);
        }



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
            case 5:
                yield return new WaitForSeconds(5.0f);
                _deathRay.SetActive(false);
                _deathRayActive = false;
               
                break;
            default:
                break;

        }
    }
    IEnumerator ThrusterBoostRoutine()
    {
        if (_thrusterCooldown < 100)
        {
            Debug.Log("boot coroutine triggered");
            _thrusterCooldown += 5;
            _thrustSlide.value = _thrusterCooldown;
            yield return new WaitForSeconds(1f);
        }

        
    }
    IEnumerator ThrusterCoolRoutine()
    {
        if (_thrusterCooldown > 0)
        {
            if (_thrusterCooldown >= 100)
            {
                Debug.Log("overheat triggered");
                yield return new WaitForSeconds(5f);
                _thrusterCooldown --;
                
               
            }
            else
            {
                Debug.Log("cooldown triggered");
                _thrusterCooldown--;
                _thrustSlide.value = _thrusterCooldown;
                yield return new WaitForSeconds(1f);

            }
            
        }
        else
        {
            yield return null;
        }
        
    }
}
