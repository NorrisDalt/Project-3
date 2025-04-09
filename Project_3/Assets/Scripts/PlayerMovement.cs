using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float RotateSpeed = 150f;
    public float JumpForce = 5f;
    public float GravityMultiplier = 2f;

    public float pMaxHealth = 100f;
    public float pCurrentHealth;

    private float _vInput;
    private float _hInput;
    private bool _isGrounded;
    private Rigidbody _rb;

    public Transform cameraTransform;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true; //prevent Rigidbody rotation to avoid camera shake
        pCurrentHealth = pMaxHealth;
    }

    void Update()
    {
        //the vertical and horizontal input values
        _vInput = Input.GetAxis("Vertical") * MoveSpeed;
        _hInput = Input.GetAxis("Horizontal") * MoveSpeed;

        //jumping
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        //get camera direction
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        //combine the forward and right inputs with the camera's direction
        Vector3 direction = forward * _vInput + right * _hInput;

        //movement
        if (direction != Vector3.zero)
        {
            _rb.MovePosition(_rb.position + direction * Time.fixedDeltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, Time.fixedDeltaTime * RotateSpeed));
        }

        //manual gravity to avoid camera shake
        _rb.AddForce(Physics.gravity * GravityMultiplier, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision other) //detects when the player is on the ground
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }
    
    public void TakeDamage(float dmg)
    {
        pCurrentHealth -= dmg;

        if(pCurrentHealth <= 0)
        {
            PlayerDeath();
        }
    }

    void PlayerDeath()
    {
        Destroy(this.gameObject);
    }
}
