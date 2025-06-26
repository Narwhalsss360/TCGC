using TruckConnect;

namespace TCGC
{
    class GaugeClusterData() : DataDefinition(DEFINITION_ID)
    {
        public const int DEFINITION_ID = 0;

        [DataDefinitionMember(TelemetryID.TruckChannelFuel)]
        public ValueStorage<float> FuelLevel { get; set; } = new();

        [DataDefinitionMember(TelemetryID.TruckChannelOilTemperature)]
        public ValueStorage<float> OilTemperature { get; set; }=  new();

        [DataDefinitionMember(TelemetryID.TruckChannelEngineRpm)]
        public ValueStorage<float> Rpm { get; set; } = new();

        [DataDefinitionMember(TelemetryID.TruckChannelOdometer)]
        public ValueStorage<float> Odometer { get; set; } = new();

        [DataDefinitionMember(TelemetryID.TruckChannelSpeed)]
        public ValueStorage<float> Speed { get; set; } = new();

        [DataDefinitionMember(TelemetryID.TruckChannelBatteryVoltage)]
        public ValueStorage<float> BatteryVoltage { get; set; } = new();

        [DataDefinitionMember(TelemetryID.TruckChannelOilPressure)]
        public ValueStorage<float> OilPressure { get; set; } = new();

        [DataDefinitionMember(TelemetryID.ConfigurationTruckInfo)]
        public MasterStorage.ConfigurationStorage.ConfigurationTruckStorage TruckConfiguration = new();
    }
}
