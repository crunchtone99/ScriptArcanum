using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController.AI;

public class spawn_manager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Создаем массив для префабов врагов
    public float rangeX = 20; // координаты для появления. Можно вводить ниже просто цифрами без переменных
    public float positionZ = 20; // позиция по оси Z
    public int numberOfenemies = 0; // в этой переменной будет храниться количество всех врагов (добавляется +1, когда враги появляются, а -1, когда враги умирают)
    public GameObject WayPoint;
    public float stateparam;

    void Start()
    {
        InvokeRepeating("SpawnEnemies", 5, 10f); // Метод, который запускает функцию появления каждые 10 секунд. Перед первым появлением 5 секунд
    }

    void SpawnEnemies()
    {
        int enemyIndex = Random.Range(0, enemyPrefabs.Length); // Выбирает случайный индекс префаба, которые мы загрузили в наш скрипт
        Vector3 spawnPosition = new Vector3(Random.Range(-rangeX, rangeX), 0, positionZ); // Случайная позиция для появления врага
        GameObject SpawnEnemyObject =  Instantiate(enemyPrefabs[enemyIndex], spawnPosition, enemyPrefabs[enemyIndex].transform.rotation); // спауним префаб и подключаемся к нему в переменной SpawnEnemyObject типа GameObject (чтобы не искать через find objects)
        var EnemyController = SpawnEnemyObject.GetComponent<vControlAIMelee>(); // подключаемся к контроллеру AI
        EnemyController.onDead.AddListener(OnEnemyDead); // здесь мы следим за функцией onDead - оставляем коллбек AddListener и когда она производится мы запускаем метод OnEnemyDead
        enemyList.Add(SpawnEnemyObject); // Добавляем в список всех объектов наш новый объект префаба, чтобы за ним следить в списке
        numberOfenemies++; // Добавили в переменную, что появился новый враг
       
    }

    List<GameObject> enemyList = new List<GameObject>(); // создаем список для хнранени
    bool isAnyInCombat; // переменнтая если враг в бою

    private void Update()
    {
        bool isAnyInCombatNewState = false; // создаем промежуточную переменную для нового состояния врага
        // foreach (var enemymob in enemyList)
        for (int i = 0; i < enemyList.Count; i++) 
        {
            var enemymob = enemyList[i];
            var EnemyController =  enemymob.GetComponent<vControlAIMelee>();
            
            isAnyInCombatNewState = EnemyController.isInCombat;
            if (isAnyInCombatNewState) break;
        }

        if (isAnyInCombatNewState != isAnyInCombat)
        {
            isAnyInCombat = isAnyInCombatNewState;
            
            if (isAnyInCombat)
            {
                stateparam = 1f;
            }
            else stateparam = 0f;

            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("player_in_battle", stateparam);
        }
    }

    void OnEnemyDead(GameObject DeadEnemy)
    {
        enemyList.Remove(DeadEnemy);
        numberOfenemies--;
    }

    private void OnGUI()
    {
        GUILayout.Space(100);
        GUILayout.Label("Game State: " +  (isAnyInCombat ? "Combat" : "Exploration" ));
        GUILayout.Label("Number of Enemies: " + numberOfenemies);
    }
}
