using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public int HP = 100;
    public Image hpBar;
    

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (hpBar != null)
        {
            hpBar.DOFillAmount((float)HP / 100f, 0.5f).SetEase(Ease.InOutQuad);
        }
        if (HP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Base destroyed!");
        Destroy(gameObject);
    }
}