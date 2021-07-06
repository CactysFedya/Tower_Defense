using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public GameObject[] targets; //массив всех целей
    public GameObject curTarget;
    public float towerPrice = 100.0f;
    public float attackMaximumDistance = 50.0f; //дистанция атаки
    public float attackMinimumDistance = 5.0f;
    public float attackDamage = 10.0f; //урон
    public float reloadTimer = 2.5f; //задержка между выстрелами, изменяемое значение
    public const float reloadCooldown = 2.5f; //задержка между выстрелами, константа
    public float rotationSpeed = 1.5f; //множитель скорости вращения башни
    public int FiringOrder = 1; //очередность стрельбы для стволов (у нас же их 2)

    public Transform turretHead;

    public RaycastHit Hit;

    // Start is called before the first frame update
    void Start()
    {
        turretHead = transform.Find("TurretHead"); //находим башню в иерархии частей модели
    }

    // Update is called once per frame
    void Update()
    {
        if (curTarget != null) //если переменная текущей цели не пустая
        {
            float distance = Vector3.Distance(turretHead.position, curTarget.transform.position); //меряем дистанцию до нее
            if (attackMinimumDistance < distance && distance < attackMaximumDistance) //если дистанция больше мертвой зоны и меньше дистанции поражения пушки
            {
                turretHead.rotation = Quaternion.Slerp(turretHead.rotation, Quaternion.LookRotation(curTarget.transform.position - turretHead.position), rotationSpeed * Time.deltaTime); //вращаем башню в сторону цели
                if (reloadTimer > 0) reloadTimer -= Time.deltaTime; //если таймер перезарядки больше нуля - отнимаем его
                if (reloadTimer < 0) reloadTimer = 0; //если он стал меньше нуля - устанавливаем его в ноль
                if (reloadTimer == 0) //став нулем
                {
                    MobHP mhp = curTarget.GetComponent<MobHP>();
                    switch (FiringOrder) //смотрим, из какого ствола стрелять
                    {
                        case 1:
                            if (mhp != null) mhp.ChangeHP(-attackDamage); //наносим дамаг цели
                            FiringOrder++; //увеличиваем FiringOrder на 1
                            break;
                        case 2:
                            if (mhp != null) mhp.ChangeHP(-attackDamage); //наносим дамаг цели
                            FiringOrder = 1; //устанавливаем FiringOrder в изначальную позицию
                            break;
                    }
                    reloadTimer = reloadCooldown; //возвращаем переменной задержки её первоначальное значение из константы
                }
            }
        }
        else //иначе
        {
            curTarget = SortTargets(); //сортируем цели и получаем новую
        }
    }
    //Очень примитивный метод сортировки целей, море возможностей для модификации!
    public GameObject SortTargets()
    {
        float closestMobDistance = 0; //инициализация переменной для проверки дистанции до моба
        GameObject nearestmob = null; //инициализация переменной ближайшего моба
        List<GameObject> sortingMobs = GameObject.FindGameObjectsWithTag("Monster").ToList(); //находим всех мобов с тегом Monster и создаём массив для сортировки

        foreach (var everyTarget in sortingMobs) //для каждого моба в массиве
        {
            //если дистанция до моба меньше, чем closestMobDistance или равна нулю
            if ((Vector3.Distance(everyTarget.transform.position, turretHead.position) < closestMobDistance) || closestMobDistance == 0)
            {
                closestMobDistance = Vector3.Distance(everyTarget.transform.position, turretHead.position); //Меряем дистанцию от моба до пушки, записываем её в переменную
                nearestmob = everyTarget;//устанавливаем его как ближайшего
            }
        }
        return closestMobDistance > attackMaximumDistance ? null : nearestmob; //пушка не схватит цель, до которой не может достать
    }
}
