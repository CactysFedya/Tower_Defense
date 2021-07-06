using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHP : MonoBehaviour
{
    public float maxHP = 100; //Максимум ХП
    public float curHP = 100; //Текущее ХП
    public Color MaxDamageColor = Color.red; //цвета полностью побитого
    public Color MinDamageColor = Color.blue; //и целого моба
    private GlobalVars gv; //поле для объекта глобальных переменных

    private void Awake()
    {
        gv = GameObject.Find("GlobalVars").GetComponent<GlobalVars>(); //инициализируем поле
        if (gv != null)
        {
            gv.MobList.Add(gameObject); //добавляем себя в общий лист мобов
            gv.MobCount++; //увеличиваем счетчик мобов
        }
        if (maxHP < 1) maxHP = 1; //если максимальное хп задано менее единицы - ставим единицу
    }

    public void ChangeHP(float adjust) //метод корректировки ХП моба
    {
        if ((curHP + adjust) > maxHP) curHP = maxHP;//если сумма текущего ХП и adjust в результате более, чем максимальное хп - текущее ХП становится равным максимальному
        else curHP += adjust; //иначе просто добавляем adjust
    }

    private void Update()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.Lerp(MaxDamageColor, MinDamageColor, curHP / maxHP); //Лерпим цвет моба по заданным в начале цветам. В примере: красный - моб почти полностью убит, синий - целый.
        if (curHP <= 0) //если ХП упало в ноль или ниже
        {
            MobAI mai = gameObject.GetComponent<MobAI>(); //подключаемся к компоненту AI моба
            if (mai != null && gv != null) gv.PlayerMoney += mai.mobPrice; //если он существует - добавляем денег игроку в размере цены за голову моба
            Destroy(gameObject); //удаляем себя
        }
    }

    private void OnDestroy() //при удалении
    {
        if (gv != null)
        {
            gv.MobList.Remove(gameObject); //удаляем себя из глобального списка мобов
            gv.MobCount--; //уменьшаем глобальный счетчик мобов на 1
        }
    }
}