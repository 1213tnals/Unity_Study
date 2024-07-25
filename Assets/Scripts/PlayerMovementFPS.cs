using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementFPS : MonoBehaviour
{
    // �̵� �ӵ�
    public float speed = 5.0f;

    // ���콺 ����
    public float mouseSensitivity = 300.0f;
    private float xRotation = 0.0f;
    public Transform cameraTransform; // ī�޶� Transform

    void Start()
    {
        // ���콺 Ŀ�� ���̱�
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // Ű���带 �̿��� �̵�
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        movement = transform.TransformDirection(movement);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // ���콺�� �̿��� ȸ��
        if (Input.GetMouseButton(1)) // ���� ���콺 ��ư�� ������ �ִ� ���ȿ��� ȸ��
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation += mouseY; // ���Ʒ� ȸ�� ���� �ݴ�� ����
            xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f); // ī�޶��� ȸ�� ���� ����

            // ī�޶��� ���� ȸ�� ���� (���Ʒ� ȸ��)
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);

            // �÷��̾� �ٵ� ȸ�� (�¿� ȸ�� �ݴ�� ����)
            transform.Rotate(Vector3.up * -mouseX);
        }
    }
}
