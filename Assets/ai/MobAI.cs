using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAI : MonoBehaviour
{
    public GameObject Target; //текущая цель

    public float mobPrice = 5.0f; //цена за убийство моба
    public float mobMinSpeed = 0.5f; //минимальная скорость моба
    public float mobMaxSpeed = 2.0f; //максимальная скорость моба
    public float mobRotationSpeed = 2.5f; //скорость поворота моба
    public float attackDistance = 5.0f; //дистанция атаки
    public float damage = 5; //урон, наносимый мобом
    public float attackTimer = 0.0f; //переменная расчета задержки между ударами
    public const float coolDown = 2.0f; //константа, используется для сброса таймера атаки в начальное значение

    private float MobCurrentSpeed; //скорость моба, инициализируем позже
    private Transform mob; //переменная для трансформа моба
    private GlobalVars gv; //поле для объекта глобальных переменных

    private void Awake()
    {
        gv = GameObject.Find("GlobalVars").GetComponent<GlobalVars>(); //инициализируем поле
        mob = transform; //присваиваем трансформ моба в переменную (повышает производительность)
        MobCurrentSpeed = Random.Range(mobMinSpeed, mobMaxSpeed); //посредством рандома выбираем скорость между минимально и максимально указанной
    }

    private void Update()
    {
        if (Target == null) //если цели ещё нет
        {
            Target = SortTargets(); //пытаемся достать её из общего списка
        }
        else //если у нас есть цель
        {
            mob.rotation = Quaternion.Lerp(mob.rotation, Quaternion.LookRotation(new Vector3(Target.transform.position.x, 0.0f, Target.transform.position.z) - new Vector3(mob.position.x, 0.0f, mob.position.z)), mobRotationSpeed); //избушка-избушка, повернись к пушке передом!
            mob.position += mob.forward * MobCurrentSpeed * Time.deltaTime; //двигаем в сторону, куда смотрит моб
            float distance = Vector3.Distance(Target.transform.position, mob.position); //меряем дистанцию до цели
            Vector3 structDirection = (Target.transform.position - mob.position).normalized; //получаем вектор направления
            float attackDirection = Vector3.Dot(structDirection, mob.forward); //получаем вектор атаки
            if (distance < attackDistance && attackDirection > 0) //если мы на дистанции атаки и цель перед нами
            {
                if (attackTimer > 0) attackTimer -= Time.deltaTime; //если таймер атаки больше 0 - отнимаем его
                if (attackTimer <= 0) //если же он стал меньше нуля или равен ему
                {
                    TurretHP thp = Target.GetComponent<TurretHP>(); //подключаемся к компоненту ХП цели
                    if (thp != null) thp.ChangeHP(-damage); //если цель ещё живая, наносим дамаг (мы можем не одни бить по цели, потому проверка необходима)
                    attackTimer = coolDown; //возвращаем таймер в исходное положение
                }
            }
        }
    }
    //Очень примитивный метод сортировки целей, море возможностей для модификации!
    private GameObject SortTargets()
    {
        float closestTurretDistance = 0; //инициализация переменной для проверки дистанции до пушки
        GameObject nearestTurret = null; //инициализация переменной ближайшей пушки
        List<GameObject> sortingTurrets = gv.TurretList; //оздаём массив для сортировки

        foreach (var turret in sortingTurrets) //для каждой пушки в массиве
        {
            //если дистанция до пушки меньше, чем closestTurretDistance или равна нулю
            if ((Vector3.Distance(mob.position, turret.transform.position) < closestTurretDistance) || closestTurretDistance == 0)
            {
                closestTurretDistance = Vector3.Distance(mob.position, turret.transform.position); //Меряем дистанцию от моба до пушки, записываем её в переменную
                nearestTurret = turret;//устанавливаем её как ближайшего
            }
        }
        return nearestTurret; //возвращаем ближайший ствол
    }
}
