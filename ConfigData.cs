namespace RegenPercent
{
    public class ConfigData
    {
        public int HealthRegenRateStep { get; set; }
        public int StaminaRegenRateStep { get; set; }

        public ConfigData() : this(ModEntry.DefaultHealthRegenRateStep, ModEntry.DefaultStaminaRegenRateStep) {  }
        public ConfigData(int healthRegenRateStep, int staminaRegenRateStep) 
        {
            HealthRegenRateStep = healthRegenRateStep;
            StaminaRegenRateStep = staminaRegenRateStep;
        }
    }
}
