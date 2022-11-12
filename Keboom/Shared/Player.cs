namespace Keboom.Shared;

public class Player
{
    private int _score;

    public event EventHandler? ScoreChanged;

    public string? Name { get; set; }
    public int Score
    {
        get => _score;
        set
        {
            if (_score != value)
            {
                _score = value;
                ScoreChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
    public string? Id { get; set; }
    public PlayerColor Color { get; set; }
}
