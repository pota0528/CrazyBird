using System;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{
    public GameObject Piece;
    public int MinPieceCount = 1;
    public int MaxPieceCount = 1;

    private Camera mainCamera;
    public AudioClip birdHitSound; // 새가 다른 객체와 부딪히면 나는 소리
    AudioSource audioSource;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Props") || collision.gameObject.CompareTag("Enemy")
            || collision.gameObject.CompareTag("Ground"))
        {
            if (birdHitSound != null)
            {
                audioSource.PlayOneShot(birdHitSound); // 새 충돌 소리
            }
            Vector3 hitDirection = GetComponent<Rigidbody2D>().velocity * -1;

            int count = Random.Range(MinPieceCount, MaxPieceCount + 1);

            for (int i = 0; i < count; ++i)
            {
                // 랜덤 방향으로 파편 발사
                Vector3 randomDiretion = Random.insideUnitSphere;
                Vector3 lastDiretion = Quaternion.LookRotation(randomDiretion)
                                       * hitDirection;

                // 파편 생성
                GameObject instance = Instantiate(Piece, transform.position,
                    Quaternion.LookRotation(lastDiretion));
                // 파편에 힘을 가해서 발사
                instance.GetComponent<Rigidbody>().AddForce(lastDiretion, ForceMode.Impulse);
                // 파편이 일정 시간 후 삭제
                Destroy(instance, 2f);
            }

            Destroy(this.gameObject, 2f); // 새 삭제
        }
    }

    private void LateUpdate()
    {
        if (transform.position.x > mainCamera.transform.position.x)
        {
            var vector3 = mainCamera.transform.position;
            vector3.x = transform.position.x;
            mainCamera.transform.position = vector3;
        }
    }

    private void OnDestroy()
    {
        // 카메라 경고를 잡아줄 유니티에서 지원하는 키워드. 위의 새 삭제 코드와 별개로 적어놔서 카메라 오류가...
        if (!mainCamera.IsUnityNull()) 
            mainCamera.GetComponent<ReturnToPosition>().StartReturnToPosition();
    }
}