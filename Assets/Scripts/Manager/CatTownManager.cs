using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatTownManager : MonoBehaviour
{
    [Tooltip("Miya, Bambi, Leo, Cosmo, Chrono, Misty")]
    [SerializeField] private GameObject[] NPCPrefabs;
    [SerializeField] private Vector3[] NPCPositions;
    [SerializeField] private Vector3 StartPosition;

    private void Start()
    {
        int selectedPlayerIndex = (int)GPGS_AccountDataManager.Instance.SelectedCharacter;

        for (int i = 0; i < NPCPrefabs.Length; i++)
        {
            if (i == selectedPlayerIndex)
            {
                GameObject Player = Instantiate(NPCPrefabs[i], StartPosition, Quaternion.identity);
                Player.GetComponent<PlayerNPC>().NPCPosition = NPCPositions[i];
                Player.GetComponent<PlayerNPC>().enabled = false;
                GameManager.Instance.ChangePlayer(Player);
            }
            else
            {
                GameObject NPC = Instantiate(NPCPrefabs[i], NPCPositions[i], Quaternion.identity);
                NPC.GetComponent<PlayerNPC>().NPCPosition = NPCPositions[i];
            }
        }
    }
}
