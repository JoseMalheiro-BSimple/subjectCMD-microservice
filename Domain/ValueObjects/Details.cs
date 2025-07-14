namespace Domain.ValueObjects;

public record Details
{
    public string Value { get; }

    public Details(string value)
    {
        if (value == null)
            throw new ArgumentNullException("Description can't be null!");

        if (value.Length > 500)
            throw new ArgumentException("Details has a max 500 characters!");

        Value = value;
    }
}
