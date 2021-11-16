using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 3;
    public float rotationSpeed = 1;
    private float _moveSpeed;
    private float _rotationSpeed;

    public int hitPoints = 10;

    private CharacterController _characterController;
    private Animator _animator;
    
    void Start() {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;
    }

    void Update() {
        
        if (hitPoints <= 0) {
            _rotationSpeed = 0;
            _moveSpeed = 0;
        } else {
            _rotationSpeed = rotationSpeed;
            _moveSpeed = moveSpeed;
        }
        
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftShift)) {
            _moveSpeed = moveSpeed * 2;
        } else {
            _moveSpeed = moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            _animator.SetTrigger("Jump");
        }

        if (Input.GetKeyDown(KeyCode.E) && (_animator.GetFloat("Speed") < 0.01f)) {
            _animator.SetTrigger("HeavyAttack");
        }

        // Fase di movimento
        var rotation = Input.GetAxis("Horizontal") * _rotationSpeed * Vector3.up;
        transform.Rotate(rotation);

        var move = Input.GetAxis("Vertical") * _moveSpeed * transform.forward;
        _characterController.SimpleMove(move);

        // Sistema di animazioni
        _animator.SetBool("IsDead", hitPoints <= 0);
        _animator.SetFloat("Speed", _characterController.velocity.magnitude);
    }
    public void StartFire() {
        Debug.Log("Fire");
    }

    public void EndFire() {
        Debug.Log("End Fire");
    }
}
