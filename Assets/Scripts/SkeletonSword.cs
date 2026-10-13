using Unity.VisualScripting;
using UnityEngine;

public class SkeletonSword : MonoBehaviour
{
    private float damage = 20f;

    void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<TeamController>().health -= damage;
    }
}
