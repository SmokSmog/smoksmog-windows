namespace SmokSmog.Services.Search
{
    public interface ISearchable
    {
        SearchSettings Settings { get; }

        SearchQuerry Querry { get; set; }
    }
}