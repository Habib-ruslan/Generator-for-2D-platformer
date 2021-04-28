using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public GameObject startWindow;
    public GameObject settingsWindow;
    public GameObject menuWindow;

    public GameObject[] RadioButtonsCheck;
    private int[] CustomChancesValue = new int[3];
    public Text[] CustomChances;

    private int lengthLvl = 0;
    private int customChances = 000000; // 0% 0% 0% 
    public void GoGenerateLvL()
    {
        SaveChanges();
        LoadScene(0);
    }
    public void Play()
    {
        startWindow.SetActive(false);
        settingsWindow.SetActive(true);
    }
    public void ToStartWindow()
    {
        startWindow.SetActive(true);
        settingsWindow.SetActive(false);
    }
    public void RadioButtonActive(int num)
    {
        for (int i = 0; i<RadioButtonsCheck.Length; i++)
        {
            if (i == num) RadioButtonsCheck[i].SetActive(true);
            else RadioButtonsCheck[i].SetActive(false);
        }
        lengthLvl = num;
    }
    public void IncreaseCustomChance(int BAndNum)
    {
        int num = 0, b = 0;
        Separate(ref b, ref num, BAndNum);
        if (sumChances() + b <= 100)
        {
            CustomChancesValue[num] += b;
            if (CustomChancesValue[num] >= 100) CustomChancesValue[num] -= 100;
            UpdateCustomChance(num);
        }
    }
    public void DecreaseCustomChance(int BAndNum)
    {
        int num=0, b=0;
        Separate(ref b, ref num, BAndNum);
        CustomChancesValue[num] -= b;
        if (CustomChancesValue[num] < 0)
        {
            if (sumChances() + 100 <= 100) CustomChancesValue[num] += 100;
            else CustomChancesValue[num] = 0;
        }
        UpdateCustomChance(num);
    }
    private int sumChances()
    {
        int sum = 0;
        for(int i = 0;i< CustomChancesValue.Length; i++) sum += CustomChancesValue[i];
        return sum;
    }
    private void Separate(ref int a, ref int b, int AB)
    {
        a = AB / 10;
        b = AB % 10;
    }    

    private void SaveChanges()
    {
        PlayerPrefs.SetInt("Length", lengthLvl);
        PlayerPrefs.SetInt("Custom", customChances);
    }
    private void UpdateCustomChance(int num)
    {
        CustomChances[num].text = CustomChancesValue[num].ToString();
        customChances = CustomChancesValue[0] * 10000 + CustomChancesValue[1] * 100 + CustomChancesValue[2];

    }
    public void FromLvlToStartWindow()
    {
        LoadScene(1);
    }
    private void LoadScene(int num)
    {
        SceneManager.LoadScene(num);
    }
    public void Close()
    {
        menuWindow.SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
