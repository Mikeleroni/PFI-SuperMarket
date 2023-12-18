using UnityEngine;

public class ItemSwitching : MonoBehaviour
{
    public int selectedItem = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectItem();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedItem = selectedItem;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(selectedItem >= transform.childCount - 1)
            {
                selectedItem = 0;
            }
            else
            {
                selectedItem++;
            }
            
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedItem <= 0)
            {
                selectedItem = transform.childCount - 1;
            }
            else
            {
                selectedItem--;
            }

        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            selectedItem = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedItem = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedItem = 2;
        }

        if (previousSelectedItem != selectedItem)
        {
            SelectItem();
        }
    }
    void SelectItem()
    {
        int i = 0;
        foreach(Transform item in transform)
        {
            if(i == selectedItem)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
