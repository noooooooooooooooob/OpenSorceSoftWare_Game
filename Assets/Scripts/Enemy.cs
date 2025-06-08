using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float HP;
    public Image hpBar;
    public int damage;
    public float speed;
    Animator animator;
    SpriteRenderer spritetrenderer;
    BoxCollider2D boxCollider2D;
    Transform target;
    bool isAttacking = false;
    bool isDead = false;
    public bool isSlow = false; // skill3 적용

    void Start()
    {
        animator = GetComponent<Animator>();
        spritetrenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator.SetTrigger("Run");
        target = GameObject.FindGameObjectWithTag("Base").transform;
    }
    void Update()
    {
        if (target != null && !isAttacking && !isDead)
        {
            MoveTowardsTarget();
        }
    }
    void MoveTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction.x >= 0)
        {
            spritetrenderer.flipX = false;
        }
        else
        {
            spritetrenderer.flipX = true;
        }
        transform.position += direction * speed * Time.deltaTime;
    }
    void Attack()
    {
        Vector2 diff = transform.position - target.position;

        if (diff.y < 0.5f)
        {
            animator.SetTrigger("AttackUp");
        }
        else if (diff.y >= 0.5f)
        {
            animator.SetTrigger("AttackDown");
        }
        else if (diff.x >= 0)
        {
            spritetrenderer.flipX = true; // Face right
            animator.SetTrigger("Attack");
        }
        else if (diff.x < 0)
        {
            spritetrenderer.flipX = false; // Face left
            animator.SetTrigger("Attack");
        }

        isAttacking = true;
    }
    public void DoDamage()
    {
        if (target != null)
        {
            // Assuming the target has a method to take damage
            Base baseTarget = target.GetComponent<Base>();
            if (baseTarget != null)
            {
                baseTarget.TakeDamage(damage);
            }
        }
    }
    public void TakeDamage(float damageAmount)
    {
        HP -= damageAmount;
        if (spritetrenderer != null)
        {
            if (hpBar != null)
            {
                hpBar.DOFillAmount((float)HP / 100f, 0.5f).SetEase(Ease.InOutQuad);
            }
            StartCoroutine(HitAnimation());
        }
        if (HP <= 0)
        {
            boxCollider2D.enabled = false; // Disable collider to prevent further interactions
            Die();
        }
    }
    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        Destroy(gameObject, 1.3f); // Delay to allow death animation to play
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Base"))
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }
    public void ApplySlow(float slowFactor, float duration)
    {
        if (isSlow) return; // 이미 느려진 상태라면 중복 적용 방지
        StartCoroutine(ApplyeSlowCoroutine(slowFactor, duration));
    }
    IEnumerator ApplyeSlowCoroutine(float slowFactor, float duration)
    {
        isSlow = true;
        float originalSpeed = speed;
        speed *= slowFactor;

        // 애니메이션 속도 조정
        animator.speed *= slowFactor;

        yield return new WaitForSeconds(duration);

        isSlow = false;
        // 원래 속도로 복귀
        speed = originalSpeed;

        // 애니메이션 속도 복원
        animator.speed /= slowFactor;
    }
    IEnumerator HitAnimation()
    {
        spritetrenderer.DOColor(Color.red, 0.1f).OnComplete(() =>
        {
            spritetrenderer.DOColor(Color.white, 0.1f);
        });
        yield return null;
    }
}
