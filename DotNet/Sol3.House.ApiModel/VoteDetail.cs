namespace Sol3.House.ApiModel
{
    public interface IDetail
    {
        int VoteValue { get; set; }
        int Weight { get; set; }
        string Title { get; set; }
    }
    public class VoteDetail : IDetail
    {
        public bool IsChecked => VoteValue > 0;
        public int VoteValue { get; set; }
        public int Weight { get; set; }
        public string Title { get; set; }
    }
}
