using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Responsavel por controlar o trafego de informações sobre os jogadores
    List<PlayerInfo> allPlayers = new List<PlayerInfo>(); //Lista com todos os jogadores no servidor
    public PlayerInfo LocalPlayer { get; private set; }

    [SerializeField]
    GameObject[] charListPrefabs; //Lista com os prefabs dos chars

    [SerializeField]
    Transform startPoint;

    Camera mainCamera;

    public static PlayerManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start() {
        mainCamera = Camera.main;
    }

    //Tratamento quando um jogador se conecta ao servidor
    public void NewPlayer(Dictionary<string, string> infos)
    {
        //Instancia o char do player
        //Seta nome

        string charIndexStr = infos["char"];
        int charIndex = int.Parse(charIndexStr);

        GameObject charInstance = Instantiate(charListPrefabs[charIndex],
         startPoint.position, startPoint.rotation);

        PlayerInfo playerInfo = charInstance.GetComponent<PlayerInfo>();

        string playerName = infos["name"];
        string playerId = infos["id"];

        playerInfo.SetPlayerInfo(playerName, charIndexStr, playerId);
        allPlayers.Add(playerInfo);

    }

    //Use para localizar um player especifico
    PlayerInfo SearchPlayer(string playerId)
    {

        PlayerInfo result = null;

        foreach (PlayerInfo playerInfo in allPlayers)
        {
            if (playerInfo.PlayerId == playerId)
            {

                result = playerInfo;
                return result;
            }
        }

        return result;
    }

    public PlayerController GetPlayerController()
    {
        PlayerController localPlayerController;

        localPlayerController = LocalPlayer.gameObject.GetComponent<PlayerController>();

        return localPlayerController;
    }

    //Tratamento quando um jogador se movimenta
    public void SomePlayerMove(Dictionary<string, string> infos)
    {
        //Vector3 posi = jsonto
        string id = infos["id"];

        PlayerInfo player = SearchPlayer(id);

        if (player != null)
        {
            Vector3 posi = ConverJsonVector.JsonToVector3(infos["postion"]);
            Vector4 rot = ConverJsonVector.JsonToVector4(infos["rotation"]);

            Quaternion quaternion = new Quaternion(rot.x, rot.y, rot.z, rot.w);

            player.UpdatePlayerPosic_Rot(posi, quaternion);
        }
    }

    //Tratamento quando um jogador desconecta do servidor
    public void PlayerLeftGame(Dictionary<string, string> infos)
    {
        //Destroi a instancia do player
        //Remove player da lista de players

        string playerid = infos["id"];
        foreach (PlayerInfo player in allPlayers)
        {
            if (player.PlayerId == playerid)
            {
                Destroy(player.gameObject);
                allPlayers.Remove(player);
            }
        }
    }

    //Jogador local conectou-se ao servidor
    public void LocalPlayerJoinInServe(Dictionary<string, string> infos)
    {
        //Instancia o char do player
        //Seta nome
        string charIndexStr = infos["char"];
        int charIndex = int.Parse(charIndexStr);

        GameObject charInstance = Instantiate(charListPrefabs[charIndex],
         startPoint.position, startPoint.rotation);

        LocalPlayer = charInstance.GetComponent<PlayerInfo>();

        string playerName = infos["name"];
        string playerId = infos["id"];

        LocalPlayer.SetPlayerInfo(playerName, charIndexStr, playerId);

        mainCamera.gameObject.SetActive(false);
        GetPlayerController().MyCamera();
        GetPlayerController().IslocalPlayer = true;
    }

    public void LocalPlayerSpeaks(Dictionary<string, string> infos)
    {

        string msg = infos["message"];

        //Digita a mensagem no balão de msg do player 
        PlayerController playerController = LocalPlayer.gameObject.GetComponent<PlayerController>();
        playerController.Speach(msg);
        
        //Digita a mensagem no chat
        string playerName = LocalPlayer.PlayerName;
        ChatSystem.instance.MsgReceived(msg, playerName);
    }

    //Mensagem de chat recebida do servidor
    public void SomePlayerSpeaks(Dictionary<string, string> infos)
    {
        string playerId = infos["id"];

        PlayerInfo player = SearchPlayer(playerId);
        if (player != null)
        {

            //Digita mensagem no balão de texto
            PlayerController playerController = player.gameObject.GetComponent<PlayerController>();
            string msg = infos["message"];
            playerController.Speach(msg);

            //Digita mensagem no chat
            string playerName = player.PlayerName;
            ChatSystem.instance.MsgReceived(msg, playerName);
        }

    }

}
