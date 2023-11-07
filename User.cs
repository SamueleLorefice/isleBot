using System.Runtime.Serialization;

namespace IsleBot;

public class User {
    [DataMember(IsRequired = true)]
    public ulong Id { get; private set; }
    public string UserName { get; set; }
    
    public List<Card> Cards { get; set; }
    
    //public List<Match> Matches { get; set; }

    public User(ulong id, string userName) {
        Id = id;
        UserName = userName;
        Cards = new List<Card>();
    }

    public override string ToString() {
        var profileStr = $"**{UserName}**\n"+
                            $"**Total Cards:** {Cards.Count}\n" +
                            $"\n";

        foreach (var card in Cards.Take(5)) {
            profileStr += card.ToFormattedString() + "\n";
        }
        
        return profileStr;
    }
}