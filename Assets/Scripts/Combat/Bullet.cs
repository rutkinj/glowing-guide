using UnityEngine;

namespace RPG.Combat
{
  public class Bullet : MonoBehaviour
  {
    [SerializeField] float speed;
    [SerializeField] float lifeTime;

    private Vector3 shootDir;

    private void Update() {
      transform.Translate(Vector3.forward * speed * Time.deltaTime);
      // transform.position += transform.forward * speed * Time.deltaTime;
    }

    // public void Setup(Vector3 shootDir){
    //   this.shootDir = shootDir;

    // }
  }
}