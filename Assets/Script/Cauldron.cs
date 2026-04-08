using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PotionRecipe
{
    public string potionName;
    public List<string> requiredIngredients;

    [Header("The Potion Object already on the table")]
    public GameObject potionToActivate; // NEW: We just need a reference to the hidden object (新增：只需要這個隱藏物件的參考)
}

public class Cauldron : MonoBehaviour
{
    [Header("Brewing Settings")]
    public List<PotionRecipe> allRecipes;

    private PotionRecipe currentActiveRecipe = null;
    private List<string> ingredientsInside = new List<string>();
    private List<GameObject> physicalItemsInCauldron = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        MagicIngredient item = other.GetComponent<MagicIngredient>();

        if (item != null)
        {
            ProcessIngredient(item);
        }
    }

    void ProcessIngredient(MagicIngredient item)
    {
        if (currentActiveRecipe == null)
        {
            currentActiveRecipe = FindRecipeThatUses(item.ingredientName);

            if (currentActiveRecipe != null)
            {
                AcceptItem(item);
            }
            else
            {
                item.RespawnOnTable();
            }
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
        UnityEngine.Debug.Log("Added " + item.ingredientName + " to cauldron.");
    }

    void CheckIfPotionFinished()
    {
        if (ingredientsInside.Count == currentActiveRecipe.requiredIngredients.Count)
        {
            UnityEngine.Debug.Log("Potion Complete: " + currentActiveRecipe.potionName);

            // UPDATED: Simply turn on the hidden potion! (更新：直接開啟隱藏的魔藥！)
            if (currentActiveRecipe.potionToActivate != null)
            {
                currentActiveRecipe.potionToActivate.SetActive(true);
            }
            else
            {
                UnityEngine.Debug.LogError("You forgot to assign the hidden potion object for " + currentActiveRecipe.potionName + "!");
            }

            // Destroy the used ingredients
            foreach (GameObject obj in physicalItemsInCauldron)
            {
                Destroy(obj);
            }

            // Reset the cauldron
            currentActiveRecipe = null;
            ingredientsInside.Clear();
            physicalItemsInCauldron.Clear();
        }
    }

    PotionRecipe FindRecipeThatUses(string ingredientName)
    {
        foreach (PotionRecipe recipe in allRecipes)
        {
            if (recipe.requiredIngredients.Contains(ingredientName))
            {
                return recipe;
            }
        }
        return null;
    }
}