using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryButton : MonoBehaviour
{
    public Sprite buttonSprite;
    public Sprite[] gunSkins = new Sprite[5];
    public int NumberButton = 1; // число по замовчуванню на кнопці
    private int countButton = 5; // кількість кнопок
    private string keyNumberButton = "NumberButton";

    void Start()
    {
        //PlayerPrefs.DeleteAll ();
        int valueButton = 0;

        for (int i = 1; i <= countButton; i++)
        {
            if (PlayerPrefs.HasKey(keyNumberButton + i.ToString())) //	возвращает true, если ключ существует в хранилище	
            {
                valueButton = PlayerPrefs.GetInt(keyNumberButton + i.ToString()); // зчитуємо збереженне значення для кожної і-тої кнопки
                //SetZombiesCountItem(i, valueButton); // встановлюємо для кожної кнопки її збережене значення
            }
            else
            {
                valueButton = i;               // якщо значення не збережене, то для третьої кнопки (наприклад) тре встановити значення по замовчуванню 3	
                //SetZombiesCountItem(i, valueButton); // встановлюємо для кожної кнопки її збережене значення
            }
        }
    }

    void SetZombiesCountItem(int numberButton, int valueButton) // numberButton - номер кнопки 1,2,3... valueButton - значення на кнопці, яке тре встановити
    {
        Text textLabel;
        Image buttonImage;

        textLabel = GameObject.FindGameObjectWithTag("InventoryTextLabel" + numberButton.ToString()).GetComponent<Text>();
        buttonImage = GameObject.FindGameObjectWithTag("InventoryButton" + numberButton.ToString()).GetComponent<Image>();

        if (valueButton == 0)
        {
            textLabel.text = "Open";
            buttonImage.sprite = gunSkins[0];
        }
        else
        {
            textLabel.text = valueButton.ToString();
        }
    }

    public void OnInventoryButtonClick()
    {
        // NumberButton - це номер кнопки на яку клікнули, тобто перша, друга, третя... задаєм з юніті
        int valueButton = 0;
        if (PlayerPrefs.GetInt("zombiescount") > 0)
        {       
            if (PlayerPrefs.HasKey(keyNumberButton + NumberButton.ToString()))
            {                //	возвращает true, если ключ существует в хранилище	
                valueButton = PlayerPrefs.GetInt(keyNumberButton + NumberButton.ToString()); // зчитуємо збереженне значення для NumberButton кнопки
            }
            else
            {
                valueButton = NumberButton;
            }
            valueButton--;
            if (valueButton < 0)
            {
                valueButton = 0;
                SceneManager.LoadScene("Menu");
                PlayerPrefs.SetInt("PressedButtonNumber", NumberButton);
                PlayerPrefs.Save();
            }
            else
            {
                ZombieController.ZombiesCounter--;
                if (ZombieController.ZombiesCounter < 0)
                    ZombieController.ZombiesCounter = 0;

                PlayerPrefs.SetInt("zombiescount", ZombieController.ZombiesCounter);
                PlayerPrefs.Save();
            }
            // зберігаєм нове значення для цієї кнопки NumberButton
            PlayerPrefs.SetInt(keyNumberButton + NumberButton.ToString(), valueButton);
            PlayerPrefs.Save();
            //---
            SetZombiesCountItem(NumberButton, valueButton);
        }
        else
        {
            if (PlayerPrefs.HasKey(keyNumberButton + NumberButton.ToString()))
            {                //	возвращает true, если ключ существует в хранилище	
                valueButton = PlayerPrefs.GetInt(keyNumberButton + NumberButton.ToString()); // зчитуємо збереженне значення для NumberButton кнопки
            }
            else
            {
                valueButton = NumberButton;
            }

            if (valueButton == 0)
            {
                SceneManager.LoadScene("Menu");
                PlayerPrefs.SetInt("PressedButtonNumber", NumberButton);
                PlayerPrefs.Save();
            }
        }
    }
}