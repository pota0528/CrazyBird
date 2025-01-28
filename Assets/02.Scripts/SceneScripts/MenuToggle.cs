using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    public GameObject stageListPanel; // 목록 패널 할당

    public void ToggleStageList()
    {
        // 패널이 버튼을 눌렀을때 나와야하므로, 사운드때처럼 활성 / 비활성 처리
        bool isActive = stageListPanel.activeSelf;
        stageListPanel.SetActive(!isActive);
    }

    // 목록을 닫는 함수
    public void CloseStageList()
    {
        stageListPanel.SetActive(false); // 패널 비활성화
    }
}
