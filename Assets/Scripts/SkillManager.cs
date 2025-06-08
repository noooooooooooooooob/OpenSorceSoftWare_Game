using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject fireWallPrefab;  // �� �庮 ������

    private bool isFireWallMode = false;
    private Vector2 firstClickPos;
    private bool waitingForSecondClick = false;

    void Update()
    {
        // 2�� Ű ������ ��ų ��� ����
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isFireWallMode = true;
            waitingForSecondClick = false;
            Debug.Log("FireWall ��� ON: ù ��° Ŭ���� �ϼ���.");
        }

        // ��ų ��� �߿� ���콺 Ŭ�� ����
        if (isFireWallMode && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (!waitingForSecondClick)
            {
                // ù ��° Ŭ��: ������ ����
                firstClickPos = mousePos;
                waitingForSecondClick = true;
                Debug.Log("ù ��° Ŭ�� �Ϸ�: " + firstClickPos);
            }
            else
            {
                // �� ��° Ŭ��: ���� �����ϰ� �� �庮 ����
                Vector2 secondClickPos = mousePos;
                CreateFireWall(firstClickPos, secondClickPos);

                // ��ų ��� ����
                isFireWallMode = false;
                waitingForSecondClick = false;
                Debug.Log("FireWall ���� �Ϸ�");
            }
        }
    }

    void CreateFireWall(Vector2 start, Vector2 end)
    {
        // �� �� ���� �߰� ����
        Vector2 midPoint = (start + end) / 2f;
        // �� �� ���� �Ÿ� (����)
        float distance = Vector2.Distance(start, end);
        // �� �� ���� (���� ���)
        Vector2 direction = end - start;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ������ ����
        GameObject wall = Instantiate(fireWallPrefab, midPoint, Quaternion.Euler(0, 0, angle));
        // Scale ���� (X�� ���̸� �Ÿ���ŭ ����)
        wall.transform.localScale = new Vector3(distance, wall.transform.localScale.y, wall.transform.localScale.z);
    }
}
