using Microsoft.EntityFrameworkCore;

namespace IsleBot;

public class Match {
    public int MatchId { get; set; } 
    
    public User PlayerA { get; set; }
    public Card PlayerACard { get; set; }
    
    public User PlayerB { get; set; }
    public Card PlayerBCard { get; set; }
    
    public int PlayedTurns { get; set; }
    
    public EStatus Status { get; set; }
}