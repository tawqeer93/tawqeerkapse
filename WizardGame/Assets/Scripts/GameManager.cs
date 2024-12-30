using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private List<HiddenObject> hiddenObjectsList;
    private List<HiddenObject> activeHiddenObjectsList;

    [SerializeField]
    private int maxActiveHiddenObjectsCount = 5;

    private int totalHiddenObjectsFound = 0;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != null)
            Destroy(gameObject);
    }

    private void Start()
    {
        activeHiddenObjectsList = new List<HiddenObject>();
        AssignHiddenObjects();
    }

    void AssignHiddenObjects()
    {
        totalHiddenObjectsFound = 0;
        activeHiddenObjectsList.Clear();
        for (int i = 0; i < hiddenObjectsList.Count; i++)
        {
            hiddenObjectsList[i].hiddenObject.GetComponent<Collider2D>().enabled = false;
        }

        int k = 0;
        while (k < maxActiveHiddenObjectsCount)
        {
            int randomVal = Random.Range(0, hiddenObjectsList.Count);

            if (hiddenObjectsList[randomVal].makeHidden)
            {
                hiddenObjectsList[randomVal].hiddenObject.name = "" + k;
                hiddenObjectsList[randomVal].makeHidden = true;
                hiddenObjectsList[randomVal].hiddenObject.GetComponent<Collider2D>().enabled = true;

                activeHiddenObjectsList.Add(hiddenObjectsList[randomVal]);
                k++;
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(position, Vector3.zero);

            if (hit && hit.collider != null)
            {
                //Debug.Log("Object Name:" + hit.collider.gameObject.name);

                hit.collider.gameObject.SetActive(false);

                for (int i = 0; i < hiddenObjectsList.Count; i++)
                {
                    if (activeHiddenObjectsList[i].hiddenObject.name == hit.collider.gameObject.name)
                    {
                        activeHiddenObjectsList.RemoveAt(i);
                        break;
                    }
                }

                totalHiddenObjectsFound++;

                if (totalHiddenObjectsFound >= maxActiveHiddenObjectsCount)
                {
                    Debug.Log("Level Complete");
                }
            }
        }
    }
}

[System.Serializable]
public class HiddenObject
{
    public string name;
    public GameObject hiddenObject;
    public bool makeHidden = false;
}
