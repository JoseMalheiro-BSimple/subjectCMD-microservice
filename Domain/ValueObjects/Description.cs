namespace Domain.ValueObjects;

public record Description
{
    public string Value { get; }

    public Description(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Description can't be empty!");

        if (value.Length > 50)
            throw new ArgumentException("Description has a max 50 characters!");

        Value = value;
    }
}
