using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [Header("Behaviour")]
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private AudioClip _clip;
    [Header("Type")]
    [SerializeField]
    [Range(0, 4)]
    [Tooltip("0: Triple Shot | 1: Speed | 2: Shield | 3: Ammo Primary | 4: Life")]
    private int _powerupID;
    

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f) {

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(_clip, transform.position);

        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();

            
            
            if (player != null)
            {
                player.TogglePowerup(_powerupID);
            }
            
            
            Destroy(this.gameObject);
        }
    }
   
}
