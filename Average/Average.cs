namespace Average;

public class Average
{
    public Average(IFileOperations fileAccess)
    {
        this.fileAccess = fileAccess;
    }

    private readonly IFileOperations fileAccess;

    public double ComputeMeanOfFile()
    {
        List<int> numbers = fileAccess.ReadNumbers();
        return Statistics.Mean(numbers);
    }

    public double ComputeMedianOfFile()
    {
        List<int> numbers = fileAccess.ReadNumbers();
        return Statistics.Median(numbers);
    }

    public List<int> ComputeModeOfFile()
    {
        List<int> numbers = fileAccess.ReadNumbers();
        return Statistics.Mode(numbers);
    }
}
