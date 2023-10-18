using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject Camera;
    public GameObject Listener;

    void Update()
    {
        // простой способ расположения AudioListener в пространстве между камерой и персонажем
        Debug.DrawLine(Player.transform.position, Camera.transform.position);
        Listener.transform.position = ((Camera.transform.position - Player.transform.position) / 2) +Player.transform.position;
        Listener.transform.rotation = Camera.transform.rotation;
        // более сложный вариант - проверять дальность расположения камеры и от этого варьировать пвсполодение AudioListener для большей реалистичности
    }
}
