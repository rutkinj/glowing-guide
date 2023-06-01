using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private PlayerInput playerInput;

    [SerializeField] Material red;

    private void Awake() {
        
    }

    private void OnJump(){
        FindObjectOfType<SphereCollider>().GetComponent<MeshRenderer>().material = red;
    }
}
