using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerAI : MonoBehaviour
{
    public int waveAmount = 5; //Количество мобов за 1 волну на каждой точке спауна
    public int waveNumber = 0; //переменная текущей волны
    public float waveDelayTimer = 30.0F; //переменная таймера спауна волны
    public float waveCooldown = 20.0F; //переменная (не константа уже!) для сброса таймера выше, мы её будем модифицировать
    public int maximumWaves = 500; //максимальное количество мобов в игре
    public Transform Mob; //переменная для загрузки префаба в Unity
    public GameObject[] SpawnPoints; //массив точек спауна
    private GlobalVars gv; //поле для объекта глобальных переменных

    private void Awake()
    {
        SpawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint"); //забираем все точки спауна в массив
        gv = GameObject.Find("GlobalVars").GetComponent<GlobalVars>(); //инициализируем поле
    }

    private void Update()
    {
        if (waveDelayTimer > 0) //если таймеh спауна волны больше нуля
        {
            if (gv != null)
            {
                if (gv.MobCount == 0) waveDelayTimer = 0; //если мобов на сцене нет - устанавливаем его в ноль
                else waveDelayTimer -= Time.deltaTime; //иначе отнимаем таймер
            }
        }
        if (waveDelayTimer <= 0) //если таймер менее или равен нулю
        {
            if (SpawnPoints != null && waveNumber < maximumWaves) //если имеются точки спауна и ещё не достигнут предел количества волн
            {
                foreach (GameObject spawnPoint in SpawnPoints) //на каждой точке спауна
                {
                    for (int i = 0; i < waveAmount; i++) //используем i как модификатор для спауна, чтобы мобы не были в упор друг к другу
                    {
                        Instantiate(Mob, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z + i * 10), Quaternion.identity); //спауним моба
                    }

                    if (waveCooldown > 5.0f) //если задержка длится более 5 секунд
                    {
                        waveCooldown -= 0.1f; //сокращаем на 0.1 секунды
                        waveDelayTimer = waveCooldown; //задаём новый таймер
                    }
                    else //иначе
                    {
                        waveCooldown = 5.0f; //задержка никогда не будет менее 5 секунд
                        waveDelayTimer = waveCooldown;
                    }

                    if (waveNumber >= 50) //после 50 волны
                    {
                        waveAmount = 10; //будем спаунить по 10 мобов на каждой точке
                    }
                }
                waveNumber++; //увеличиваем номер волны
            }
        }
    }
}
