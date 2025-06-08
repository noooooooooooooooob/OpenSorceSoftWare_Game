using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackController : MonoBehaviour
{
    [Header("스킬 1 UI")]
    public Image skill1Image; // 스킬 1 이미지
    public TMP_Text skill1Text; // 스킬 1 쿨다운 텍스트
    // 조준점(Crosshair) 관리 스크립트
    private CrosshairManager crosshairManager;
    // 메인 카메라
    private Camera mainCamera;
    // 공격 사운드 재생을 위한 AudioSource
    private AudioSource audioSource;

    // 공격 속도 (초당 공격 횟수)
    public float attackSpeed = 2.5f;
    // 다음 공격 가능한 시간
    private float nextAttackTime = 0f;

    // 크리티컬 공격 확률 (0~1)
    public float criticalChance = 0.3f;
    // 일반 공격 데미지
    public int attackDamage = 10;
    // 크리티컬 공격 데미지
    public int criticalDamage = 20;

    [Header("Audio Clips")]
    public AudioClip attackSound;    // 일반 공격 사운드
    public AudioClip criticalSound;  // 크리티컬 공격 사운드

    // 스킬 1
    // --- 스킬 관련 변수 ---
    public bool isSkillActive = false; // 스킬 활성화 상태
    private bool isBuffActive = false;
    private float buffEndTime = 0f;
    private float buffCooldownEndTime = 0f;
    private float originalAttackSpeed;

    // 버프 설정
    private float buffDuration = 5f;
    private float cooldownDuration = 30f;
    private float attackSpeedMultiplier = 2.0f;

    void Start()
    {
        // 씬에 있는 CrosshairManager를 찾아 참조
        crosshairManager = FindFirstObjectByType<CrosshairManager>();
        // 메인 카메라 가져오기
        mainCamera = Camera.main;
        // 오디오 소스 가져오기 (사운드 재생용)
        audioSource = GetComponent<AudioSource>();
        originalAttackSpeed = attackSpeed;
    }

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 체크
        if (Input.GetMouseButton(0) && !isSkillActive)
        {
            // 공격 쿨타임 체크
            if (Time.time >= nextAttackTime)
            {
                // 랜덤하게 크리티컬 여부 결정
                bool isCritical = Random.value < criticalChance;
                TryAttack(isCritical);
                PlayAttackSound(isCritical);
                // 다음 공격 시간 갱신
                nextAttackTime = Time.time + (1f / attackSpeed);
            }
        }
        // 공격 속도 복귀
        if (isBuffActive && Time.time >= buffEndTime)
        {
            attackSpeed = originalAttackSpeed;
            isBuffActive = false;

            crosshairManager.ChangeCrosshairToDefault();
        }
        SkillUIUpdate();
    }
    void SkillUIUpdate()
    {
        if (isBuffActive)
        {
            return;
        }
        // 스킬 1 쿨다운 텍스트 업데이트
        if (Time.time < buffCooldownEndTime)
        {
            skill1Text.text = ((int)(buffCooldownEndTime - Time.time)).ToString();
            skill1Image.fillAmount = (buffCooldownEndTime - Time.time) / cooldownDuration;
        }
    }

    // 공격 시도 메서드
    void TryAttack(bool isCritical)
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

        // 마우스 위치에서 Raycast
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                // 크리티컬 여부에 따른 데미지 결정
                int damageToDeal = isCritical ? criticalDamage : attackDamage;
                // 적에게 데미지 적용
                enemy.TakeDamage(damageToDeal);
                // 크로스헤어 히트마커 표시
                crosshairManager.ShowHitMarker(isCritical);
            }
        }
    }

    // 공격 사운드 재생 메서드
    void PlayAttackSound(bool isCritical)
    {
        // 공격 사운드 피치 랜덤화 (음정 살짝 변형)
        float randomSemitone = Random.Range(-1f, 1f);
        float pitch = Mathf.Pow(1.059463f, randomSemitone); // 반음 간격 계산
        audioSource.pitch = pitch;

        // 크리티컬 여부에 따라 다른 사운드 재생
        if (isCritical)
        {
            audioSource.PlayOneShot(criticalSound);
        }
        else
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    public void ActivateAttackSpeedBuff()
    {
        if (Time.time < buffCooldownEndTime)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("스킬1 쿨다운 중...");
            }
            return;
        }

        attackSpeed *= attackSpeedMultiplier;
        isBuffActive = true;

        buffEndTime = Time.time + buffDuration;
        buffCooldownEndTime = Time.time + cooldownDuration;

        Debug.Log("공격 속도 증가 스킬 발동");
    }
}
