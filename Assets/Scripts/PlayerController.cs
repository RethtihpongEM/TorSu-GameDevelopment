using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _leftJoystick;
    [SerializeField] private FixedJoystick _rightJoystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _bulletPrefab; // Reference to your bullet prefab
    [SerializeField] private Transform _shootPoint; // Position where bullets spawn
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private float _rotationSpeed = 9f;
    [SerializeField] private float _bulletSpeed = 10f;
    [SerializeField] private float _fireRate = 0.1f; // Time between shots

    private float _nextFireTime;

    private void FixedUpdate()
    {
        // Movement logic
        Vector3 movement = Vector3.zero;

        // Check for joystick or WASD input
        if (_leftJoystick.Horizontal != 0 || _leftJoystick.Vertical != 0)
        {
            // Joystick movement
            movement = new Vector3(_leftJoystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _leftJoystick.Vertical * _moveSpeed);
        }
        else
        {
            // WASD movement
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            movement = new Vector3(horizontal * _moveSpeed, _rigidbody.velocity.y, vertical * _moveSpeed);
        }

        // Apply movement to Rigidbody
        _rigidbody.velocity = movement;

        // Determine Animator states
        bool isMovingForward = movement.z > 0.1f;
        bool isMovingBackward = movement.z < -0.1f;
        bool isIdle = movement == Vector3.zero;

        // Set Animator parameters
        _animator.SetBool("IsRunningFront", isMovingForward);
        _animator.SetBool("IsRunningBack", isMovingBackward);
        _animator.SetBool("IsIdle", isIdle);

        // Rotation and shooting logic using the right joystick
        float horizontalRotation = _rightJoystick.Horizontal;
        float verticalRotation = _rightJoystick.Vertical;

        if (horizontalRotation != 0 || verticalRotation != 0)
        {
            // Calculate direction to face or shoot
            Vector3 direction = new Vector3(horizontalRotation, 0, verticalRotation).normalized;

            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smooth rotation
            _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);

            // Shooting logic
            if (Time.time >= _nextFireTime)
            {
                Shoot(direction);
                _nextFireTime = Time.time + _fireRate;
            }
        }
    }

    private void Shoot(Vector3 direction)
    {
        // Instantiate bullet at the shoot point
        GameObject bullet = Instantiate(_bulletPrefab, _shootPoint.position, _bulletPrefab.transform.rotation);

        // Add force to the bullet in the given direction
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = direction * _bulletSpeed;

        // Optionally, destroy the bullet after a certain time
        Destroy(bullet, 5f);
    }
}
