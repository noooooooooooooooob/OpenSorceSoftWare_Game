using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    [Header("Crosshair Sprites")]
    public Sprite crosshair0;
    public Sprite crosshair1;
    public Sprite crosshair2;
    public Sprite crosshair3;
    public Sprite crosshair4;

    [Header("Hit Marker Sprites")]
    public Sprite normalHitMarker;   // 일반 공격 히트마커
    public Sprite critHitMarker;     // 크리티컬 히트마커

    private int selectedSkill = 0;

    // 조준점 이미지
    private Image crosshairImage;
    // 히트마커 이미지
    private Image hitMarkerImage;
    // 메인 카메라
    private Camera mainCamera;

    private int crosshairClickCount = 0;

    void Start()
    {
        // 자식 오브젝트에서 Crosshair, HitMarker 이미지 찾기
        crosshairImage = transform.Find("Image - Crosshair").GetComponent<Image>();
        hitMarkerImage = transform.Find("Image - HitMarker").GetComponent<Image>();

        // 히트마커 처음에 비활성화
        hitMarkerImage.enabled = false;
        // 메인 카메라 참조
        mainCamera = Camera.main;

        // 👉 마우스 커서 숨기기
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        transform.position = mousePos;

        if (Input.GetKeyDown(KeyCode.Alpha0)) ChangeCrosshair(0);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResetCrosshairClick();
            selectedSkill = 1;
            FindFirstObjectByType<AttackController>().ActivateAttackSpeedBuff();
            ChangeCrosshair(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetCrosshairClick();
            selectedSkill = 2;
            ChangeCrosshair(2);
            crosshairClickCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResetCrosshairClick();
            selectedSkill = 3;
            ChangeCrosshair(3);
            crosshairClickCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ResetCrosshairClick();
            selectedSkill = 4;
            ChangeCrosshair(4);
            crosshairClickCount = 0;
        }

        if (crosshairClickCount == 1 && Input.GetMouseButtonDown(0))
        {
            ChangeCrosshair(0);
            crosshairClickCount = 0;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedSkill == 2)
            {
                crosshairClickCount++;
                if (crosshairClickCount == 2)
                {
                    ChangeCrosshair(0);
                    crosshairClickCount = 0;
                }
            }
            else if (selectedSkill == 3 || selectedSkill == 4)
            {
                ChangeCrosshair(0);
                crosshairClickCount = 0;
            }
        }
    }


    private void ResetCrosshairClick()
    {
        crosshairClickCount = 0;
    }

    // 히트마커 표시 메서드
    public void ShowHitMarker(bool isCritical)
    {
        // 크리티컬 여부에 따라 히트마커 이미지 변경
        hitMarkerImage.sprite = isCritical ? critHitMarker : normalHitMarker;
        // 히트마커 표시
        hitMarkerImage.enabled = true;
        // 0.3초 뒤 히트마커 숨김
        Invoke(nameof(HideHitMarker), 0.3f);
    }

    // 히트마커 숨김 메서드
    private void HideHitMarker()
    {
        hitMarkerImage.enabled = false;
    }
    public void ChangeCrosshair(int number)
    {
        switch (number)
        {
            case 0:
                crosshairImage.sprite = crosshair0;
                break;
            case 1:
                crosshairImage.sprite = crosshair1;
                break;
            case 2:
                crosshairImage.sprite = crosshair2;
                break;
            case 3:
                crosshairImage.sprite = crosshair3;
                break;
            case 4:
                crosshairImage.sprite = crosshair4;
                break;
        }
    }

    public void ChangeCrosshairToDefault()
    {
        ChangeCrosshair(0);
        selectedSkill = 0;
    }
}
