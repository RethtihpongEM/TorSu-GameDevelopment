// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
// public class PlayerController : MonoBehaviour
// {
//     [SerializeField] private Rigidbody _rigidbody;         
//     [SerializeField] private FixedJoystick _leftJoystick; 
//     [SerializeField] private FixedJoystick _rightJoystick;
//     [SerializeField] private Animator _animator;          
//     [SerializeField] private float _moveSpeed;       
//     [SerializeField] private float _rotationSpeed; 

//     private void FixedUpdate()
//     {
//         // Movement logic (either joystick or WASD keys)
//         Vector3 movement = Vector3.zero;

//         // Joystick movement
//         if (_leftJoystick.Horizontal != 0 || _leftJoystick.Vertical != 0)
//         {
//             movement = new Vector3(_leftJoystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _leftJoystick.Vertical * _moveSpeed);
//         }
//         // WASD movement
//         else
//         {
//             float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
//             float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down Arrow
//             movement = new Vector3(horizontal * _moveSpeed, _rigidbody.velocity.y, vertical * _moveSpeed);
//         }

//         _rigidbody.velocity = movement;

//         // Rotation logic using the right joystick
//         float horizontalRotation = _rightJoystick.Horizontal;
//         float verticalRotation = _rightJoystick.Vertical;

//         if (horizontalRotation != 0 || verticalRotation != 0)
//         {
//             // Calculate the direction to face
//             Vector3 direction = new Vector3(horizontalRotation, 0, verticalRotation).normalized;

//             // Calculate the target rotation
//             Quaternion targetRotation = Quaternion.LookRotation(direction);

//             // Smoothly rotate towards the target direction
//             _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
//         }
//     }
// }


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
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;


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

        // Rotation logic using the right joystick
        float horizontalRotation = _rightJoystick.Horizontal;
        float verticalRotation = _rightJoystick.Vertical;

        if (horizontalRotation != 0 || verticalRotation != 0)
        {
            // Calculate direction to face
            Vector3 direction = new Vector3(horizontalRotation, 0, verticalRotation).normalized;

            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smooth rotation
            _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        }
    }
}