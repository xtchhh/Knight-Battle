using UnityEngine;

public class EntityController : MonoBehaviour
{
    [Header("Entity Health")]
    public float health = 100f;

    protected void SelfDestruct()
    {
        if (health <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
