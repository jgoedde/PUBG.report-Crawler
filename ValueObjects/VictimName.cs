namespace PubgReportCrawler.ValueObjects;

public sealed record VictimName(string Name)
{
    public static VictimName From(string name)
    {
        return new VictimName(name);
    }

    public override string ToString()
    {
        return Name;
    }

    public bool Equals(VictimName? other)
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
