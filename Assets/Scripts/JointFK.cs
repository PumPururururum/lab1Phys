using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointFK : MonoBehaviour
{ 
    public JointFK m_child;

    public float maxAngle = 100f; 

    [HideInInspector] public float pitchAngle; // local X (up/down)
    [HideInInspector] public float yawAngle;   // local Y (left/right)
    private float initialZ;

    void Start()
    {
        Vector3 e = transform.localEulerAngles;
        pitchAngle = NormalizeAngle(e.x);
        yawAngle = NormalizeAngle(e.y);
        initialZ = e.z;
        ApplyRotation(); // применить начальные значения корректно
    }

    public JointFK GetChild() => m_child;

    
    public void SetAngles(float newPitch, float newYaw)
    {
        pitchAngle = Mathf.Clamp(newPitch, -maxAngle, maxAngle);
        yawAngle = Mathf.Clamp(newYaw, -maxAngle, maxAngle);
        ApplyRotation();
    }

    // Приращение углов
    public void Rotate(float deltaPitch, float deltaYaw)
    {
        pitchAngle = Mathf.Clamp(pitchAngle + deltaPitch, -maxAngle, maxAngle);
        yawAngle = Mathf.Clamp(yawAngle + deltaYaw, -maxAngle, maxAngle);
        ApplyRotation();
    }

    // Применяем локальную ротацию жёстко (чтобы не было накопления дрейфа)
    private void ApplyRotation()
    {
        transform.localRotation = Quaternion.Euler(pitchAngle, yawAngle, initialZ);
    }

    private float NormalizeAngle(float a)
    {
        if (a > 180f) a -= 360f;
        return a;
    }

}
