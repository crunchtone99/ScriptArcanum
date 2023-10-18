using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;


public class PlayerFmodFootsteps : MonoBehaviour
{
    [FMODUnity.EventRef] 
    public string walkingEvent; // путь к ивенту шагов

    [FMODUnity.EventRef]
    public string runningEvent; // путь к ивенту шагов в беге

    [FMODUnity.EventRef]
    public string breathEvent;

    [FMODUnity.EventRef]
    public string landEvent;

    vThirdPersonInput tpInput; // переменная для обращения к магнитуде
    vThirdPersonController tpController; // переменная для отслеживания перемещения контроллера


    FMOD.Studio.EventInstance walkingInstance; // переменная инстанса для шагов
    FMOD.Studio.EventInstance breathInstance; // переменная инстанса для дыхания
    FMOD.Studio.EventInstance landInstance; // переменная инстанса для презимления

    public LayerMask lm; // переменная для выбора слоя, с которого будет считываться тэг поверхности
    float surface; // переменная для хранения числового значения параметра поверхности для передачи далее в FMOD

    public GameObject mouth; // переменная геймобъекта, из которого будет звучать дыхание
    public GameObject legs; // переменная геймобъекта, из которого будут звучать шаги


    void Start()
    {
        tpInput = GetComponent<vThirdPersonInput>(); // создаем ссылку на компонент инпута, для проверки магнитуды
        tpController = GetComponent<vThirdPersonController>(); // создаем ссылку на компонент контроллера, для проверки типа движения
        breathInstance = FMODUnity.RuntimeManager.CreateInstance(breathEvent); // создаем инстанс дыхания
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(breathInstance, mouth.transform, gameObject.GetComponent<Rigidbody>()); // привязываем инстанс дыхания, чтобы звук двигался за геймобъектом
        breathInstance.start(); // запускаем ивент дыхания


    }


    void footstep()
    {
        if (tpInput.cc.inputMagnitude > 0.1) // фикс, чтобы шаги не продолжались после остановки движения
        {
            SurfaceCheck(); // запускаем метод, в котором будет происходить проверка поверхности на которую мы наступили

            if (tpController.isJumping == false) // проверяем, не находимся ли мы в прыжке
            {

                if (tpController.isSprinting) // проверяем, бежим ли мы
                {
                    walkingInstance = FMODUnity.RuntimeManager.CreateInstance(runningEvent); // передаем в инстанс шагов ивент бега
                    FMODUnity.RuntimeManager.AttachInstanceToGameObject(walkingInstance, legs.transform, gameObject.GetComponent<Rigidbody>()); // прекрепляем инстанс к геймобъекту ног
                    breathInstance.setParameterByName("locomotion_type", 2f); // передаем параметр типа движения БЕГ в инстанс дыхания. Кстати, дыхание у нас звучит на лупе и не выключается
                    walkingInstance.setParameterByName("surface_type", surface); // передаем в инстанс шагов параметр тип поверхности и в него передаем переменную из метода поверхности поверхности
                    walkingInstance.start();
                    walkingInstance.release();
                }

                else // если мы не бежим, мы точно идем. Правда же?
                {
                    walkingInstance = FMODUnity.RuntimeManager.CreateInstance(walkingEvent); // передаем в инстанс шагов ивент шагов
                    FMODUnity.RuntimeManager.AttachInstanceToGameObject(walkingInstance, legs.transform, gameObject.GetComponent<Rigidbody>()); // прекрепляем инстанс к геймобъекту ног
                    breathInstance.setParameterByName("locomotion_type", 1f); // передаем параметр типа движения ШАГ в инстанс дыхания
                    walkingInstance.setParameterByName("surface_type", surface); // передаем в инстанс шагов параметр тип поверхности и в него передаем переменную из метода поверхности поверхности
                    walkingInstance.start();
                    walkingInstance.release();
                }
            }

        }

    }
    void jump() // функция прыжка на месте
    {
        if (tpController.isJumping) // проверяем прыгаем ли мы
        {

            breathInstance.setParameterByName("locomotion_type", 3f); // передаем параметр типа движения Прыжок в инстанс дыхания
        }
    }


    void jump_move() // функция прыжка в движении
    {
        if (tpController.isJumping)
        {
            breathInstance.setParameterByName("locomotion_type", 4f);
        }
    }

    void land_low() // ивент приземления с не высокой высоты
    {
        landInstance = FMODUnity.RuntimeManager.CreateInstance(landEvent); 
        landInstance.setParameterByName("locomotion_type", 5f);
        landInstance.setParameterByName("surface_type", surface);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(landInstance, legs.transform, gameObject.GetComponent<Rigidbody>());
        landInstance.start();
        landInstance.release();
        breathInstance.setParameterByName("locomotion_type", 5f);


    }

    void land_high() // ивент приземления с высоты
    {
        landInstance = FMODUnity.RuntimeManager.CreateInstance(landEvent);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(landInstance, legs.transform, gameObject.GetComponent<Rigidbody>());
        landInstance.setParameterByName("locomotion_type", 6f);
        landInstance.setParameterByName("surface_type", surface);
        landInstance.start();
        landInstance.release();
        breathInstance.setParameterByName("locomotion_type", 6f);
    }

    void SurfaceCheck() // метод проверки поверхности
    {
      
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out RaycastHit hit, 1f, lm)) // создаем луч от нашего геймобъекта (Контроллера), вниз на величину 1, и считываем только слой из параметра lm
        {
            Debug.Log(hit.collider.tag); // поставил дебаг, чтобы в консоль показывалось куда мы наступили
    
            switch (hit.collider.tag) // создаем свитч, в который приходит параметр поверхности. Важно назначить поверхности именно с такими именами TAG, как написано ниже
            {
                case "vMild_snow": // если в тэг коллайдера vMild_snow
                    surface = 0f; // выставляется параметру surface 0. Далее он будет передаваться в параметр FMOD
                    break;
                case "vWood":
                    surface = 1f;
                    break;
                case "vWater":
                    surface = 2f;
                    break;
                case "vCrunchy":
                    surface = 3f;
                    break;
                case "vRock":
                    surface = 4f;
                    break;
                case "vNear_water":
                    surface = 5f;
                    break;
                case "vIce_stead":
                    surface = 6f;
                    break;
                case "vIce":
                    surface = 7f;
                    break;
                case "vSnow_wood":
                    surface = 8f;
                    break;
                default:
                    surface = 0f;
                    break;
            }

        }

    }

}



