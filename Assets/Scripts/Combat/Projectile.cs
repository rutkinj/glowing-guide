using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float projectileSpeed;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }

    private Vector3 GetAimLocation(){
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        return targetCapsule.center;
    }
}
