using UnityEngine;

public class FireWallLifetime : MonoBehaviour
{
    public float lifetime = 5f;  // 5�� �� ������� (���ϸ� ����)
    public int damagePerSecond = 10;

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
                enemyHealth.TakeDamage((int)(damagePerSecond * Time.deltaTime));
            }
        }
    }



}
