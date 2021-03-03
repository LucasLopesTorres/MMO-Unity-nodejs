using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class ConnectionManager : MonoBehaviour
{
    //Responsável por gerenciar a conexão com o servidor

    SocketIOComponent socket;

    PlayerManager playerManager;

    [SerializeField] SelectChar select;

    public GameObject menuPanel;

    public static ConnectionManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            socket = GetComponent<SocketIOComponent>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerManager = PlayerManager.instance;

        //Socket.On
        socket.On("Join_Success", OnReceivedJoinSuccess);
        socket.On("Spawn_Player", OnReceivedSpawnPlayer);
        socket.On("User_Disconnected", OnReceivedUserDisconnect);
        socket.On("PlayerMove", OnReceivedPlayerMove);
        socket.On("MessageSended", OnReceiveMessageSended);
        socket.On("MessageReceived", OnReceiveMessageReceived);

    }

    //Conexão com o servidor
    public void JoinServe()
    {
        Debug.Log("JoinServe");

        Dictionary<string, string> myInfos = new Dictionary<string, string>();
        string myName = select.PlayerName;
        string myChar = select.CharId.ToString();

        //Obs: nomes dos campos precisam coincidir com o script do node
        myInfos["name"] = myName;
        myInfos["char"] = myChar;

        //Emite uma msg pro server
        //Obs: Nome da msg precisa coincidir com o script do node
        socket.Emit("Join", new JSONObject(myInfos));
    }

    //Jogador conseguiu se conectar com o servidor
    void OnReceivedJoinSuccess(SocketIOEvent infos)
    {
        //Avisa ao player manager

        Debug.Log("OnReceivedJoinSuccess");
        Dictionary<string, string> infosDic = infos.data.ToDictionary();
        playerManager.LocalPlayerJoinInServe(infosDic);

        menuPanel.SetActive(false);
    }

    void OnReceivedSpawnPlayer(SocketIOEvent infos)
    {

        Debug.Log("OnReceivedSpawnPlayer");
        Dictionary<string, string> infosDic = infos.data.ToDictionary();
        playerManager.NewPlayer(infosDic);

    }

    void OnReceivedUserDisconnect(SocketIOEvent infos)
    {

        Debug.Log("OnReceivedUserDisconnect");
        Dictionary<string, string> infosDic = infos.data.ToDictionary();
        playerManager.PlayerLeftGame(infosDic);
    }

    void OnReceivedPlayerMove(SocketIOEvent infos)
    {

        Debug.Log("OnReceivedPlayerMove");
        Dictionary<string, string> infosDic = infos.data.ToDictionary();
        playerManager.SomePlayerMove(infosDic);
    }

    //Mensagem enviada pelo jogador chegou ao servidor
    void OnReceiveMessageSended(SocketIOEvent infos)
    {

        Debug.Log("OnReceiveMessageSended");
        Dictionary<string, string> infosDic = infos.data.ToDictionary();
        playerManager.LocalPlayerSpeaks(infosDic);
    }

    //Mensagem chegou ao chat
    void OnReceiveMessageReceived(SocketIOEvent infos)
    {

        Debug.Log("OnReceiveMessageReceived");
        Dictionary<string, string> infosDic = infos.data.ToDictionary();
        playerManager.SomePlayerSpeaks(infosDic);     
    }


    //Avisa ao servidor que houve movimento
    public void EmitPosic_Rot(Vector3 posic, Quaternion rot)
    {
        Dictionary<string, string> info = new Dictionary<string, string>();

        info["position"] = posic.x + "," + posic.y + "," + posic.z;
        info["rotation"] = rot.x + "," + rot.y + "," + rot.z + "," + rot.w;

        socket.Emit("Move", new JSONObject(info));
    }

    //avisa ao servidor a msg enviado pelo local player
    public void EmitMsg(string msg)
    {
        Dictionary<string, string> info = new Dictionary<string, string>();

        info["message"] = msg;

        socket.Emit("Chat_Message", new JSONObject(info));
    }
}
