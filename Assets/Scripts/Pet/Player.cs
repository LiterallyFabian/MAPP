﻿using System;
using IKIMONO.UI;
using Newtonsoft.Json;
using UnityEngine;

namespace IKIMONO.Pet
{
    /// <summary>
    /// Represents a complete save file.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Player
    {
        public static Player Instance { get; private set; }

        #region Save file Variables

        /// <summary>
        /// The pet data.
        /// </summary>
        [JsonProperty("pet")] public Pet Pet { get; set; } = new Pet();

        /// <summary>
        /// The index for the chosen background. Not currently used.
        /// </summary>
        [JsonProperty("background")] public int BackgroundIndex { get; } = -1;

        /// <summary>
        /// The amount of money the player has.
        /// </summary>
        [JsonProperty("coins")] public int Coins { get; private set; } = 50;

        /// <summary>
        /// The DateTime the save file was created.
        /// </summary>
        [JsonProperty("createdAt")] public DateTime CreationDate { get; } = DateTime.Now;

        [JsonProperty("inventory")] public Inventory Inventory { get; private set; } = new Inventory();

        [JsonProperty("settings")] public Settings Settings { get; private set; } = new Settings();
        
        [JsonProperty("tutorialProgress")] public TutorialStep TutorialStep { get; set; } = TutorialStep.None;

        [JsonProperty("minigameJumpTutorialDone")] public bool MinigameJumpTutorialDone { get; set; } = false;

        #endregion

        // Init the instance.
        static Player()
        {
            Instance = Load();
        }

        #region Methods
        /// <summary>
        /// Saves this save file to the PlayerPrefs.
        /// </summary>
        public void Save()
        {
            PlayerPrefs.SetString("SaveFile", ToString());
            PlayerPrefs.Save();
            // Debug.Log("Saved save file.");
        }

        /// <summary>
        /// Creates a SaveFile class from the PlayerPrefs. If no save file exists, a new one is created.
        /// </summary>
        /// <returns>The SaveFile class.</returns>
        public static Player Load()
        {
            if (PlayerPrefs.HasKey("SaveFile"))
            {
                try
                {
                    string json = PlayerPrefs.GetString("SaveFile");
                    return JsonConvert.DeserializeObject<Player>(json);
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to load save file: " + e.Message); // TODO: Inform the user somehow? Maybe a popup, but this shouldn't happen.
                    return new Player();
                }
            }

            Debug.Log("No save file found.");
            //SetNamePanel.Open();
            return new Player();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GetCodeBlock()
        {
            return $"```json\n{ToString()}```";        
        }

        public void AddCoins(int amount)
        {
            Coins += amount;
        }
        public void RemoveCoins(int amount)
        {
            Coins = Math.Max(Coins - amount, 0);
        }

        public static void Reset()
        {            
            PlayerPrefs.DeleteAll();
            Instance = null;
        }
        #endregion
    }
}