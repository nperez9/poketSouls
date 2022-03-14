using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Battle { 
    public class PartyScreen : MonoBehaviour
    {
        [SerializeField] Text ScreenText;

        PartyMember[] membersSlots;

        public void Init()
        {
            membersSlots = GetComponentsInChildren<PartyMember>();
        }

        public void SetPartyMembers(List<Pokemon> teamMembers)
        {
            if (teamMembers.Count < 1)
            {
                return;
            }

            for (int i = 0; i < membersSlots.Length; i++)
            {
                if (i < teamMembers.Count)
                {
                    membersSlots[i].SetData(teamMembers[i]);
                } 
                else
                {
                    membersSlots[i].DisablePartySlot();
                }
            }            
        }

        public void SetCursor(int cursor, List<Pokemon> TeamMembers)
        {
            for (int i = 0; i < TeamMembers.Count; i++)
            {
                if (cursor == i)
                {
                    membersSlots[i].AddCursor();
                }
                else
                {
                    membersSlots[i].RemoveCursor();
                }
            }
        }

        public void SetMessage(string message)
        {
            ScreenText.text = message;
        }
    }
}