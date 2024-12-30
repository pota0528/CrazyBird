using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance; // 싱글톤 사용

    private int enemyCount = 0;

    public GameObject clearPanel; // 클리어 패널
    
    public AudioClip clearSound;
    AudioSource audioSource;
    
    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource가 이 오브젝트에 없습니다. AudioSource를 추가하세요.");
            return;
        }
        
        // 초반 적 개수를 설정
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        clearPanel.SetActive(false); // 게임 중에는 클리어 패널 숨기기
    }

    public void ReduceEnemyCount()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            ShowClearUI();
        }
    }

    void ShowClearUI()
    {
        clearPanel.SetActive(true); // 클리어 패널 표시
        if (audioSource != null && clearSound != null)
        {
            audioSource.PlayOneShot(clearSound);
        }
        else
        {
            Debug.LogWarning("AudioSource 또는 clearSound가 설정되지 않았습니다.");
        }
    }
}
