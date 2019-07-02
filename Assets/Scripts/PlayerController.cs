﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;
    [SerializeField]
    private float thrusterForce = 1000f;
    [SerializeField]
    private float jumpForce = 500f;
    [SerializeField]
    private float thurstAmount = 0.5f;

    private bool jumping = false;
    private float thrustCounter;

    private PlayerMotor motor;

    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov ;
        Vector3 _movVertical = transform.forward * _zMov;

        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

        motor.Move(_velocity);

        //calculate rotation
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        motor.Rotate(_rotation);

        //calculate camera rotation

        float _xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;

        motor.RotateCamera(_cameraRotation);

        Vector3 _thrusterForce = Vector3.zero;
        Vector3 _jumpForce = Vector3.zero;

        if (Input.GetButtonDown("Jump"))
        {
            _jumpForce = Vector3.up * jumpForce;
        }
        if(Input.GetButton("Jump"))
        {
            if(thrustCounter <= thurstAmount)
            {
                thrustCounter += Time.deltaTime;
                _thrusterForce = Vector3.up * thrusterForce;
            }
        } else
        {
            if(thrustCounter >= 0)
            {
                thrustCounter -= Time.deltaTime / 2f;

            }
        }
        print(thrustCounter);

        motor.ApplyThruster(_thrusterForce);
        motor.ApplyJump(_jumpForce);

    }
}
