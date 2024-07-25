using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyCube : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;     // 트라이앵글은 버텍스의 인덱스를 참조하므로 전역으로 둘 필요 없음
    private int currentVertexIndex = 0;
    public Text vertexIndexText;    // UI 텍스트를 참조하기 위한 변수

    void Start()
    {
        // MeshFilter와 MeshRenderer 컴포넌트 가져오기 또는 추가하기
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

        // 새로운 메쉬 생성
        mesh = new Mesh();
        meshFilter.mesh = mesh;



        /////---------------- 1) MeshFilter ----------------/////
        // MeshFilter는 Unity에서 3D 모델의 기하학적 데이터를 저장하는 컴포넌트
        // Mesh 객체를 통해 버텍스(꼭지점), 트라이앵글(삼각형), 노멀(법선 벡터), UV 맵 등의 정보를 보유
        // MeshFilter는 실제 렌더링 데이터(형태)를 정의


        // 1. 큐브의 꼭지점 정의
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

        // 한 꼭지점의 위치 변경
        //vertices[0] = new Vector3(-0.5f, -0.5f, -1.0f); // z축을 -1.0f로 변경하는 경우
        mesh.vertices = vertices;


        // 2. 큐브의 면(triangles) 정의
        int[] triangles = new int[36]
        {
            // 앞면
            0, 2, 1,        // 삼각형 2개
            0, 3, 2,

            // 뒷면
            4, 6, 5,
            4, 7, 6,

            // 왼쪽 면
            0, 7, 4,
            0, 4, 3,

            // 오른쪽 면
            1, 6, 7,
            1, 7, 0,

            // 윗면
            3, 5, 2,
            3, 4, 5,

            // 아랫면
            1, 5, 6,
            1, 2, 5
        };
        mesh.triangles = triangles;


        // 3. 큐브의 법선 벡터 - 자동으로 법선 구하는 경우
        mesh.RecalculateNormals();

        // 3. 큐브의 법선 벡터 - 직접 법선 벡터를 입력하는 경우
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


        // 4. UV 좌표 정의
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
       
        // UI 텍스트 업데이트
        UpdateVertexIndexText();



        /////---------------- 2) MeshRenderer ----------------/////
        // MeshRenderer는 MeshFilter에서 정의된 메쉬를 화면에 렌더링하는 역할을 함
        // 메쉬에 재질(Material)을 적용하고, 조명 및 그림자 효과를 관리
        // MeshRenderer는 메쉬가 어떻게 보여질지, 어떤 셰이더와 텍스처를 사용할지 결정합니다.

        // 1. 커스텀 재질을 설정
        Material material = new Material(Shader.Find("Custom/RainbowShader"));
        material.SetFloat("_Speed", 1.0f);
        meshRenderer.material = material;

        // 2. 조명 및 그림자 설정
        meshRenderer.receiveShadows = true;                                                         // 그림자를 받을지 여부
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;                // 그림자 생성 설정
        meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.BlendProbes; // 반사 프로브 사용 설정
        meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;           // 라이트 프로브 사용 설정
    }

    void Update()
    {
        // 스페이스바를 누르면 접근하는 버텍스 인덱스를 변경
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentVertexIndex = (currentVertexIndex + 1) % vertices.Length;
            UpdateVertexIndexText();
            Debug.Log("Current Vertex Index: " + currentVertexIndex);
        }

        // 키 입력에 따라 버텍스 위치 변경
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

        // 변경된 버텍스 위치를 메쉬에 적용
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }


    // 기즈모를 통해서 현재 잡고 있는 인덱스가 어디인지 알게 해줌(기즈모 켜야 활성화 됨)
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
