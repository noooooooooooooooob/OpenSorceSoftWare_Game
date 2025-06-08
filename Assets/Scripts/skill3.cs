using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill3_SlowZone : MonoBehaviour
{
    public float radius = 10f;               // ��ȭ ����
    public float slowFactor = 0.5f;         // �ӵ� ������
    public float duration = 5f;             // ���� �ð�

    private float timer = 0f;

    void Start()
    {
        // ������ �ð� �� �ı�
        Destroy(gameObject, duration);
    }

    void Update()
    {
        timer += Time.deltaTime;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D hit in hitColliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                // ���ο찡 ���� ����� ���� ���
                if (!enemy.GetComponent<SpriteRenderer>().color.Equals(Color.cyan))
                    Debug.Log($"[Skill3] ���� ��� ��: {enemy.name}");
                enemy.ApplySlow(slowFactor, 5f); // ������ ���� ȿ�� �ο�
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

