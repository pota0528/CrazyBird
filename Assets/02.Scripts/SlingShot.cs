using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    public LineRenderer[] lineRenderers; // 라인렌더러
    public Transform[] stripPositions; // 줄의 위치 설정 (왼쪽, 오른쪽)
    public Transform center; // 새총의 중심점
    public Transform idlePosition; // 게임 시작시, 새와 줄이 당겨져있는 듯한 느낌을 주기위한 위치설정
    
    public Vector3 currentPosition;
    
    public float maxLength; 

    public float bottomBoundary; 
     
    private bool isMouseDown; // 마우스를 눌렀는지 확인
    
    public GameObject birdPrefab; // 현재 새 프리팹

    public float birdPositionOffset;

    private Rigidbody2D bird;
    private Collider2D birdCollider;

    public float force;
    
    public int maxStep = 20; // 궤적의 최대 점 수, 클수록 궤적 길어짐.
    public float timeStep = 0.1f; // 궤적 계산 시간 설정, 작을수록 점이 촘촘히 생김.
    
    public GameObject trajectoryPrefab; // 궤적을 표시할 오브젝트

    // 궤적을 표시할 오브젝트의 리스트. 나중에 궤적을 지우는데에 이 리스트 활용.
    private List<GameObject> trajectoryObjects = new List<GameObject>();

    public AudioClip dragSound; // 마우스를 당길 때 날 소리
    public AudioClip releaseSound; // 마우스를 놓을 때 날 소리
    public AudioClip birdSound; // 날아갈때 새 소리
    AudioSource audioSource;
    
    void Start()
    {
        lineRenderers[0].positionCount = 2;
        lineRenderers[1].positionCount = 2;
        lineRenderers[0].SetPosition(0, stripPositions[0].position);
        lineRenderers[1].SetPosition(0, stripPositions[1].position);
        audioSource = GetComponent<AudioSource>(); // Audiosource 컴포넌트 참조
        
        CreateBird(); // 첫번째 새 생성
    }

    public void SwitchBird(GameObject newBirdPrefab) // 새를 교체하는 함수
    {
        birdPrefab = newBirdPrefab; // 새 프리팹으로 교체
        Destroy(bird?.gameObject); // 현재 새 삭제
        CreateBird(); //새로운 새 생성
    }

    void CreateBird()
    {
        bird = Instantiate(birdPrefab).GetComponent<Rigidbody2D>();
        birdCollider = bird.GetComponent<Collider2D>();
        birdCollider.enabled = false;
        
        bird.isKinematic = true;
        
        ResetStrips(); // 새의 위치도 같이 초기화됨
    }

    void Update()
    {
        if (isMouseDown) // 마우스를 누를때 위치 계산
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            
            currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition
            - center.position, maxLength);

            currentPosition = ClampBoundary(currentPosition);
            
            SetStrips(currentPosition);

            if (birdCollider)
            {
                birdCollider.enabled = true;
            }
            
            Vector3 calculatedForce = (currentPosition - center.position) * force;
            DisplayTrajectory(calculatedForce);  // 궤적 계산 및 표시
        }
        else
        {
            ResetStrips();
            ClearTrajectory(); // 마우스를 놓으면 궤적 삭제
        }
        
    }

    void OnMouseDown()
    {
        isMouseDown = true;

        if (dragSound != null)
        {
            audioSource.PlayOneShot(dragSound);
        }
    }

    void OnMouseUp()
    {
        isMouseDown = false;

        if (releaseSound != null && birdSound != null)
        {
            audioSource.PlayOneShot(releaseSound);
            audioSource.PlayOneShot(birdSound);
        }
        Shoot();
        ClearTrajectory();
    }

    void Shoot()
    {
        bird.isKinematic = false;
        Vector3 birdForce = (currentPosition - center.position) * force * -1;
        bird.velocity = birdForce;
        
        bird = null;
        birdCollider = null;
        Invoke("CreateBird", 2); // 새로운 새 생성 예약도 같이 됨
    }
    
    void ResetStrips()
    {
        currentPosition = idlePosition.position;
        SetStrips(currentPosition);
    }

    void SetStrips(Vector3 position)
    {
        lineRenderers[0].SetPosition(1, position);
        lineRenderers[1].SetPosition(1, position);

        if (bird)
        {
            Vector3 dir = position - center.position;
            bird.transform.position = position + dir.normalized * birdPositionOffset;
            bird.transform.right = -dir.normalized;
        }
    }
    
    Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, bottomBoundary, 1000);
        return vector;
    }
    
    void DisplayTrajectory(Vector3 force)
    {
        // 기존 궤적 삭제
        foreach (var trajectoryObject in trajectoryObjects)
        {
            Destroy(trajectoryObject); // 궤적 오브젝트 삭제
        }
        trajectoryObjects.Clear(); // 리스트 초기화

        Vector3 position = center.position; // 궤적의 시작점 (새총 중간)
        Vector3 velocity = -force / bird.mass; // (초반 속도 계산) 당기는 반대 방향으로 나오게 설정
        // F = ma -> a = F/m
        
        for (int i = 0; i < maxStep; i++) // 궤적의 점을 최대 단계 수만큼 계산
        {
            float timeElapsed = timeStep * i; // 특정 궤적의 점에서 경과한 시간
            // 포물선 공식으로 궤적 점을 계산
            Vector3 trajectoryPoint = position +
                                      velocity * timeElapsed +
                                      Physics.gravity * 0.5f * timeElapsed * timeElapsed;
            
            // 궤적 공식 : 위치 = 초기위치 + 초기속도 * 시간 + 중력효과 * 시간² / 2
            // 계산된 위치에다가 궤적 오브젝트 생성
            var trajectoryObject = Instantiate(trajectoryPrefab, trajectoryPoint, Quaternion.identity);
            trajectoryObjects.Add(trajectoryObject); // 리스트에 추가
        }
    }
    
    // 궤적 지우기
    void ClearTrajectory()
    {
        // 생성된 궤적 오브젝트를 삭제
        foreach (var trajectoryObject in trajectoryObjects)
        {
            Destroy(trajectoryObject); // 오브젝트 삭제
        }
        trajectoryObjects.Clear(); // 리스트 초기화
    }
}
