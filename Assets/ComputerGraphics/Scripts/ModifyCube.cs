using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyCube : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;     // Ʈ���̾ޱ��� ���ؽ��� �ε����� �����ϹǷ� �������� �� �ʿ� ����
    private int currentVertexIndex = 0;
    public Text vertexIndexText;    // UI �ؽ�Ʈ�� �����ϱ� ���� ����

    void Start()
    {
        // MeshFilter�� MeshRenderer ������Ʈ �������� �Ǵ� �߰��ϱ�
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        // ���ο� �޽� ����
        mesh = new Mesh();
        meshFilter.mesh = mesh;



        /////---------------- 1) MeshFilter ----------------/////
        // MeshFilter�� Unity���� 3D ���� �������� �����͸� �����ϴ� ������Ʈ
        // Mesh ��ü�� ���� ���ؽ�(������), Ʈ���̾ޱ�(�ﰢ��), ���(���� ����), UV �� ���� ������ ����
        // MeshFilter�� ���� ������ ������(����)�� ����


        // 1. ť���� ������ ����
        vertices = new Vector3[8]
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f)
        };

        // �� �������� ��ġ ����
        //vertices[0] = new Vector3(-0.5f, -0.5f, -1.0f); // z���� -1.0f�� �����ϴ� ���
        mesh.vertices = vertices;


        // 2. ť���� ��(triangles) ����
        int[] triangles = new int[36]
        {
            // �ո�
            0, 2, 1,        // �ﰢ�� 2��
            0, 3, 2,

            // �޸�
            4, 6, 5,
            4, 7, 6,

            // ���� ��
            0, 7, 4,
            0, 4, 3,

            // ������ ��
            1, 6, 7,
            1, 7, 0,

            // ����
            3, 5, 2,
            3, 4, 5,

            // �Ʒ���
            1, 5, 6,
            1, 2, 5
        };
        mesh.triangles = triangles;


        // 3. ť���� ���� ���� - �ڵ����� ���� ���ϴ� ���
        mesh.RecalculateNormals();

        // 3. ť���� ���� ���� - ���� ���� ���͸� �Է��ϴ� ���
        //Vector3[] normals = new Vector3[8]
        //{
        //    -Vector3.forward,
        //    -Vector3.forward,
        //    Vector3.forward,
        //    Vector3.forward,
        //    Vector3.up,
        //    Vector3.up,
        //    -Vector3.up,
        //    -Vector3.up
        //};
        //mesh.normals = normals;


        // 4. UV ��ǥ ����
        Vector2[] uv = new Vector2[8]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };
        mesh.uv = uv;
        /////-----------------------------------------------/////
       
        // UI �ؽ�Ʈ ������Ʈ
        UpdateVertexIndexText();



        /////---------------- 2) MeshRenderer ----------------/////
        // MeshRenderer�� MeshFilter���� ���ǵ� �޽��� ȭ�鿡 �������ϴ� ������ ��
        // �޽��� ����(Material)�� �����ϰ�, ���� �� �׸��� ȿ���� ����
        // MeshRenderer�� �޽��� ��� ��������, � ���̴��� �ؽ�ó�� ������� �����մϴ�.

        // 1. Ŀ���� ������ ����
        Material material = new Material(Shader.Find("Custom/RainbowShader"));
        material.SetFloat("_Speed", 1.0f);
        meshRenderer.material = material;

        // 2. ���� �� �׸��� ����
        meshRenderer.receiveShadows = true;                                                         // �׸��ڸ� ������ ����
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;                // �׸��� ���� ����
        meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.BlendProbes; // �ݻ� ���κ� ��� ����
        meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;           // ����Ʈ ���κ� ��� ����
    }

    void Update()
    {
        // �����̽��ٸ� ������ �����ϴ� ���ؽ� �ε����� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentVertexIndex = (currentVertexIndex + 1) % vertices.Length;
            UpdateVertexIndexText();
            Debug.Log("Current Vertex Index: " + currentVertexIndex);
        }

        // Ű �Է¿� ���� ���ؽ� ��ġ ����
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            vertices[currentVertexIndex].x -= 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            vertices[currentVertexIndex].x += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            vertices[currentVertexIndex].y -= 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            vertices[currentVertexIndex].y += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            vertices[currentVertexIndex].z -= 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            vertices[currentVertexIndex].z += 0.1f;
        }

        // ����� ���ؽ� ��ġ�� �޽��� ����
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }


    // ����� ���ؼ� ���� ��� �ִ� �ε����� ������� �˰� ����(����� �Ѿ� Ȱ��ȭ ��)
    void OnDrawGizmos()
    {
        if (vertices != null && vertices.Length > 0)
        {
            Gizmos.color = Color.red;
            Vector3 worldPosition = transform.TransformPoint(vertices[currentVertexIndex]);
            Gizmos.DrawSphere(worldPosition, 0.1f);
        }
    }

    void UpdateVertexIndexText()
    {
        if (vertexIndexText != null)
        {
            vertexIndexText.text = "Current Vertex Index: " + currentVertexIndex + "\n" +
                                   "Vertex Position: " + vertices[currentVertexIndex].ToString("F2") + "\n\n" +
                                   "Controls:\n" +
                                   "Space: Change Vertex\n" +
                                   "1(Keypad1): X -0.1\n" +
                                   "3(Keypad3): X +0.1\n" +
                                   "4(Keypad4): Y -0.1\n" +
                                   "6(Keypad6): Y +0.1\n" +
                                   "7(Keypad7): Z -0.1\n" +
                                   "9(Keypad9): Z +0.1";
        }
    }
}
