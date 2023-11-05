import os
import random
import dotenv
import discord

from Card import Card
from Player import Player


dotenv.load_dotenv()
token = os.getenv("TOKEN")
#intents: discord.Intents = discord.Intents.default()
#intents.guilds = True
#intents.members = True
#intents.messages = True
#intents.message_content = True

bot = discord.Bot()
rng = random.Random()
players = []

@bot.event
async def on_ready():
    print("Bot loaded")

@bot.event
async def on_message(message):
    # Prevents bot from responding to itself
    if message.author == bot.user:
        return

    if message.content.startswith("!hello"):
        await message.channel.send("Hello!")
        return

@bot.slash_command(name="get-card", description="Get a random card")
async def getCard(ctx: discord.ApplicationContext):
    sender = ctx.author
    player = Player(sender.id, sender.name)
    player.activeCard = Card(1, f"TestCard {rng.randint(0, 1999)}", rng.randint(2,20), rng.randint(0,5))

    for p in players:
        if p.id == player.id:
            p.activeCard = player.activeCard
            await ctx.respond(f"You already have a card, swapping it...\nYour new card is: \n   {p.activeCard}")
            return
    players.append(player)
    await ctx.respond(f"Your card is: \n   {player.activeCard}")
    return

@bot.slash_command(name="attack", description="Attack another player")
async def attackPlayer(ctx: discord.ApplicationContext, target: discord.Member):
    attacker: Player = None
    defender: Player = None

    if target == ctx.author:
        await ctx.respond("You can't attack yourself!")
        return

    if target.bot:
        await ctx.respond("You can't attack a bot!")
        return

    for p in players:
        if p.id == ctx.author.id:
            attacker = p
            if p.activeCard == None:
                await ctx.respond("You don't have a card! Generate one first!")
                return
            break

    for p in players:
        if p.id == target.id:
            defender = p
            if p.activeCard == None:
                await ctx.respond("That player doesn't have a card! Tell him to generate one first!")
                return
            break

    if attacker is None or defender is None:
        await ctx.respond(f"can't find a match... attacker: {attacker} defender: {defender}")
        return

    attacker.activeCard.Attack(defender.activeCard)

    if defender.activeCard.currHp <= 0:
        await ctx.respond(f"{attacker.Name} attacked {defender.Name} and killed his card!")
        players.remove(defender)
        return
    else:
        await ctx.respond(f"{attacker.Name} attacked {defender.Name}! {defender.Name}'s card has {defender.activeCard.currHp} hp left!")
        await ctx.send(f"Battle Status:\n{attacker.Name}'s card: {attacker.activeCard.currHp} hp left\n{defender.Name}'s card: {defender.activeCard.currHp} hp left")
        return

bot.run(token)