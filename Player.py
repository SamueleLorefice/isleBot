import string

from Card import Card


class Player:
    id: string = ""
    name: string = ""
    activeCard: Card = None


    def __init__(self, id: string, name: string):
        self.id = id
        self.Name = name

    def __str__(self):
        return f"Player: {self.Name} - ID: {self.id}"

    def setActiveCard(self, card: Card):
        self.activeCard = card

    def getActiveCard(self):
        return self.activeCard

