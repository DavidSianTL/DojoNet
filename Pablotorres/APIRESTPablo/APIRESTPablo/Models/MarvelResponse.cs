namespace APIRESTPablo.Models
{
    public class MarvelResponse
    {
        public MarvelData Data { get; set; }
    }

    public class MarvelData
    {
        public List<MarvelCharacter> Results { get; set; }
    }
}
