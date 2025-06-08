using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject fireWallPrefab;  // 불 장벽 프리팹

    private bool isFireWallMode = false;
    private Vector2 firstClickPos;
    private bool waitingForSecondClick = false;

    void Update()
    {
        // 2번 키 누르면 스킬 모드 진입
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isFireWallMode = true;
            waitingForSecondClick = false;
            Debug.Log("FireWall 모드 ON: 첫 번째 클릭을 하세요.");
        }

        // 스킬 모드 중에 마우스 클릭 감지
        if (isFireWallMode && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (!waitingForSecondClick)
            {
                // 첫 번째 클릭: 시작점 저장
                firstClickPos = mousePos;
                waitingForSecondClick = true;
                Debug.Log("첫 번째 클릭 완료: " + firstClickPos);
            }
            else
            {
                // 두 번째 클릭: 끝점 저장하고 불 장벽 생성
                Vector2 secondClickPos = mousePos;
                CreateFireWall(firstClickPos, secondClickPos);

                // 스킬 모드 종료
                isFireWallMode = false;
                waitingForSecondClick = false;
                Debug.Log("FireWall 생성 완료");
            }
        }
    }

    void CreateFireWall(Vector2 start, Vector2 end)
    {
        // 두 점 사이 중간 지점
        Vector2 midPoint = (start + end) / 2f;
        // 두 점 사이 거리 (길이)
        float distance = Vector2.Distance(start, end);
        // 두 점 방향 (각도 계산)
        Vector2 direction = end - start;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 프리팹 생성
        GameObject wall = Instantiate(fireWallPrefab, midPoint, Quaternion.Euler(0, 0, angle));
        // Scale 조정 (X축 길이를 거리만큼 조정)
        wall.transform.localScale = new Vector3(distance, wall.transform.localScale.y, wall.transform.localScale.z);
    }
}
