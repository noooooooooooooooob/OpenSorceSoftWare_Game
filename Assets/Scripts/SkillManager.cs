using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class SkillManager : MonoBehaviour
{
    [Header("스킬 UI 설정")]
    public Image skill2Image;  // FireWall 스킬 Image
    public Image skill3Image;  // 둔화 지역 스킬 Image
    public Image skill4Image;  // 위성 폭격 스킬 Image
    public TMP_Text skill2Text;  // FireWall 쿨다운 텍스트
    public TMP_Text skill3Text;  // 둔화 지역 스킬 쿨다운 텍스트
    public TMP_Text skill4Text;  // 위성 폭격 스킬 쿨다운 텍스트
    public float skill2Cooldown;  // FireWall 스킬 쿨다운 시간
    public float skill2CooldownTime;  // FireWall 스킬 쿨다운 시간 설정
    public float skill3Cooldown;  // 둔화 지역 스킬 쿨다운 시간
    public float skill3CooldownTime;  // 둔화 지역 스킬 쿨다운 시간 설정
    public float skill4Cooldown;  // 위성 폭격 스킬 쿨다운 시간
    public float skill4CooldownTime;  // 위성 폭격 스킬 쿨다운 시간 설정

    public GameObject fireWallPrefab;  // FireWall 프리팹
    public GameObject skill3Prefab;  // 둔화 지역 프리팹
    public GameObject skill4Prefab;  // 위성 폭격 프리팹

    private bool isSkill2Selected = false;
    private bool isSkill3Selected = false;
    private bool isSkill4Selected = false;

    private Vector2 firstClickPos;
    private bool waitingForSecondClick = false;
    public AttackController attackController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && skill2Cooldown <= 0f)
        {
            isSkill2Selected = true;
            isSkill3Selected = false;
            isSkill4Selected = false;
            attackController.isSkillActive = true;  // 스킬 활성화 상태로 변경
        }
        // 키 입력으로 스킬 선택
        if (Input.GetKeyDown(KeyCode.Alpha3) && skill3Cooldown <= 0f)
        {
            isSkill3Selected = true;
            isSkill2Selected = false;
            isSkill4Selected = false;
            attackController.isSkillActive = true;  // 스킬 활성화 상태로 변경
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && skill4Cooldown <= 0f)
        {
            isSkill4Selected = true;
            isSkill3Selected = false;
            isSkill2Selected = false;
            attackController.isSkillActive = true;  // 스킬 활성화 상태로 변경
        }

        // 마우스 좌클릭으로 시전 위치 선택
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPos = GetMouseWorldPosition();

            if (isSkill3Selected)
            {
                Instantiate(skill3Prefab, spawnPos, Quaternion.identity);
                isSkill3Selected = false;
                // 스킬 쿨다운 시작
                skill3Cooldown = skill3CooldownTime;
                skill3Text.text = skill3Cooldown.ToString();

                DOVirtual.DelayedCall(0.2f, () =>
                {
                    attackController.isSkillActive = false;
                });  // 스킬 완료
            }
            else if (isSkill4Selected)
            {
                Instantiate(skill4Prefab, spawnPos, Quaternion.identity);
                isSkill4Selected = false;
                // 스킬 쿨다운 시작
                skill4Cooldown = skill4CooldownTime;
                skill4Text.text = skill4Cooldown.ToString();

                DOVirtual.DelayedCall(0.2f, () =>
                {
                    attackController.isSkillActive = false;
                });  // 스킬 완료
            }
            else if (isSkill2Selected)
            {
                // FireWall 모드일 때 첫 번째 클릭
                firstClickPos = spawnPos;
                CreateFireWall(firstClickPos);
                isSkill2Selected = false;
                // 스킬 쿨다운 시작
                skill2Cooldown = skill2CooldownTime;
                skill2Text.text = skill2Cooldown.ToString();
                DOVirtual.DelayedCall(0.2f, () =>
                {
                    attackController.isSkillActive = false;
                });  // 스킬 완료
            }
        }
    }
    void FixedUpdate()
    {
        // 스킬 쿨다운 업데이트
        if (skill2Cooldown > 0f)
        {
            skill2Cooldown -= Time.fixedDeltaTime;
            skill2Text.text = ((int)skill2Cooldown).ToString();
            skill2Image.fillAmount = (skill2CooldownTime - skill2Cooldown) / skill2CooldownTime;  // 쿨다운 이미지 업데이트
        }
        if (skill3Cooldown > 0f)
        {
            skill3Cooldown -= Time.fixedDeltaTime;
            skill3Text.text = ((int)skill3Cooldown).ToString();
            skill3Image.fillAmount = (skill3CooldownTime - skill3Cooldown) / skill3CooldownTime;  // 쿨다운 이미지 업데이트
        }
        if (skill4Cooldown > 0f)
        {
            skill4Cooldown -= Time.fixedDeltaTime;
            skill4Text.text = ((int)skill4Cooldown).ToString();
            skill4Image.fillAmount = (skill4CooldownTime - skill4Cooldown) / skill4CooldownTime;  // 쿨다운 이미지 업데이트
        }
    }

    // 마우스 좌표 → 월드 좌표 변환 (2D 기준)
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;
        return worldPos;
    }

    void CreateFireWall(Vector2 pos)
    {
        Instantiate(fireWallPrefab, pos, Quaternion.identity);
        Debug.Log("FireWall 생성: " + pos);
    }
}
