using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo: MonoBehaviour
{
    //Responsável por salvar as informações dos jogadores
    
    public string PlayerName { get; private set; }
    public string CharId { get; private set; }  
    public string PlayerId { get; private set; }

    public Text playerNameTxt; 

    public void SetPlayerInfo(string playername, string charId, string playerid){

        PlayerName = playername;
        CharId = charId;
        PlayerId = playerid;

        playerNameTxt.text = PlayerName;
    }

    //Use para atualizar a posição/rotação do jogador no server
    public void UpdatePlayerPosic_Rot(Vector3 posic, Quaternion rot){
        transform.position = posic;
        transform.rotation = rot;
    }
}
