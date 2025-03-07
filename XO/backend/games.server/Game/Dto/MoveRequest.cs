namespace Game.Dto
{
    public class MoveRequest
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public string CurrentPlayer { get; set; }
        public List<List<string>> Board { get; set; }
    }
}