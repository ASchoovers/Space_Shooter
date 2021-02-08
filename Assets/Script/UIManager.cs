using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Image _livesImg;
    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Text _shieldText;
    [SerializeField]private Text _ammoText;
    private GameManager _gameManager;
    void Start()
    {
        _scoreText.text = "SCORE:" + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (!_gameManager) {

            Debug.LogError("Gamemanager is not defined");
        }
    }

    
    void Update()
    {
        
    }

    public void UpdateScore(int playerScore) {

        _scoreText.text = "Score: " + playerScore.ToString();

    }

    public void UpdateLives(int currentLives)
    {

        _livesImg.sprite = _livesSprites[currentLives];
        if (currentLives == 0) {

            GameOverSequence();
        
        }

    }


    public void UpdateShield(int currentShield)
    {

        switch (currentShield)
        {
            case 0:
                _shieldText.text = "0 / 3";
                break;
            case 1:
                _shieldText.text = "1 / 3";
                break;
            case 2:
                _shieldText.text = "2 / 3";
                break;
            case 3:
                _shieldText.text = "3 / 3";
                break;
            default:
                break;

        }


    }

    public void UpdateAmmo(int currentAmmo)
    {
        _ammoText.text = currentAmmo.ToString() + " / 15";
    }



    public void GameOverSequence() {

        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());


    }

    IEnumerator GameOverFlickerRoutine() {

        while (true) {

            _gameOverText.text = "-- GAME OVER --";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);


        }
    
    }
}
