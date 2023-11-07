using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IsleBot;

public class Card {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public User Owner { get; set; }

    public override string ToString() {
        return $"{Name} ATK: {Attack} DEF: {Defense} HP: {CurrentHealth}/{MaxHealth}";
    }

    public Card(string name, int attack, int defense, int maxHealth) {
        Name = name;
        Attack = attack;
        Defense = defense;
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }
}

