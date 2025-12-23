using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : MonoBehaviour
{
    public Joint m_child;
    public float maxAngle = 100f;

    [HideInInspector] public float pitchAngle;
    [HideInInspector] public float yawAngle;
    private float initialZ;

    void Start()
    {
        Vector3 e = transform.localEulerAngles;
        // localEulerAngles äà¸ò çíà÷åíèÿ â 0..360; ïğèâîäèì â -180..180
        pitchAngle = NormalizeAngle(e.x);
        yawAngle = NormalizeAngle(e.y);
        initialZ = e.z;
    }
    public Joint GetChild()
    {
        return m_child;
    }
    public void Rotate(float deltaPitch, float deltaYaw)
    {
        pitchAngle += deltaPitch;
        yawAngle += deltaYaw;

      
        pitchAngle = Mathf.Clamp(pitchAngle, -maxAngle, maxAngle);
        yawAngle = Mathf.Clamp(yawAngle, -maxAngle, maxAngle);

        
        transform.localRotation = Quaternion.Euler(pitchAngle, yawAngle, initialZ);
    }

    private float NormalizeAngle(float a)
    {
        if (a > 180f) a -= 360f;
        return a;
    }
}
