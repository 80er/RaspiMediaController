export class WeatherModuleEntry {
  public name: string;
  public humidity: string;
  public temperature: string;
  public co2: string;
  public type: string;
  public pressure: string;
  constructor(data) {
    this.name = data.Name;
    this.humidity = data.Humidity;
    this.temperature = data.Temperature;
    this.co2 = data.CO2;
    this.type = data.Type;
    if (data.Type === 0) {
      this.pressure = data.Pressure;
    }
  }
}
