// CharacterManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    
    public static int selectedCharacterIndex = 0;

    public GameObject[] characterPrefabs;

    public string playerTag = "Player";

    
    public void SelectCharacter(int index)
    {
        selectedCharacterIndex = index;
       
    }

    private void Start()
    {
        
        if (SceneManager.GetActiveScene().name == "PlayerMovement")
        {
            ReplaceExistingPlayerInThisScene();
        }
    }

    private void ReplaceExistingPlayerInThisScene()
    {
        if (characterPrefabs == null || characterPrefabs.Length == 0) return;

        GameObject existing = GameObject.FindWithTag(playerTag);
        if (existing == null)
        {
        
            Character existingChar = FindObjectOfType<Character>();
            if (existingChar != null) existing = existingChar.gameObject;
        }

        
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        Transform parent = null;

        if (existing != null)
        {
            pos = existing.transform.position;
            rot = existing.transform.rotation;
            parent = existing.transform.parent;
        }

        
        if (existing != null) Destroy(existing);

        
        int i = Mathf.Clamp(selectedCharacterIndex, 0, characterPrefabs.Length - 1);
        GameObject newPlayer = Instantiate(characterPrefabs[i], pos, rot, parent);

    
        if (!string.IsNullOrEmpty(playerTag)) newPlayer.tag = playerTag;

        var cam = Camera.main;
        if (cam != null)
        {
            var camIntro = cam.GetComponent<CameraIntro>();
            if (camIntro != null)
            {
                camIntro.targetPosition = newPlayer.transform;
            }
        }
    }
}
