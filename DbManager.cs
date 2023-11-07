using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace IsleBot;

public class DbManager : DbContext {
    public DbSet<User> Players { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Match> Matches { get; set; }

    public string dbpath { get; } = "database.db";

    public DbManager() {
        var fullPath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + dbpath;
        dbpath = fullPath;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={dbpath}");
    
    #region Players Functions

    public List<User> GetAllPlayers() => Players.ToList();
    public User? GetPlayerById(ulong id) => Players.Find(id);
    
    public void AddPlayer(User player) {
        Players.Add(player);
        SaveChanges();
    }
    
    public void RemovePlayer(User player) {
        Players.Remove(player);
        SaveChanges();
    }
    
    public void UpdatePlayer(User player) {
        Players.Update(player);
        SaveChanges();
    }
    #endregion
    
    #region Cards Functions
    public List<Card> GetAllCards() => Cards.ToList();
    public Card? GetCardById(int id) => Cards.Find(id);
    
    public void AddCard(Card card) {
        Cards.Add(card);
        SaveChanges();
    }
    
    public void RemoveCard(Card card) {
        Cards.Remove(card);
        SaveChanges();
    }
    
    public void UpdateCard(Card card) {
        Cards.Update(card);
        SaveChanges();
    }
    
    public List<Card> GetCardsByPlayerId(ulong id) => Cards.Where(card => card.Owner.Id == id).ToList();
    public List<Card> GetCardsByPlayer(User player) => Cards.Where(card => card.Owner == player).ToList();
    public List<Card> GetCardsByPlayerId(ulong id, int limit) => Cards.Where(card => card.Owner.Id == id).Take(limit).ToList();
    public List<Card> GetCardsByPlayer(User player, int limit) => Cards.Where(card => card.Owner == player).Take(limit).ToList();
    public List<Card> GetCardsByPlayerId(ulong id, int limit, int offset) => Cards.Where(card => card.Owner.Id == id).Skip(offset).Take(limit).ToList();
    public List<Card> GetCardsByPlayer(User player, int limit, int offset) => Cards.Where(card => card.Owner == player).Skip(offset).Take(limit).ToList();
    //probably gonna add extra options filtered by card class or something like that in the future
    
    #endregion
    
    #region Matches Functions
    public IEnumerable<Match> GetAllMatches() => Matches;

    public Match? GetMatchById(int id) => Matches.Find(id);
    
    public void AddMatch(Match match) {
        Matches.Add(match);
        SaveChanges();
    }
    
    public void RemoveMatch(Match match) {
        Matches.Remove(match);
        SaveChanges();
    }
    
    public void UpdateMatch(Match match) {
        Matches.Update(match);
        SaveChanges();
    }
    
    //By player
    public List<Match> GetMatchesByPlayerId(ulong id) => Matches.Where(match => match.PlayerA.Id == id || match.PlayerB.Id == id).ToList();
    public List<Match> GetMatchesByPlayer(User player) => Matches.Where(match => match.PlayerA == player || match.PlayerB == player).ToList();
    public List<Match> GetMatchesByPlayerId(ulong id, int limit) => Matches.Where(match => match.PlayerA.Id == id || match.PlayerB.Id == id).Take(limit).ToList();
    public List<Match> GetMatchesByPlayer(User player, int limit) => Matches.Where(match => match.PlayerA == player || match.PlayerB == player).Take(limit).ToList();
    public List<Match> GetMatchesByPlayerId(ulong id, int limit, int offset) => Matches.Where(match => match.PlayerA.Id == id || match.PlayerB.Id == id).Skip(offset).Take(limit).ToList();
    public List<Match> GetMatchesByPlayer(User player, int limit, int offset) => Matches.Where(match => match.PlayerA == player || match.PlayerB == player).Skip(offset).Take(limit).ToList();
    
    //By status
    public List<Match> GetMatchesByStatus(EStatus status) => Matches.Where(match => match.Status == status).ToList();
    public List<Match> GetMatchesByStatus(EStatus status, int limit) => Matches.Where(match => match.Status == status).Take(limit).ToList();
    public List<Match> GetMatchesByStatus(EStatus status, int limit, int offset) => Matches.Where(match => match.Status == status).Skip(offset).Take(limit).ToList();
    
    //By card
    public List<Match> GetMatchesByCardId(int id)=> Matches.Where(match => match.PlayerACard.Id == id || match.PlayerBCard.Id == id).ToList();
    public List<Match> GetMatchesByCard(Card card) => Matches.Where(match => match.PlayerACard == card || match.PlayerBCard == card).ToList();
    public List<Match> GetMatchesByCardId(int id, int limit) => Matches.Where(match => match.PlayerACard.Id == id || match.PlayerBCard.Id == id).Take(limit).ToList();
    public List<Match> GetMatchesByCard(Card card, int limit) => Matches.Where(match => match.PlayerACard == card || match.PlayerBCard == card).Take(limit).ToList();
    public List<Match> GetMatchesByCardId(int id, int limit, int offset) => Matches.Where(match => match.PlayerACard.Id == id || match.PlayerBCard.Id == id).Skip(offset).Take(limit).ToList();
    public List<Match> GetMatchesByCard(Card card, int limit, int offset) => Matches.Where(match => match.PlayerACard == card || match.PlayerBCard == card).Skip(offset).Take(limit).ToList();
    
    //By player and status
    public List<Match> GetMatchesByStatusAndPlayerId(EStatus status, ulong id) => Matches.Where(match => match.Status == status && (match.PlayerA.Id == id || match.PlayerB.Id == id)).ToList();
    public List<Match> GetMatchesByStatusAndPlayer(EStatus status, User player) => Matches.Where(match => match.Status == status && (match.PlayerA == player || match.PlayerB == player)).ToList();
    public List<Match> GetMatchesByStatusAndPlayerId(EStatus status, ulong id, int limit) => Matches.Where(match => match.Status == status && (match.PlayerA.Id == id || match.PlayerB.Id == id)).Take(limit).ToList();
    public List<Match> GetMatchesByStatusAndPlayer(EStatus status, User player, int limit) => Matches.Where(match => match.Status == status && (match.PlayerA == player || match.PlayerB == player)).Take(limit).ToList();
    public List<Match> GetMatchesByStatusAndPlayerId(EStatus status, ulong id, int limit, int offset) => Matches.Where(match => match.Status == status && (match.PlayerA.Id == id || match.PlayerB.Id == id)).Skip(offset).Take(limit).ToList();
    public List<Match> GetMatchesByStatusAndPlayer(EStatus status, User player, int limit, int offset) => Matches.Where(match => match.Status == status && (match.PlayerA == player || match.PlayerB == player)).Skip(offset).Take(limit).ToList();
    
    //By card and status
    public List<Match> GetMatchesByCardIdAndStatus(int id, EStatus status) => Matches.Where(match => match.Status == status && (match.PlayerACard.Id == id || match.PlayerBCard.Id == id)).ToList();
    public List<Match> GetMatchesByCardAndStatus(Card card, EStatus status) => Matches.Where(match => match.Status == status && (match.PlayerACard == card || match.PlayerBCard == card)).ToList();
    public List<Match> GetMatchesByCardIdAndStatus(int id, EStatus status, int limit) => Matches.Where(match => match.Status == status && (match.PlayerACard.Id == id || match.PlayerBCard.Id == id)).Take(limit).ToList();
    public List<Match> GetMatchesByCardAndStatus(Card card, EStatus status, int limit) => Matches.Where(match => match.Status == status && (match.PlayerACard == card || match.PlayerBCard == card)).Take(limit).ToList();
    public List<Match> GetMatchesByCardIdAndStatus(int id, EStatus status, int limit, int offset) => Matches.Where(match => match.Status == status && (match.PlayerACard.Id == id || match.PlayerBCard.Id == id)).Skip(offset).Take(limit).ToList();
    public List<Match> GetMatchesByCardAndStatus(Card card, EStatus status, int limit, int offset) => Matches.Where(match => match.Status == status && (match.PlayerACard == card || match.PlayerBCard == card)).Skip(offset).Take(limit).ToList();
    #endregion
}