namespace IsleBot;

public class User
{
    public int Id { get; private set; }
    public string UserName { get; set; }
    public List<Card> Cards { get; set; }

    public User(int id, string userName) {
        Id = id;
        UserName = userName;
        Cards = new List<Card>();
    }
}