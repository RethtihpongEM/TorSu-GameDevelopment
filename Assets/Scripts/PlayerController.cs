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
    [SerializeField] private float _fireRate = 0.5f; // Time between shots
    [SerializeField] private AmmoManager _ammoManager;
    [SerializeField] private AudioSource _audioSource; // Reference to the AudioSource
    [SerializeField] private AudioClip _shootSound;    // Shooting sound effect


    private float _nextFireTime;

    private void FixedUpdate()
    {
        // Handle movement regardless of whether reloading or not
        HandleMovement();

        // Rotation logic
        float horizontalRotation = _rightJoystick.Horizontal;
        float verticalRotation = _rightJoystick.Vertical;

        // Implement a small deadzone for the joystick to avoid firing when joystick is at rest
        if (Mathf.Abs(horizontalRotation) > 0.1f || Mathf.Abs(verticalRotation) > 0.1f)
        {
            // Rotation logic when the right joystick is used
            Vector3 direction = new Vector3(horizontalRotation, 0, verticalRotation).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);

            // Shooting logic only when not reloading
            if (!_ammoManager.IsReloading && Time.time >= _nextFireTime && _ammoManager.TryShoot())
            {

                GameManager.Instance.UpdateAmmoUI(_ammoManager.CurrentAmmo, _ammoManager.MaxAmmo);
                Shoot(direction);  // Shoot in the direction of the joystick input
                _nextFireTime = Time.time + _fireRate;
            }
        }
    }

    private void HandleMovement()
    {
        // Movement logic
        Vector3 movement = Vector3.zero;

        // Check for joystick or WASD input
        if (_leftJoystick.Horizontal != 0 || _leftJoystick.Vertical != 0)
        {
            movement = new Vector3(_leftJoystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _leftJoystick.Vertical * _moveSpeed);
        }
        else
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            movement = new Vector3(horizontal * _moveSpeed, _rigidbody.velocity.y, vertical * _moveSpeed);
        }

        // Apply movement to Rigidbody
        _rigidbody.velocity = movement;

        // Set Animator parameters
        bool isMovingForward = movement.z > 0.1f;
        bool isMovingBackward = movement.z < -0.1f;
        bool isIdle = movement == Vector3.zero;

        _animator.SetBool("IsRunningFront", isMovingForward);
        _animator.SetBool("IsRunningBack", isMovingBackward);
        _animator.SetBool("IsIdle", isIdle);
    }

    private void Shoot(Vector3 direction)
    {
        // Play shooting sound each function called
        if (_audioSource != null && _shootSound != null)
        {
            _audioSource.PlayOneShot(_shootSound);
        }

        // Instantiate bullet at the shoot point
        GameObject bullet = Instantiate(_bulletPrefab, _shootPoint.position, _bulletPrefab.transform.rotation);
        bullet.transform.rotation = Quaternion.LookRotation(_shootPoint.forward);


        // Add force to the bullet in the given direction
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = direction * _bulletSpeed;

        // Optionally, destroy the bullet after a certain time
        Destroy(bullet, 5f);
    }
}

