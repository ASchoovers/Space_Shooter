using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing_Missile : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private Transform[] _targets;
    [SerializeField]
    private Transform _closestTarget;
    private float _speed = 5f;
    private float rotateSpeed = 100f;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _targets = _enemyContainer.GetComponentsInChildren<Transform>();

        SetTarget();



    }

    // Update is called once per frame
    void Update()
    {

        MoveToTarget();


    }

    public void MoveToTarget()
    {
        if (_closestTarget != null)
        {

            Vector3 direction = (Vector3)_closestTarget.position - rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = new Vector3(0, 0, -rotateAmount * rotateSpeed);

            rb.velocity = transform.up * _speed;

        }
        else {SetTarget();}

        
        


    }

    private void SetTarget() {

        if (_targets[1] != null)
        {
            _targets = _enemyContainer.GetComponentsInChildren<Transform>();
            _closestTarget = _targets[1].transform;

            foreach (Transform target in _targets)
            {
                if (target)
                {
                    float dist = Vector3.Distance(_closestTarget.position, transform.position);
                    float curDist = Vector3.Distance(target.transform.position, transform.position);

                    if (curDist < dist && target.tag == "Enemy")
                    {

                        _closestTarget = target.transform;

                    }
                }
            }
        }
        
        

    }

    public void setEnemyContainer(GameObject con) {

        _enemyContainer = con;

    }
}
