namespace PubgReportCrawler.ValueObjects;

public sealed record KillerName(string Name)
{
    public static KillerName From(string name)
    {
        return new KillerName(name);
    }

    public override string ToString()
    {
        return Name;
    }

    public bool Equals(KillerName? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Name == other.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
