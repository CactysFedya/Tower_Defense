using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public List<GameObject> MobList = new List<GameObject>(); //������ ����� � ����
    public int MobCount = 0; //������� ����� � ����

    public List<GameObject> TurretList = new List<GameObject>(); //������ ����� � ����
    public int TurretCount = 0; //������� ����� � ����

    public float PlayerMoney = 200.0f; //������ ������
}
