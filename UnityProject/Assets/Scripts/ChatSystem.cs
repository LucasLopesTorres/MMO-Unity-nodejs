using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatSystem : MonoBehaviour
{

    bool typing = false; //Envia ou abre chat?
    [SerializeField] InputField speachPlace; // onde digita a msg
    [SerializeField] Text conversation; //Texto que mostra todas as mensagens

    public static ChatSystem instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) &&
        PlayerManager.instance.LocalPlayer.GetComponent<PlayerController>().IslocalPlayer)
        {
            Debug.Log("Enter");
            if (typing)
            {
                //Envia msg ao servidor
                SendMsg();
                typing = false;
            }
            else
            {
                //abre chat
                speachPlace.gameObject.SetActive(true);
                speachPlace.Select();
                typing = true;
            }
        }
    }

    //Enviar msg servidor
    void SendMsg()
    {
        //Verificar se é o player local
        //Envia msg para o servidor
        speachPlace.gameObject.SetActive(false);

        if (PlayerManager.instance.GetPlayerController().IslocalPlayer)
        {
            string msg = speachPlace.text;
            //Avisa ao connectionManager
            ConnectionManager.instance.EmitMsg(msg);
        }
    }

    //Mensagem recebida
    public void MsgReceived(string newMsg, string playerName)
    {
        //Seta mensagem na janela de chat
        string msg = playerName + ": " + newMsg + "\n";
        conversation.text += msg;
    }

}
