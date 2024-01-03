//Called by: backend/src/Controllers/CarController.cs

public class Car
{
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }

    public string Make { get; set; } = string.Empty;
    public double FuelEfficiency { get; set; } = 0;
}
