using Unity.VisualScripting;
using UnityEngine;

public class CrusaderSword : MonoBehaviour
{
    private float damage = 20f;

    void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<EnemyController>().health -= damage;
    }
}
