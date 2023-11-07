using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IsleBot;

public class Match {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [DataMember(IsRequired = true)]
    public int Id { get; set; } 
    [ForeignKey("PlayerAId"), Column(Order = 1)]
    public User PlayerA { get; set; }
    [ForeignKey("PlayerACardId"), Column(Order = 2)]
    public Card PlayerACard { get; set; }
    
    [ForeignKey("PlayerBId"), Column(Order=3)]
    public User PlayerB { get; set; }
    [ForeignKey("PlayerBCardId"), Column(Order=4)]
    public Card PlayerBCard { get; set; }
    
    public int PlayedTurns { get; set; }

    public EStatus Status { get; set; }

    public Match() { } //required for EFCore build

    public Match(User playerA, User playerB, Card playerACard, Card playerBCard) {
        PlayerA = playerA;
        PlayerB = playerB;
        PlayerACard = playerACard;
        PlayerBCard = playerBCard;
        PlayedTurns = 0;
        Status = EStatus.NotStarted;
    }
}