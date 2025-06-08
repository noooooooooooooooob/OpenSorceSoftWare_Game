using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public int HP = 100;
    public Image hpBar;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (hpBar != null)
        {
            hpBar.DOFillAmount((float)HP / 100f, 0.5f).SetEase(Ease.InOutQuad);
        }
        if (spriteRenderer != null)
        {
            StartCoroutine(HitAnimation());
        }
        if (HP <= 0)
        {
            Die();
        }
    }
    // Base가 공격을 받았을 때 빨간색으로 깜빡이는 애니메이션
    IEnumerator HitAnimation()
    {
        spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() =>
        {
            spriteRenderer.DOColor(Color.white, 0.1f);
        });
        yield return null;
    }
    void Die()
    {
        Debug.Log("Base destroyed!");
        Destroy(gameObject);
    }
}