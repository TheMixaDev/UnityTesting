using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameField;
    public void SwitchScene()
    {
        PersistenceManager.Instance.Name = nameField.text;
        SceneManager.LoadScene(1);
    }
}
