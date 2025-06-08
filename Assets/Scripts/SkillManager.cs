using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject skill3Prefab;  // 둔화 지역 프리팹
    public GameObject skill4Prefab;  // 위성 폭격 프리팹

    private bool isSkill3Selected = false;
    private bool isSkill4Selected = false;

    void Update()
    {
        // 키 입력으로 스킬 선택
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            isSkill3Selected = true;
            isSkill4Selected = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            isSkill3Selected = false;
            isSkill4Selected = true;
        }

        // 마우스 좌클릭으로 시전 위치 선택
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPos = GetMouseWorldPosition();

            if (isSkill3Selected)
            {
                Instantiate(skill3Prefab, spawnPos, Quaternion.identity);
                isSkill3Selected = false;
            }
            else if (isSkill4Selected)
            {
                Instantiate(skill4Prefab, spawnPos, Quaternion.identity);
                isSkill4Selected = false;
            }
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
}

