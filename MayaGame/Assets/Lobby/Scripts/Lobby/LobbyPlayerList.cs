using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Prototype.NetworkLobby
{
    //List of players in the lobby
    public class LobbyPlayerList : MonoBehaviour
    {
        public static LobbyPlayerList _instance = null;

        public RectTransform playerListContentTransform;
        public GameObject warningDirectPlayServer;
        public Transform addButtonRow;

        public Text ParameterText;

        protected VerticalLayoutGroup _layout;
        public List<LobbyPlayer> _players = new List<LobbyPlayer>();

        public void OnEnable()
        {
            _instance = this;
            _layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();
        }

        public void DisplayDirectServerWarning(bool enabled)
        {
            if(warningDirectPlayServer != null)
                warningDirectPlayServer.SetActive(enabled);
        }

        void Update()
        {
            //this dirty the layout to force it to recompute evryframe (a sync problem between client/server
            //sometime to child being assigned before layout was enabled/init, leading to broken layouting)
            
            if(_layout)
                _layout.childAlignment = Time.frameCount%2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
        }

        public void AddPlayer(LobbyPlayer player)
        {
            if (_players.Contains(player))
                return;

            _players.Add(player);

            player.transform.SetParent(playerListContentTransform, false);
            addButtonRow.transform.SetAsLastSibling();

            PlayerListModified();

            if (player.isServer)
            {
                transform.root.GetComponent<LobbyManager>().hostPlayer = player;
            }

        }

        public void RemovePlayer(LobbyPlayer player)
        {
            _players.Remove(player);
            PlayerListModified();
        }

        public void PlayerListModified()
        {
            int i = 0;
            foreach (LobbyPlayer p in _players)
            {
                if (p.isServer)
                {
                    p.SendMission(p.missionSlot);
                }
                p.OnPlayerListChanged(i);
                ++i;
            }
        }

        public void SetPrimaly(string name)
        {
            
            foreach (LobbyPlayer p in _players)
            {
                if (p.isLocalPlayer)
                {
                    p.CmdSetPrim(name);
                }
            }
        }

        public void SetSecondaly(string name)
        {

            foreach (LobbyPlayer p in _players)
            {
                if (p.isLocalPlayer)
                {
                    p.CmdSetSecond(name);
                }
            }
        }

        public void SetAttach(string name,int slot)
        {

            foreach (LobbyPlayer p in _players)
            {
                if (p.isLocalPlayer)
                {
                    p.CmdSetAtach(name,slot);
                }
            }
        }

        public void SetItem(string name)
        {

            foreach (LobbyPlayer p in _players)
            {
                if (p.isLocalPlayer)
                {
                    p.CmdSetItem(name);
                }
            }
        }


    }
}
