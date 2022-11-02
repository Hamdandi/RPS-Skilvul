using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Button startGameButton;
    [SerializeField] TMP_InputField newRoomInputField;
    [SerializeField] TMP_Text feedBackText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject roomListObject;
    [SerializeField] GameObject playerListObject;
    [SerializeField] RoomItem roomItemPrefab;
    [SerializeField] PlayerItem playerItemPrefab;

    List<RoomItem> roomItemList = new List<RoomItem>();
    List<PlayerItem> playerItemList = new List<PlayerItem>();

    private void Start()
    {
        PhotonNetwork.JoinLobby();
        roomPanel.SetActive(false);
    }

    public void ClickCreateRoom()
    {
        feedBackText.text = "";
        if (newRoomInputField.text.Length < 3)
        {
            feedBackText.text = "Room name min 3 character";
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(newRoomInputField.text, roomOptions);
    }

    internal void JoinedRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Create Room : " + PhotonNetwork.CurrentRoom.Name);
        feedBackText.text = "Create Room : " + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Join Room : " + PhotonNetwork.CurrentRoom.Name);
        feedBackText.text = "Join Room : " + PhotonNetwork.CurrentRoom.Name;
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        roomPanel.SetActive(true);

        // Update Player List
        UpdatePlayerList();

        // Atur button start game
        SetStartGameButton();

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Update Player List
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Update Player List
        UpdatePlayerList();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // Atur button start game
        SetStartGameButton();
    }

    private void SetStartGameButton()
    {
        startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);

        startGameButton.interactable = PhotonNetwork.CurrentRoom.PlayerCount > 1;
    }

    private void UpdatePlayerList()
    {
        // Destroy terlebih dahulu player item yang sudah ada
        foreach (PlayerItem item in playerItemList)
        {
            Destroy(item.gameObject);
        }
        playerItemList.Clear();

        // bikin ulang player list

        foreach (var (id, player) in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newplayerItem = Instantiate(playerItemPrefab, playerListObject.transform);
            newplayerItem.Set(player);
            playerItemList.Add(newplayerItem);

            if (player == PhotonNetwork.LocalPlayer)
                newplayerItem.transform.SetAsFirstSibling();

        }

        // start game hanya bisa di klik jika player sudah mencapai jumlah tertentu
        SetStartGameButton();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + "," + message);
        feedBackText.text = returnCode.ToString() + "," + message;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var item in roomItemList)
        {
            Destroy(item.gameObject);
        }

        this.roomItemList.Clear();

        foreach (var roomInfo in roomList)
        {
            var newRoomItem = Instantiate(roomItemPrefab, roomListObject.transform);
            newRoomItem.Set(this, roomInfo.Name);
            this.roomItemList.Add(newRoomItem);
        }
    }
}
