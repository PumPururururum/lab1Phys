using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKManager : MonoBehaviour
{
    public Joint m_root;
    public Joint m_end;
    public Transform m_target;
    public float treshhold = 0.05f;

    [Header("Solver settings")]
    public float m_rate = 5f;    // скорость/коэффициент шага (попробуй 1..20)
    public int m_steps = 20;     // количество итераций за кадр
    public float deltaTheta = 0.1f; // малое приращение в градусах дл€ численного дифференцировани€

    void Update()
    {
        if (m_root == null || m_end == null || m_target == null) return;

        // ¬ыполн€ем несколько шагов градиентного спуска за кадр
        for (int s = 0; s < m_steps; s++)
        {
            if (Vector3.Distance(m_end.transform.position, m_target.position) <= treshhold) break;

            Joint current = m_root;
            while (current != null)
            {
                // считаем градиенты по pitch и yaw отдельно
                float slopePitch = CalculateSlope(current, true);
                float slopeYaw = CalculateSlope(current, false);

                // примен€ем шаг (знак минус Ч движение в направлении уменьшени€ рассто€ни€)
                float applyPitch = -slopePitch * m_rate;
                float applyYaw = -slopeYaw * m_rate;

                // Ќебольша€ стабилизаци€: ограничиваем величину шага, чтобы не перепрыгивать
                float maxStep = 10f; // градусы за обновление Ч можно уменьшить
                applyPitch = Mathf.Clamp(applyPitch, -maxStep, maxStep);
                applyYaw = Mathf.Clamp(applyYaw, -maxStep, maxStep);

                current.Rotate(applyPitch, applyYaw);

                current = current.GetChild();
            }
        }
    }

    // ≈сли isPitch == true Ч считаем d(distance)/d(pitch), иначе d(distance)/d(yaw)
    float CalculateSlope(Joint _joint, bool isPitch)
    {
        // исходное рассто€ние
        float distance1 = Vector3.Distance(m_end.transform.position, m_target.position);

        // примен€ем малое приращение
        if (isPitch)
            _joint.Rotate(deltaTheta, 0f);
        else
            _joint.Rotate(0f, deltaTheta);

        // нова€ дистанци€
        float distance2 = Vector3.Distance(m_end.transform.position, m_target.position);

        // откатываем на -delta (Rotate сам сделает clamp при необходимости)
        if (isPitch)
            _joint.Rotate(-deltaTheta, 0f);
        else
            _joint.Rotate(0f, -deltaTheta);

        // производна€ (правосторонн€€ разностна€ аппроксимаци€)
        return (distance2 - distance1) / deltaTheta;
    }
}
