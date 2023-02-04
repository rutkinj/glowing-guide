using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;
    Ray lastRay;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)){
            MoveToCursor();
        }
        AnimationControl();
    }

  private void MoveToCursor()
  {
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    bool hasHit = Physics.Raycast(ray, out hit);

    if(hasHit){
        this.GetComponent<NavMeshAgent>().destination = hit.point;
    }
  }

  private void AnimationControl(){
    Vector3 velocity = this.GetComponent<NavMeshAgent>().velocity;
    // Vector3 localVelocity = transform.InverseTransformDirection;
    
  }
}
