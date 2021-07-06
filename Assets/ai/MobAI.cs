using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAI : MonoBehaviour
{
    public GameObject Target; //������� ����

    public float mobPrice = 5.0f; //���� �� �������� ����
    public float mobMinSpeed = 0.5f; //����������� �������� ����
    public float mobMaxSpeed = 2.0f; //������������ �������� ����
    public float mobRotationSpeed = 2.5f; //�������� �������� ����
    public float attackDistance = 5.0f; //��������� �����
    public float damage = 5; //����, ��������� �����
    public float attackTimer = 0.0f; //���������� ������� �������� ����� �������
    public const float coolDown = 2.0f; //���������, ������������ ��� ������ ������� ����� � ��������� ��������

    private float MobCurrentSpeed; //�������� ����, �������������� �����
    private Transform mob; //���������� ��� ���������� ����
    private GlobalVars gv; //���� ��� ������� ���������� ����������

    private void Awake()
    {
        gv = GameObject.Find("GlobalVars").GetComponent<GlobalVars>(); //�������������� ����
        mob = transform; //����������� ��������� ���� � ���������� (�������� ������������������)
        MobCurrentSpeed = Random.Range(mobMinSpeed, mobMaxSpeed); //����������� ������� �������� �������� ����� ���������� � ����������� ���������
    }

    private void Update()
    {
        if (Target == null) //���� ���� ��� ���
        {
            Target = SortTargets(); //�������� ������� � �� ������ ������
        }
        else //���� � ��� ���� ����
        {
            mob.rotation = Quaternion.Lerp(mob.rotation, Quaternion.LookRotation(new Vector3(Target.transform.position.x, 0.0f, Target.transform.position.z) - new Vector3(mob.position.x, 0.0f, mob.position.z)), mobRotationSpeed); //�������-�������, ��������� � ����� �������!
            mob.position += mob.forward * MobCurrentSpeed * Time.deltaTime; //������� � �������, ���� ������� ���
            float distance = Vector3.Distance(Target.transform.position, mob.position); //������ ��������� �� ����
            Vector3 structDirection = (Target.transform.position - mob.position).normalized; //�������� ������ �����������
            float attackDirection = Vector3.Dot(structDirection, mob.forward); //�������� ������ �����
            if (distance < attackDistance && attackDirection > 0) //���� �� �� ��������� ����� � ���� ����� ����
            {
                if (attackTimer > 0) attackTimer -= Time.deltaTime; //���� ������ ����� ������ 0 - �������� ���
                if (attackTimer <= 0) //���� �� �� ���� ������ ���� ��� ����� ���
                {
                    TurretHP thp = Target.GetComponent<TurretHP>(); //������������ � ���������� �� ����
                    if (thp != null) thp.ChangeHP(-damage); //���� ���� ��� �����, ������� ����� (�� ����� �� ���� ���� �� ����, ������ �������� ����������)
                    attackTimer = coolDown; //���������� ������ � �������� ���������
                }
            }
        }
    }
    //����� ����������� ����� ���������� �����, ���� ������������ ��� �����������!
    private GameObject SortTargets()
    {
        float closestTurretDistance = 0; //������������� ���������� ��� �������� ��������� �� �����
        GameObject nearestTurret = null; //������������� ���������� ��������� �����
        List<GameObject> sortingTurrets = gv.TurretList; //����� ������ ��� ����������

        foreach (var turret in sortingTurrets) //��� ������ ����� � �������
        {
            //���� ��������� �� ����� ������, ��� closestTurretDistance ��� ����� ����
            if ((Vector3.Distance(mob.position, turret.transform.position) < closestTurretDistance) || closestTurretDistance == 0)
            {
                closestTurretDistance = Vector3.Distance(mob.position, turret.transform.position); //������ ��������� �� ���� �� �����, ���������� � � ����������
                nearestTurret = turret;//������������� � ��� ����������
            }
        }
        return nearestTurret; //���������� ��������� �����
    }
}
