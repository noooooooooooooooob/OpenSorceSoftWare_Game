using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [Header("줌 설정")]
    public float zoomedSize = 3f;
    public float zoomDuration = 0.5f;
    public float defaultZoomSize = 5f;
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 8f;

    [Header("카메라 이동 제한 (절대 좌표)")]
    public float xMin = 0f;
    public float xMax = 10f;
    public float yMin = 0f;
    public float yMax = 10f;
    [Header("카메라 이동 속도")]
    public float cameraMoveSpeed = 5f;

    private Camera cam;
    private Vector3 dragOrigin;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleRightClickDrag();
        HandleLeftClickTileSelect();
        HandleScrollZoom();
        HandleInputCameraMove();
    }
    // 입력에 따른 카메라 이동
    void HandleInputCameraMove()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            cam.transform.Translate(Vector3.up * Time.deltaTime * cameraMoveSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            cam.transform.Translate(Vector3.down * Time.deltaTime * cameraMoveSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            cam.transform.Translate(Vector3.left * Time.deltaTime * cameraMoveSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            cam.transform.Translate(Vector3.right * Time.deltaTime * cameraMoveSpeed);
        }

        // 경계 보정
        ClampCameraToBounds();
    }

    // 오른쪽 마우스 드래그 이동
    void HandleRightClickDrag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 screen = Input.mousePosition;
            screen.z = Mathf.Abs(cam.transform.position.z);
            dragOrigin = cam.ScreenToWorldPoint(screen);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 screen = Input.mousePosition;
            screen.z = Mathf.Abs(cam.transform.position.z);
            Vector3 currentPos = cam.ScreenToWorldPoint(screen);

            Vector3 diff = dragOrigin - currentPos;
            Vector3 newPos = cam.transform.position + diff;
            cam.transform.position = ClampToBounds(newPos);
        }
    }


    // 왼쪽 클릭 타일 선택 + 부드러운 카메라 이동
    void HandleLeftClickTileSelect()
    {
    }

    // 휠 줌 + 위치 보정
    void HandleScrollZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float newSize = cam.orthographicSize - scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            ClampCameraToBounds();
        }
    }

    // 경계 보정 함수
    Vector3 ClampToBounds(Vector3 target)
    {
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        float minX = xMin + horzExtent;
        float maxX = xMax - horzExtent;
        float minY = yMin + vertExtent;
        float maxY = yMax - vertExtent;

        target.x = Mathf.Clamp(target.x, minX, maxX);
        target.y = Mathf.Clamp(target.y, minY, maxY);

        return target;
    }

    void ClampCameraToBounds()
    {
        cam.transform.position = ClampToBounds(cam.transform.position);
    }
}