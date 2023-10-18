using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fmod_windScript : MonoBehaviour
{
    [FMODUnity.EventRef] // обращение к ивентам
    public string windEvent; // сюда в инспекторе нужно будет поместить ивент
    FMOD.Studio.EventInstance windInstance; // создание и сохранение инстанса
    
    void Start()
    {
        // FMODUnity.RuntimeManager.PlayOneShot(windEvent); // простой способ (создание одной копии event), нельзя передавать параметр
        windInstance = FMODUnity.RuntimeManager.CreateInstance(windEvent);
        windInstance.start(); // запуск (аргумент)


    }


    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            windInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); // остакновка с фейд-аутом
        }
    }
}
