using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public GameObject[] targets; //������ ���� �����
    public GameObject curTarget;
    public float towerPrice = 100.0f;
    public float attackMaximumDistance = 50.0f; //��������� �����
    public float attackMinimumDistance = 5.0f;
    public float attackDamage = 10.0f; //����
    public float reloadTimer = 2.5f; //�������� ����� ����������, ���������� ��������
    public const float reloadCooldown = 2.5f; //�������� ����� ����������, ���������
    public float rotationSpeed = 1.5f; //��������� �������� �������� �����
    public int FiringOrder = 1; //����������� �������� ��� ������� (� ��� �� �� 2)

    public Transform turretHead;

    public RaycastHit Hit;

    // Start is called before the first frame update
    void Start()
    {
        turretHead = transform.Find("TurretHead"); //������� ����� � �������� ������ ������
    }

    // Update is called once per frame
    void Update()
    {
        if (curTarget != null) //���� ���������� ������� ���� �� ������
        {
            float distance = Vector3.Distance(turretHead.position, curTarget.transform.position); //������ ��������� �� ���
            if (attackMinimumDistance < distance && distance < attackMaximumDistance) //���� ��������� ������ ������� ���� � ������ ��������� ��������� �����
            {
                turretHead.rotation = Quaternion.Slerp(turretHead.rotation, Quaternion.LookRotation(curTarget.transform.position - turretHead.position), rotationSpeed * Time.deltaTime); //������� ����� � ������� ����
                if (reloadTimer > 0) reloadTimer -= Time.deltaTime; //���� ������ ����������� ������ ���� - �������� ���
                if (reloadTimer < 0) reloadTimer = 0; //���� �� ���� ������ ���� - ������������� ��� � ����
                if (reloadTimer == 0) //���� �����
                {
                    MobHP mhp = curTarget.GetComponent<MobHP>();
                    switch (FiringOrder) //�������, �� ������ ������ ��������
                    {
                        case 1:
                            if (mhp != null) mhp.ChangeHP(-attackDamage); //������� ����� ����
                            FiringOrder++; //����������� FiringOrder �� 1
                            break;
                        case 2:
                            if (mhp != null) mhp.ChangeHP(-attackDamage); //������� ����� ����
                            FiringOrder = 1; //������������� FiringOrder � ����������� �������
                            break;
                    }
                    reloadTimer = reloadCooldown; //���������� ���������� �������� � �������������� �������� �� ���������
                }
            }
        }
        else //�����
        {
            curTarget = SortTargets(); //��������� ���� � �������� �����
        }
    }
    //����� ����������� ����� ���������� �����, ���� ������������ ��� �����������!
    public GameObject SortTargets()
    {
        float closestMobDistance = 0; //������������� ���������� ��� �������� ��������� �� ����
        GameObject nearestmob = null; //������������� ���������� ���������� ����
        List<GameObject> sortingMobs = GameObject.FindGameObjectsWithTag("Monster").ToList(); //������� ���� ����� � ����� Monster � ������ ������ ��� ����������

        foreach (var everyTarget in sortingMobs) //��� ������� ���� � �������
        {
            //���� ��������� �� ���� ������, ��� closestMobDistance ��� ����� ����
            if ((Vector3.Distance(everyTarget.transform.position, turretHead.position) < closestMobDistance) || closestMobDistance == 0)
            {
                closestMobDistance = Vector3.Distance(everyTarget.transform.position, turretHead.position); //������ ��������� �� ���� �� �����, ���������� � � ����������
                nearestmob = everyTarget;//������������� ��� ��� ����������
            }
        }
        return closestMobDistance > attackMaximumDistance ? null : nearestmob; //����� �� ������� ����, �� ������� �� ����� �������
    }
}
