using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    [SerializeField]
    private int currentLevelIndex;
    [SerializeField]
    private int playerScore;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(JsonUtility.ToJson(this));
        //JsonUtility.FromJson<SaveData>("");
    }

}
