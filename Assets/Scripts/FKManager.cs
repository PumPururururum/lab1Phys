using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FKManager : MonoBehaviour
{
    [Header("Chain")]
    public List<JointFK> joints = new List<JointFK>(); // от root к концу
    public JointFK endEffector;

    [Header("Target")]
    public Transform target; // будет перемещаться в позицию конца руки

    [Header("Target angles (degrees)")]
    public List<Vector2> targetAngles = new List<Vector2>(); // (pitch, yaw)

    void OnValidate()
    {
        // синхронизация списков
        if (targetAngles.Count != joints.Count)
        {
            targetAngles.Clear();
            for (int i = 0; i < joints.Count; i++)
                targetAngles.Add(Vector2.zero);
        }
    }

    void Update()
    {
        if (joints == null || joints.Count == 0)
            return;

        ApplyFK();
        UpdateTargetPosition();
    }

    private void ApplyFK()
    {
        int count = Mathf.Min(joints.Count, targetAngles.Count);
        for (int i = 0; i < count; i++)
        {
            joints[i].SetAngles(
                targetAngles[i].x,
                targetAngles[i].y
            );
        }
    }

    private void UpdateTargetPosition()
    {
        if (target == null || endEffector == null)
            return;

        target.position = endEffector.transform.position;
        target.rotation = endEffector.transform.rotation;
    }
}
