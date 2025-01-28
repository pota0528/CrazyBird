using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMToggle : MonoBehaviour
{
    // 음소거 상태 확인
    private bool isMuted = false;
    
    // 버튼 클릭시 음소거 전환 토글
    public void ToggleMute()
    {
        isMuted = !isMuted;
        
        //Main Camera에 달린 AudioListener의 음소거 설정
        AudioListener.volume = isMuted ? 0f : 1f;
    }
}
