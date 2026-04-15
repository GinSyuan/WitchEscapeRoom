using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PotionRecipe
{
    public string potionName;
    public List<string> requiredIngredients;
    public GameObject potionToActivate;
}

public class Cauldron : MonoBehaviour
{
    [Header("Brewing Settings")]
    public List<PotionRecipe> allRecipes;

    [Header("Win Settings (勝利設定)")]
    public GameObject winScreenUI; // The "You Escaped" text object 
    public int potionsNeededToWin = 3; // How many to win 

    private PotionRecipe currentActiveRecipe = null;
    private List<string> ingredientsInside = new List<string>();
    private List<GameObject> physicalItemsInCauldron = new List<GameObject>();

    // NEW: Memory to track which potions are finished!
    private List<string> brewedPotions = new List<string>();

    void OnTriggerEnter(Collider other)
    {
        MagicIngredient item = other.GetComponent<MagicIngredient>();
        if (item != null) ProcessIngredient(item);
    }

    void ProcessIngredient(MagicIngredient item)
    {
        if (currentActiveRecipe == null)
        {
            currentActiveRecipe = FindRecipeThatUses(item.ingredientName);
            if (currentActiveRecipe != null) AcceptItem(item);
            else item.RespawnOnTable();
        }
        else
        {
            bool isRequired = currentActiveRecipe.requiredIngredients.Contains(item.ingredientName);
            bool isAlreadyInside = ingredientsInside.Contains(item.ingredientName);

            if (isRequired && !isAlreadyInside)
            {
                AcceptItem(item);
                CheckIfPotionFinished();
            }
            else
            {
                UnityEngine.Debug.Log("Wrong ingredient! Respawning...");
                item.RespawnOnTable();
            }
        }
    }

    void AcceptItem(MagicIngredient item)
    {
        ingredientsInside.Add(item.ingredientName);
        physicalItemsInCauldron.Add(item.gameObject);
    }

    void CheckIfPotionFinished()
    {
        if (ingredientsInside.Count == currentActiveRecipe.requiredIngredients.Count)
        {
            UnityEngine.Debug.Log("Potion Complete: " + currentActiveRecipe.potionName);

            if (currentActiveRecipe.potionToActivate != null)
            {
                currentActiveRecipe.potionToActivate.SetActive(true);
            }



            if (!brewedPotions.Contains(currentActiveRecipe.potionName))
            {
                brewedPotions.Add(currentActiveRecipe.potionName);
            }


            if (brewedPotions.Count >= potionsNeededToWin)
            {
                TriggerWinState();
            }


            foreach (GameObject obj in physicalItemsInCauldron)
            {
                Destroy(obj);
            }

            currentActiveRecipe = null;
            ingredientsInside.Clear();
            physicalItemsInCauldron.Clear();
        }
    }

    void TriggerWinState()
    {
        UnityEngine.Debug.Log("YOU ESCAPED! Game Over.");

        if (winScreenUI != null)
        {
            winScreenUI.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    PotionRecipe FindRecipeThatUses(string ingredientName)
    {
        foreach (PotionRecipe recipe in allRecipes)
        {
            if (recipe.requiredIngredients.Contains(ingredientName)) return recipe;
        }
        return null;
    }
}