using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHP : MonoBehaviour
{
    public float maxHP = 100; //�������� ��
    public float curHP = 100; //������� ��
    private GlobalVars gv; //���� ��� ������� ���������� ����������

    private void Awake()
    {
        gv = GameObject.Find("GlobalVars").GetComponent<GlobalVars>(); //�������������� ����
        if (gv != null)
        {
            gv.TurretList.Add(gameObject);
            gv.TurretCount++;
        }
        if (maxHP < 1) maxHP = 1;
    }

    public void ChangeHP(float adjust)
    {
        if ((curHP + adjust) > maxHP) curHP = maxHP;
        else curHP += adjust;
        if (curHP > maxHP) curHP = maxHP;
    }

    private void Update()
    {
        if (curHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (gv != null)
        {
            gv.TurretList.Remove(gameObject);
            gv.TurretCount--;
        }
    }
}

