using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Invector.vCharacterController;

public class DemoFootstepsScrit_collider : MonoBehaviour
{
    [EventRef] // даст переменной доступ к ивентам в проекте FMod
    public string walkingEvent;

    float surface;
    vThirdPersonController tpController; // переменная для проверки стейта
    EventInstance walkingInstance; // для создания инстансов

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("vSnow"))
        {
            surface = 0f;
        }
        else if (other.CompareTag("vWood"))
        {
            surface = 1f;
        }
        else surface = 0f;

        walking();
    }

    void walking()
    {
        Debug.Log("walking");
        walkingInstance = RuntimeManager.CreateInstance(walkingEvent); // создание инстансов шагов для передачи параметров (в тч о поверхностях)
        walkingInstance.setParameterByName("footsteps_type", surface); // передача параметра поверхности
        RuntimeManager.AttachInstanceToGameObject(walkingInstance, gameObject.GetComponent<Transform>(), gameObject.GetComponent<Rigidbody>());
        walkingInstance.start();
        walkingInstance.release(); // уничтожение инстанса после отыгрывания
    }

}
