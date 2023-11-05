import string


class Card:
    id: int = 0
    name: string = ""
    attack: int = 0
    defence: int = 0
    currHp: int = 0
    maxHp: int = 0

    def __init__(self, id: int, name: string, attack: int, defense: int):
        self.id = id
        self.name = name
        self.attack = attack
        self.defense = defense

    def __str__(self):
        return f"Card: {self.name} - Attack: {self.attack} - Defense: {self.defense}"

    def Attack(self, card):
        # current formula being sustained damage = attack - own card defence
        card.currHp -= (self.attack-card.defense)
        # might want to expand with crits and dice rolls to make it more fun

