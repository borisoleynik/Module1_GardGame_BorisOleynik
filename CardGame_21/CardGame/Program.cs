using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CardGame
{
    public enum CardRank { six = 6, seven = 7, eight = 8, nine = 9, ten = 10, Jack = 2, Queen = 3, King = 4, Ace = 11 }
    public enum CardColor { Clubs, Diamonds, Hearts, Spades }
    public enum GameState { CountinueGame, EndGame }

    
    class Card
    {
        CardColor _color;
        CardRank _rank;


        public Card()
        {
        }
        public Card(CardColor setColor, CardRank setRank)
        {
            _color = setColor;
            _rank = setRank;
        }
        public CardColor Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public CardRank Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }
    }
    class CardDeck
    {
        Card[] _deck;
        Random r = new Random();

        public CardDeck()
        {
            _deck = CardDeck36_Builder();
        }
        public Card[] CardDeck36_Builder()
        {
            //Generate Card Deck
            Card[] cardDeck36 = new Card[36];

            for (int i = 0; i < cardDeck36.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 2; k <= 11; k++)
                    {
                        if (k == 5) continue;
                        cardDeck36[i] = new Card((CardColor)j,(CardRank)k);
                        i++;
                    }
                }
            }

            //Mixing a Deck of cards
            for (int i = 0; i < cardDeck36.Length; i++)
            {
                int rnd = r.Next(0, 36);
                Card tmp;
                tmp = cardDeck36[i];
                cardDeck36[i] = cardDeck36[rnd];
                cardDeck36[rnd] = tmp;

            }
            return cardDeck36;
        }
        public Card[] DeckOnTable
        {
            get
            {
                return _deck;
            }
        }
} 
    class Player
    {
        string _name;
        int _cardsTaken;
        int _cardsInHandValue;
        int _wins;
        int _loses;
        Card[] _cardInHand = new Card[18];

        public Player(string _name)
        {
            Name = _name;
            _cardsInHandValue = 0;
            _wins = 0;
            _loses = 0;
            _cardInHand = new Card[18];
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int CardsTaken
        {
            get { return _cardsTaken; }
            set { _cardsTaken = value; }
        }

        public int CardsInHandValue
        {
            get { return _cardsInHandValue; }
            set { _cardsInHandValue = value; }
        }
        public int Wins
        {
            get { return _wins; }
            set { _wins = value; }
        }
        public int Loses
        {
            get { return _loses; }
            set { _loses = value; }
        }
        public Card[] CardinHand
        {
            get { return _cardInHand; }
            set { _cardInHand = value; }
        }
    }
    class Game
    {
        Random r = new Random();
        int _round = 1;
        Player player;
        Player computer;
        CardDeck Deck;
        char _firstMove;
        int _cardsTakenFromDeck;
        bool DidCompMadedecision;
        GameState _nextStep = GameState.CountinueGame;

        public GameState CheckNexStep
        {
            get { return _nextStep; }
        }
        public int Round
        {
            get { return _round; }
            set { _round = value; }
        }
        public int CardsTakenFromDeck
        {
            get { return _cardsTakenFromDeck; }
            set { _cardsTakenFromDeck = value; }
        }
        public char FirstMove
        {
            get { return _firstMove; }
            set {_firstMove = value; }
        }


        public void NewRound()
        {
            if (_nextStep == GameState.CountinueGame)
            {
                if (Round == 1)
                {
                    Deck = new CardDeck();
                    Console.WriteLine("Select who receive cards first. (P)- You (C) - Computer");
                    FirstMove = (char)Console.ReadKey(true).KeyChar;
                    CreatePlayers();
                }
                if (FirstMove == 'p')
                {
                    TakeCards(player);
                    PrintResultPerMove(player);
                    CheckMoveResult(player);
                    if (_nextStep == GameState.CountinueGame && ComputerDecides() == true)
                    {
                        TakeCards(computer);
                        PrintResultPerMove(computer);
                        CheckMoveResult(computer);
                    }
                    else Console.WriteLine("\n\nComputed Decided to stop taking cards");
                }
                if (FirstMove == 'c')
                {
                    
                    if (Round == 1)
                    {
                        TakeCards(computer);
                        PrintResultPerMove(computer);
                        CheckMoveResult(computer);
                        if (_nextStep == GameState.CountinueGame)
                        {
                            TakeCards(player);
                            PrintResultPerMove(player);
                            CheckMoveResult(player);
                        }
                        Round += 1;
                    }
                    if (_nextStep == GameState.CountinueGame && ComputerDecides() == true && DidCompMadedecision == false)
                    {
                        TakeCards(computer);
                        PrintResultPerMove(computer);
                        CheckMoveResult(computer);
                    }
                    else Console.WriteLine("\n\nComputed Decided to stop taking cards");
                    if (_nextStep == GameState.CountinueGame)
                    {
                        Console.WriteLine("\n\nDo {0} want(s) to take one more card? (Y) - Yes, (N) - No", player.Name);
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Y:
                                {
                                    Round++;
                                    TakeCards(player);
                                    PrintResultPerMove(player);
                                    CheckMoveResult(player);
                                    NewRound();
                                    if (Round != 1) EndGame();
                                }

                                break;
                            case ConsoleKey.N:
                                {
                                    _nextStep = GameState.EndGame;
                                    RoundPoinsComparison();
                                }
                                break;
                        }
                    }
                }
                if (_nextStep == GameState.CountinueGame)
                {
                    Console.WriteLine("\n\nDo {0} want(s) to take one more card? (Y) - Yes, (N) - No", player.Name);
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Y:
                            {
                                Round++;
                                NewRound();
                                if (Round != 1) EndGame();
                            }

                            break;
                        case ConsoleKey.N:
                            {
                                _nextStep = GameState.EndGame;
                                RoundPoinsComparison();
                            }
                            break;
                    }
                }
            }
            if (Round == 1)EndGame();
        }
        public void CreatePlayers()
        {
            player = new Player("You");
            computer = new Player("Computer");
        }
        public void TakeCards(Player somePlayer)
        {
            if (Round == 1 && _nextStep == GameState.CountinueGame)
            {
                somePlayer.CardinHand[somePlayer.CardsTaken] = Deck.DeckOnTable[CardsTakenFromDeck];
                somePlayer.CardinHand[somePlayer.CardsTaken + 1] = Deck.DeckOnTable[CardsTakenFromDeck + 1];
                somePlayer.CardsInHandValue = (int)somePlayer.CardinHand[somePlayer.CardsTaken].Rank + (int)somePlayer.CardinHand[somePlayer.CardsTaken +1].Rank;
                CardsTakenFromDeck = CardsTakenFromDeck + 2;
                somePlayer.CardsTaken = somePlayer.CardsTaken + 2;
                for (int i = 0; i <= CardsTakenFromDeck - 1; i++)
                {
                    Deck.DeckOnTable[i] = new Card();
                }
            }

            if (Round != 1 && _nextStep == GameState.CountinueGame)
            {
                somePlayer.CardinHand[somePlayer.CardsTaken] = Deck.DeckOnTable[CardsTakenFromDeck];
                somePlayer.CardsInHandValue = somePlayer.CardsInHandValue + (int)somePlayer.CardinHand[somePlayer.CardsTaken].Rank;
                CardsTakenFromDeck = CardsTakenFromDeck + 1;
                somePlayer.CardsTaken = somePlayer.CardsTaken + 1;
                for (int i = 0; i <= CardsTakenFromDeck - 1; i++)
                {
                    Deck.DeckOnTable[i] = new Card();
                }
            }

        }
        public void CheckMoveResult(Player somePlayer)
        {
            if (somePlayer.CardsInHandValue == 21)
            {
                Console.WriteLine("\n{0} callected 21 points", somePlayer.Name);
                somePlayer.Wins += 1;
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\n{0} Won", somePlayer.Name);
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine("\nLet's Play new Game? (Y) - Yes, (N) - No");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Y:
                        Round = 1;
                        _nextStep = GameState.CountinueGame;
                        Console.Clear();
                        NewRound();
                        break;
                    case ConsoleKey.N:
                        {
                            _nextStep = GameState.EndGame;
                            Console.Clear();
                            EndGame();
                        }
                        break;
                }
            }
            if (somePlayer.CardsInHandValue > 21)
            {
                Console.WriteLine("\n{0} have got more than 21 points", somePlayer.Name);
                somePlayer.Loses += 1;
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\n{0} Lose", somePlayer.Name);
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine("Let's Play new Game? (Y) - Yes, (N) - No");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Y:
                        Round = 1;
                        _nextStep = GameState.CountinueGame;
                        Console.Clear();
                        NewRound();
                        break;
                    case ConsoleKey.N:
                        {
                            _nextStep = GameState.EndGame;
                            Console.Clear();
                        }
                        break;
                }
            }
            else _nextStep = GameState.CountinueGame;
        }
        public void RoundPoinsComparison()
        {
            if (player.CardsInHandValue > computer.CardsInHandValue)
            {
                player.Wins += 1;
                computer.Loses += 1;
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\n{0} Won", player.Name);
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine("\nLet's Play new Game? (Y) - Yes, (N) - No");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Y:
                        Round = 1;
                        _nextStep = GameState.CountinueGame;
                        Console.Clear();
                        NewRound();
                        break;
                    case ConsoleKey.N:
                        {
                            _nextStep = GameState.EndGame;
                            Console.Clear();
                        }
                        break;
                }
            }
            if (player.CardsInHandValue < computer.CardsInHandValue)
            {
                player.Loses += 1;
                computer.Wins += 1;
                System.Threading.Thread.Sleep(1500);
                Console.WriteLine("\n{0} Lose", player.Name);
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine("Let's Play new Game? (Y) - Yes, (N) - No");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Y:
                        Round = 1;
                        _nextStep = GameState.CountinueGame;
                        Console.Clear();
                        NewRound();
                        break;
                    case ConsoleKey.N:
                        {
                            _nextStep = GameState.EndGame;
                            Console.Clear();
                        }
                        break;
                }
            }
        }
        public void EndGame()
        {
            _nextStep = GameState.EndGame;
            //SavePlayerResults
            string fileReader = File.ReadAllText(@"Results\PlayerWins.txt");
            int scoreToFile = player.Wins + int.Parse(fileReader);
            File.WriteAllText(@"Results\PlayerWins.txt", scoreToFile.ToString());
            fileReader = File.ReadAllText(@"Results\PlayerLoses.txt");
            scoreToFile = player.Loses + computer.Wins + int.Parse(fileReader);
            File.WriteAllText(@"Results\PlayerLoses.txt", scoreToFile.ToString());

            //SaveComputerResults
            fileReader = File.ReadAllText(@"Results\ComputerWins.txt");
            scoreToFile = player.Loses + computer.Wins + int.Parse(fileReader);
            File.WriteAllText(@"Results\ComputerWins.txt", scoreToFile.ToString());
            fileReader = File.ReadAllText(@"Results\ComputerLoses.txt");
            scoreToFile = player.Wins + int.Parse(fileReader);
            File.WriteAllText(@"Results\ComputerLoses.txt", scoreToFile.ToString());

            //Print all Results
            for (int i = 0; i < 80; i++)
            {
                if (i == 40)
                {
                    Console.WriteLine("\n{0,2}{1,15}{2,15}", "Player", "Wins", "Loses");
                }
                Console.Write("-");
            }
            Console.WriteLine("\n{0,2}{1,16}{2,14}", player.Name, File.ReadAllText(@"Results\PlayerWins.txt"), File.ReadAllText(@"Results\PlayerLoses.txt"));
            Console.WriteLine("\n{0,2}{1,11}{2,14}", computer.Name, File.ReadAllText(@"Results\ComputerWins.txt"), File.ReadAllText(@"Results\ComputerLoses.txt"));
        }
        public void PrintResultPerMove(Player somePlayer)
        {
            Console.Write("\n\n{0} have got {1} Points - {2} - Cards Taken",somePlayer.Name,somePlayer.CardsInHandValue, somePlayer.CardsTaken);
            for (int i = 0; i < somePlayer.CardsTaken ; i++)
            {
                Console.Write("||{0}||",somePlayer.CardinHand[i].Rank);
            }
        }
        public bool ComputerDecides()
        {
            if (Round != 1 && _nextStep == GameState.CountinueGame)
            {
                if (computer.CardsInHandValue < 21 && computer.CardsInHandValue > 14)
                {
                    if (r.Next(5) == 0) return true;
                    else
                    {
                        DidCompMadedecision = true;
                        return false;
                            };
                }
                if (computer.CardsInHandValue < 14 && computer.CardsInHandValue > 10)
                {
                    if (r.Next(4) == 0) return true;
                    {
                          DidCompMadedecision = true;
                        return false;
                    };
                }
                if (computer.CardsInHandValue < 10 && computer.CardsInHandValue > 5)
                {
                    if (r.Next(2) == 0) return true;
                    {
                        DidCompMadedecision = true;
                        return false;
                    };
                }
                if (computer.CardsInHandValue < 5) return true;
            }

             return true;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Game NewGame = new Game();
            NewGame.NewRound(); 


        }
    }
}
