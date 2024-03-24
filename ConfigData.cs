namespace RegenPercent
{
    public class ConfigData
    {
        public float HealthRegenRate { get; set; }
        public float StaminaRegenRate { get; set; }

        public ConfigData() : this(ModEntry.DefaultHealthRegenRate, ModEntry.DefaultStaminaRegenRate) { }
        public ConfigData(float healthRegenRate, float staminaRegenRate) 
        {
            HealthRegenRate = healthRegenRate;
            StaminaRegenRate = staminaRegenRate;
        }
    }
}
