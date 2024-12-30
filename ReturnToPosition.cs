using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReturnToPosition : MonoBehaviour
{
    [SerializeField] private float returnTime;
    Vector3 startPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    public void StartReturnToPosition()
    {
        StartCoroutine(StartReturnToPositionCoroutine());
    }

    IEnumerator StartReturnToPositionCoroutine()
    {
        float duration = 0.0f;
        Vector3 initPosition = transform.position;
        while (true)
        {
            float lerp = Mathf.Min(duration, returnTime) / returnTime;
            Vector3 newPos = Vector3.Slerp(initPosition, startPosition, lerp);
            transform.position = newPos;

            if (lerp >= 1.0f)
                break;
            
            duration += Time.deltaTime;
            
            yield return new LateUpdate();
        }

        transform.position = startPosition;
    }
}
