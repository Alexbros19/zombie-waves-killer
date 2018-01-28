using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShootgunMenuUI : MonoBehaviour {
    [SerializeField]
    private Text zombiesCountLable;
    [SerializeField]
    private Text collectZombiesText;
    //[SerializeField]
    //private Text inventoryZombiesCount1;
    [SerializeField]
    private GameObject collectZombiesObject;
    [SerializeField]
    private GameObject[] openWeapons;
    [SerializeField]
    private GameObject[] weaponsNames;
    [SerializeField]
    private Sprite openWeaponBackground;

    void Start () {
        //PlayerPrefs.DeleteAll();
        zombiesCountLable.text = PlayerPrefs.GetInt("zombiescount", 0).ToString();

        for (int i = 2; i <= 6; i++) {
            if (PlayerPrefs.GetInt("inventorybutton" + i, 0) == 1) {
                GameObject.FindGameObjectWithTag("InventoryButton" + i).SetActive(false);
            }
            if (PlayerPrefs.GetInt("openweapon" + i, 0) == 1)
            {
                openWeapons[i - 2].SetActive(true);
            }
            if (PlayerPrefs.GetInt("inventorytextlabel" + i, 0) == 1)
            {
                GameObject.FindGameObjectWithTag("InventoryTextLabel" + i).SetActive(false);
            }
            if (PlayerPrefs.GetInt("openweaponname" + i, 0) == 1)
            {
                weaponsNames[i - 2].SetActive(true);
            }
            if (PlayerPrefs.GetInt("weaponbackground" + i, 0) == 1)
            {
                GameObject.FindGameObjectWithTag("WeaponBackground" + i).GetComponent<Image>().sprite = openWeaponBackground;
            }
        }
    }

    public void BackButton() {
        SceneManager.LoadScene("Menu");
    }

    public void InventoryButton() {
        // if collect zombies count >= zombies count in text label 
        if (PlayerPrefs.GetInt("zombiescount", 0) >= PlayerPrefs.GetInt("zombiescounttounlock", 0))
        {
            StartCoroutine(DecreaseTextLabelNumber(int.Parse(GameObject.FindGameObjectWithTag("InventoryTextLabel" + OnInventoryButtonClick.Number).GetComponent<Text>().text), 0, 1f));
        }
        else {
            collectZombiesObject.SetActive(true);
            StartCoroutine(FadeText(Color.clear, Color.red, 1f));
        }
    }

    public void InventoryOpenButton() {
        SceneManager.LoadScene("Menu");
    }

    // fade animation of 'please collect zombies to unlock' text
    IEnumerator FadeText(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            collectZombiesText.color = Color.Lerp(from, to, percent);
            GameObject.FindGameObjectWithTag("InventoryTextLabel" + OnInventoryButtonClick.Number).GetComponent<Text>().color = Color.Lerp(Color.white, Color.red, percent);
            percent += speed * Time.deltaTime;
            yield return null;
        }
        percent = 0;
        while (percent < 1)
        {
            collectZombiesText.color = Color.Lerp(to, from, percent);
            GameObject.FindGameObjectWithTag("InventoryTextLabel" + OnInventoryButtonClick.Number).GetComponent<Text>().color = Color.Lerp(Color.red, Color.white, percent);
            percent += speed * Time.deltaTime;
            yield return null;
        }

        collectZombiesObject.SetActive(false);
    }

    IEnumerator DecreaseTextLabelNumber(int from, int to, float time) {
        float speed = .5f / time;
        float percent = 0;
        int number = 0;
        int updatedZombiesCount = PlayerPrefs.GetInt("zombiescount", 0) - int.Parse(GameObject.FindGameObjectWithTag("InventoryTextLabel" + OnInventoryButtonClick.Number).GetComponent<Text>().text);

        while (percent < 1) {
            from = (int)Mathf.Lerp(from, to, percent);
            // decrease zombies count from counter
            number = (int)Mathf.Lerp(PlayerPrefs.GetInt("zombiescount", 0), updatedZombiesCount, percent);
            GameObject.FindGameObjectWithTag("InventoryTextLabel" + OnInventoryButtonClick.Number).GetComponent<Text>().text = from.ToString();
            zombiesCountLable.text = number.ToString();
            percent += speed * Time.deltaTime;
            yield return null;
        }
        // save updated value of zombies count in counter
        PlayerPrefs.SetInt("zombiescount", updatedZombiesCount);

        // hide inventory button 
        GameObject.FindGameObjectWithTag("InventoryButton" + OnInventoryButtonClick.Number).SetActive(false);
        PlayerPrefs.SetInt("inventorybutton" + OnInventoryButtonClick.Number, 1);
        
        // unlock weapon begin second
        openWeapons[OnInventoryButtonClick.Number - 2].SetActive(true);
        PlayerPrefs.SetInt("openweapon" + OnInventoryButtonClick.Number, 1);

        // hide inventory text label
        GameObject.FindGameObjectWithTag("InventoryTextLabel" + OnInventoryButtonClick.Number).SetActive(false);
        PlayerPrefs.SetInt("inventorytextlabel" + OnInventoryButtonClick.Number, 1);

        // show weapon name
        weaponsNames[OnInventoryButtonClick.Number - 2].SetActive(true);
        PlayerPrefs.SetInt("openweaponname" + OnInventoryButtonClick.Number, 1);

        // change weapon background to open
        GameObject.FindGameObjectWithTag("WeaponBackground" + OnInventoryButtonClick.Number).GetComponent<Image>().sprite = openWeaponBackground;
        PlayerPrefs.SetInt("weaponbackground" + OnInventoryButtonClick.Number, 1);
    }
}
