using System.Collections;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [Header("Entity Health")]
    public float health = 100f;

    protected bool IsDead()
    {
        if (health <= 0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
