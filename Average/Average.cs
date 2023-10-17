namespace Average;

public class Average
{
    public static double ComputeMeanOfFile(string path)
    {
        FileAccess fileAccess = new FileAccess(path);
        List<int> numbers = fileAccess.ReadNumbers();
        return Statistics.Mean(numbers);
    }

    public static double ComputeMedianOfFile(string path)
    {
        FileAccess fileAccess = new FileAccess(path);
        List<int> numbers = fileAccess.ReadNumbers();
        return Statistics.Median(numbers);
    }

    public static List<int> ComputeModeOfFile(string path)
    {
        FileAccess fileAccess = new FileAccess(path);
        List<int> numbers = fileAccess.ReadNumbers();
        return Statistics.Mode(numbers);
    }
}
