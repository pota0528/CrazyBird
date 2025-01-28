using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public int health = 3; // 체력
    private Animator animator; // 애니메이터 부르기
    private bool isDamaged = false; // 맞은 상태 여부

    public AudioClip enemyHitSound;
    AudioSource audioSource;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //프롭이나 새가 부딪히면 적이 데미지 입은 후 체력이 0되면 삭제
        if (collision.gameObject.CompareTag("Props") || collision.gameObject.CompareTag("Bird")) 
        {
            if (enemyHitSound != null)
            {
                audioSource.PlayOneShot(enemyHitSound); // 적이 피격받는 소리
            }
            Vector3 hitDirection = GetComponent<Rigidbody2D>().velocity * -1;
            TakeDamage();
        }
        void TakeDamage()
        {
            health--;

            if (health > 0)
            {
                if (!isDamaged && health == 4)
                {
                    SetDamagedState();
                }
            }
            else
            {
                Die();
            }
        }

        void SetDamagedState()
        {
            isDamaged = true; // "맞은 상태"로 변경
            animator.SetBool("IsDamaged", true); // 애니메이션 상태 변경
        }

        void Die()
        {
            Destroy(gameObject); // 적 제거
            EnemyManager.Instance.ReduceEnemyCount(); // 적 개수 감소 호출
        }
    }
}