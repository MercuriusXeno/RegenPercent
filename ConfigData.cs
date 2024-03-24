namespace RegenPercent
{
    public class ConfigData
    {
        public float HealthRegenRate { get; set; }
        public float StaminaRegenRate { get; set; }

        public ConfigData() : this(1f, 1f) { }
        public ConfigData(float healthRegenRate, float staminaRegenRate) 
        {
            HealthRegenRate = healthRegenRate;
            StaminaRegenRate = staminaRegenRate;
        }
    }
}
