using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGenerator : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;

    void Update()
    {
        if (transform.childCount <= 0)
        {
            _ = Instantiate(targetPrefab, new Vector3
            (
                Random.Range(-7f, 7f),
                Random.Range(-6f, 0f)
            ), Quaternion.identity, transform);
        }
    }
}
