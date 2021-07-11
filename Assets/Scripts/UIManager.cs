using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public int numCherries = 0;
    [SerializeField] private Text textCherries;

    public void AddCherrie() {
        numCherries++;
        textCherries.text = numCherries.ToString();
    }
}
