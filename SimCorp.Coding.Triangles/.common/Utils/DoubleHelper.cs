namespace SimCorp.Coding.Triangles;

public class DoubleHelper : IDoubleHelper, IComparer<double>, IEqualityComparer<double>
{
    public double Epsilon
    {
        get { return 0.001d; }
    }

    public bool Equals(double a, double b)
    {
        return Math.Abs(a - b) < Epsilon;
    }

    public int GetHashCode(double obj)
    {
        return obj.GetHashCode();
    }

    public int Compare(double a, double b)
    {
        // take into account Epsilon.
        if (Equals(Math.Abs(a - b), 0))
        {
            return 0;
        }

        return a > b ? 1 : -1;
    }
}