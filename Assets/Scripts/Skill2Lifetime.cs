using UnityEngine;

public class FireWallLifetime : MonoBehaviour
{
    public float lifetime = 5f;  // 5초 후에 파괴
    public float damagePerSecond = 10.0f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("!!!!!!!");
            Enemy enemyHealth = other.GetComponent<Enemy>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
