using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace RegenPercent
{
    /// <summary>
    ///     The mod entry for regen-percent. 
    ///     Needed for there to be a mod.
    /// </summary>
    internal sealed class ModEntry : Mod
    {
        private ConfigData _config;
        const float MinHealthPerSecond = 0f;
        const float MaxHealthPerSecond = 10f;
        const float MinStaminaPerSecond = 0f;
        const float MaxStaminaPerSecond = 10f;
        const float PercentageCoefficient = 0.01f;
        const float PerFrameCoefficient = 60f;
        private float _healthRegenTracker = 0f;
        private float _staminaRegenTracker = 0f;

        /// <summary>
        ///     Needed to supply the mod entry point which lets the mod do things.
        /// </summary>
        /// <param name="helper">A provider-pattern helper that supplies api methods, events, etc</param>
        public override void Entry(IModHelper helper)
        {
            ReadConfig(helper);
            HookEvents(helper);
        }

        /// <summary>
        ///     Needed to wire up the mod's events.
        /// </summary>
        /// <param name="helper">Helper instance needed to hook events</param>
        private void HookEvents(IModHelper helper)
        {
            helper.Events.GameLoop.GameLaunched += GameLaunched;
            helper.Events.GameLoop.UpdateTicked += UpdateTicked;
        }

        /// <summary>
        ///     Needed to read the config from a file if it exists
        /// </summary>
        /// <param name="helper">Helper instance needed to read configs</param>
        private void ReadConfig(IModHelper helper)
        {
            _config = helper.ReadConfig<ConfigData>();
        }

        /// <summary>
        ///     Needed to update stamina and health by a percentage, if applicable.
        /// </summary>
        /// <param name="sender">The event sender as a nullable object</param>
        /// <param name="e">The event arguments</param>   
        private void UpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (IsContextValidForRegen())
            {
                HandleRegen(Game1.player);
            }
        }

        /// <summary>
        ///     Needed to track whether player has received enough regen
        ///     to increment health or stamina.
        /// </summary>
        /// <param name="player">The player regenerating</param>        
        private void HandleRegen(Farmer player)
        {
            HandleHealthRegen(player);
            HandleStaminaRegen(player);
        }

        /// <summary>
        ///     Needed to do the stamina portion of regenerating
        /// </summary>
        /// <param name="player">The player regenerating</param>    
        private void HandleStaminaRegen(Farmer player)
        {
            _staminaRegenTracker += player.MaxStamina * _config.StaminaRegenRate * PercentageCoefficient / PerFrameCoefficient;
            var regen = (int)Math.Floor(_staminaRegenTracker);
            if (regen >= 1)
            {
                _staminaRegenTracker = 0f;
                player.Stamina = Math.Min(player.MaxStamina, player.Stamina + regen);
            }
        }
        /// <summary>
        ///     Needed to do the health portion of regenerating
        /// </summary>
        /// <param name="player">The player regenerating</param>    
        private void HandleHealthRegen(Farmer player)
        {
            _healthRegenTracker += player.maxHealth * _config.HealthRegenRate * PercentageCoefficient / PerFrameCoefficient;
            var regen = (int)Math.Floor(_healthRegenTracker);
            if (regen >= 1)
            {
                _healthRegenTracker = 0f;
                player.health = Math.Min(player.maxHealth, player.health + regen);
            }
        }

        /// <summary>
        ///     Needed to determine if regen should happen on a given tick
        /// </summary>
        /// <returns>True if regen is valid, false otherwise</returns>        
        private bool IsContextValidForRegen()
        {
            return Context.IsWorldReady && Context.IsPlayerFree;
        }

        /// <summary>
        ///     Needed to fire anything that waits for the game to launch.
        /// </summary>
        /// <param name="sender">The event sender as a nullable object</param>
        /// <param name="e">The event arguments</param>
        private void GameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            SetupConfig();
        }

        /// <summary>
        ///     Needed to establish the config values and set the supplier and setter for regen rates.
        /// </summary>
        private void SetupConfig()
        {
            if (GetModConfigApi() is IGenericModConfigMenuApi configApi)
            {
                configApi.Register(ModManifest, () => _config = new ConfigData(), () => Helper.WriteConfig(_config));                
                configApi.AddNumberOption(ModManifest, () => (float)_config.HealthRegenRate, (float f) => _config.HealthRegenRate = f,
                    () => Strings.HealthRegenRateLabel, () => Strings.HealthRegenRateDescription, MinHealthPerSecond, MaxHealthPerSecond, 0.1f);
                configApi.AddNumberOption(ModManifest, () => (float)_config.StaminaRegenRate, (float f) => _config.StaminaRegenRate = f,
                    () => Strings.StaminaRegenRateLabel, () => Strings.StaminaRegenRateDescription, MinStaminaPerSecond, MaxStaminaPerSecond, 0.1f);
            }
        }

        /// <summary>
        ///     Needed to get the mod config api, if it exists.
        /// </summary>
        /// <returns>An instance of <see cref="IGenericModConfigMenuApi"/> if one can be found, otherwise null.</returns>
        private IGenericModConfigMenuApi? GetModConfigApi()
        {
            return Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>(Strings.CaseyConfigMenuApi);
        }
    }
}