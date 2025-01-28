using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BirdChanger : MonoBehaviour
{
    public SlingShot slingShot;

    // 버튼에 연결해줄 함수
    public void SwitchBird(GameObject birdPrefab)
    {
        if (slingShot != null)
        {
            slingShot.SwitchBird(birdPrefab); // slingshot의 SwitchBird 호출
        }
    }
}
