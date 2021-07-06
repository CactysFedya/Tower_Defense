using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    public List<GameObject> MobList = new List<GameObject>(); //массив мобов в игре
    public int MobCount = 0; //счетчик мобов в игре

    public List<GameObject> TurretList = new List<GameObject>(); //массив пушек в игре
    public int TurretCount = 0; //счетчик пушек в игре

    public float PlayerMoney = 200.0f; //деньги игрока
}
