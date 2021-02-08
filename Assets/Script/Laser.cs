using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    [SerializeField]
    private bool _isEnemyLaser = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {

            MoveUp();

        }
        else {

            MoveDown();
        
        }
    }

    public void MoveUp() {

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 9.0f)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {

                Destroy(this.gameObject);
            }


        }

    }

    public void MoveDown()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -9.0f)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {

                Destroy(this.gameObject);
            }


        }

    }

    public void AssignEnemy() {

        _isEnemyLaser = true;

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && _isEnemyLaser == true) 
        {
            Player player = collision.GetComponent<Player>();

            if (player)
            {
                player.TakeDamage(1);
            }
        }
        
    }
}
