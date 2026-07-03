// Common RNG Tables will inherit Weight field to be used for WeightedRandom.Pick
// All children must implement a Weight field
public interface IWeighted
{
    int Weight { get; }
} 