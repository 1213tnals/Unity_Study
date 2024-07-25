using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementFPS : MonoBehaviour
{
    // 이동 속도
    public float speed = 5.0f;

    // 마우스 감도
    public float mouseSensitivity = 300.0f;
    private float xRotation = 0.0f;
    public Transform cameraTransform; // 카메라 Transform

    void Start()
    {
        // 마우스 커서 보이기
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // 키보드를 이용한 이동
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        movement = transform.TransformDirection(movement);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // 마우스를 이용한 회전
        if (Input.GetMouseButton(1)) // 우측 마우스 버튼을 누르고 있는 동안에만 회전
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation += mouseY; // 위아래 회전 방향 반대로 설정
            xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f); // 카메라의 회전 범위 제한

            // 카메라의 로컬 회전 설정 (위아래 회전)
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

            // 플레이어 바디 회전 (좌우 회전 반대로 설정)
            transform.Rotate(Vector3.up * -mouseX);
        }
    }
}
