using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController; //строка неймспейса для работы с контроллером

public class script_Footsteps : MonoBehaviour
{
    public AudioSource FootstepSource; // переменная для обращения к AudioSource
    public AudioClip[] RunClip; // переменная для аудио-клипа run
    public AudioClip[] SprintClip; // sprint audio clips
    public AudioClip[] WalkClip;
    public AudioClip[] JumpClip;
    vThirdPersonInput tpInput; // переменная для обращения к контроллеру для проверки магнитуды
    vThirdPersonController tpController; // переменная для проверки стейта 
    public float MinWalkVol, MaxWalkVol, MinRunVol, MaxRunVol, MinSprintVol, MaxSprintVol, MinJumpVol, MaxJumpVol;
    int NewIndex;
    int LastIndex;


    void Start()
    {
        Debug.Log("Hello there");
        FootstepSource = gameObject.GetComponent<AudioSource>(); // обращение к AudioSource
        tpInput = GetComponent<vThirdPersonInput>(); // к компоненту инпута
        tpController = GetComponent<vThirdPersonController>(); // к контроллеру
    }

    void Footstep() // обращение к созданной функции в Animator для проигрывания шагов
    {
        if (tpInput.cc.inputMagnitude > 0.1) // если значение магнитуды > 0.1, значит мы двигаемся
        {
         

            if (tpController.isSprinting) // проаерка на спринт
            {
                Randomization(SprintClip.Length); // запуск рандомизатора индекса
                FootstepSource.volume = Random.Range(MinSprintVol, MaxSprintVol);
                FootstepSource.pitch = Random.Range(0.9f, 1f);
                FootstepSource.PlayOneShot(SprintClip[NewIndex]);
                LastIndex = NewIndex; // записываем номер индекса, чтобы не проиграть его снова
            }
            else // в противном случае
            {
                if (tpInput.cc.inputMagnitude < 0.5) // то он идёт
                {
                    Randomization(WalkClip.Length);
                    FootstepSource.volume = Random.Range(MinWalkVol, MaxWalkVol);
                    FootstepSource.pitch = Random.Range(0.8f, 1f);
                    FootstepSource.PlayOneShot(WalkClip[NewIndex]);
                    LastIndex = NewIndex;
                }
                else // в противном случае = он бежит
                {
                    Randomization(RunClip.Length);
                    FootstepSource.volume = Random.Range(MinRunVol, MaxRunVol);
                    FootstepSource.pitch = Random.Range(0.9f, 1.1f);
                    FootstepSource.PlayOneShot(RunClip[NewIndex]);
                    LastIndex = NewIndex;
                }

            
        }
    }
        

    }

    void Randomization (int ClipLength) // функция для рандомизации
    {
        NewIndex = Random.Range(0, ClipLength);
        while (NewIndex == LastIndex) // колесо крутится пока NewIndex == LastIndex
            NewIndex = Random.Range(0, ClipLength);

    }
}
