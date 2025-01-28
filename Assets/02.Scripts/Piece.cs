using System.Collections;
using UnityEngine;

public class Piece : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}