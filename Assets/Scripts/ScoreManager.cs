using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ScoreManager : MonoBehaviour
{
    public PlayerInfo currentPlayerInfo;

    public PlayerInfo CreateInfo(Player player)
    {
        PlayerInfo current = Instantiate(currentPlayerInfo, transform) ;
        current.transform.SetParent(transform);
        current.Name.text = player.NickName;
        Debug.Log("should add player to scoreboard");
        return current;
    }
}
