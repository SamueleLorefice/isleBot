using System.Runtime.Serialization;

namespace IsleBot;

public class User {
    [DataMember(IsRequired = true)]
    public int Id { get; private set; }
    public string UserName { get; set; }
    
    public List<Card> Cards { get; set; }
    
    //public List<Match> Matches { get; set; }

    public User(int id, string userName) {
        Id = id;
        UserName = userName;
        Cards = new List<Card>();
    }
}