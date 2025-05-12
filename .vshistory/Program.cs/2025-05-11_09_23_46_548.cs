using System.Text;

namespace Edee_Final_Project
{
    //Important enums:

    public enum Color
    {
        Red,
        Green,
        Blue,
        Yellow,
        Gray
    }

    public enum Rank
    {
        Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, 
        Reverse, Skip, PlusTwo, ChangeColor, PlusFour
    }

    public struct Player
    {
        public string? Name;
        public int TotalWinnings;
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; //Have to put this to get the rank symbols  

            GameState game = new GameState();
            int player1Num = 1, player2Num = 2, player3Num = 3, player4Num = 4;
            bool gameLoop = true;
            //bool isClockwise = true;

            Console.WriteLine("************************************");
            Console.WriteLine("Welcome to Programming 2 - Final Project - Winter 2025\n");
            Console.WriteLine("Created by CHRISTEL 6250046 on May 2025");
            Console.WriteLine("************************************");

            SetUpGame(game);

            while (gameLoop || game.DrawDeck.CardsLeft != 0)
            {
                Display(game, player1Num);
                PlayerTurn(game, game.PlayerHands[0], player1Num);

                Display(game, player2Num);
                PlayerTurn(game, game.PlayerHands[1], player2Num);

                Display(game, player3Num);
                PlayerTurn(game, game.PlayerHands[2], player3Num);

                Display(game, player4Num);
                PlayerTurn(game, game.PlayerHands[3], player4Num);
            }
            

            Console.ReadLine();
        }

        static void SetUpGame(GameState game)
        {
            game.Deal();

            Console.WriteLine("WELCOME TO UNO\n");

            for(int i = 0; i < 4; i++)
            {
                Player player = new Player();

                Console.Write($"Name of Player {i}: "); 
                player.Name = Console.ReadLine(); //Entering a player

                game.Players.Add(player); //Adding that player to the player list
            }
            

        }

        static void PlayerTurn(GameState game, Hand playerHand, int playerNum)
        {
            int count = 0;
            int nextPlayerNum = GetNextPlayerNum(playerNum); //Will be useful later
            //int previousPlayerNum = GetLastPlayerNum(playerNum); //Will be useful later

            bool canYouPlay = CanYouPlay(playerHand.Cards, game.CardInMiddle, ref count);

            if (canYouPlay)
                Console.Write("Enter the # of your next move (1 = Draw   2 = Play a Card): ");
            else
                Console.Write("Enter the # of your next move (1 = Draw): ");

            byte userInput = ChoiceValidation(canYouPlay);

            if (userInput == 1)
            {
                Card card = game.DrawOneCard();
                playerHand.AddCard(card);
                canYouPlay = CanYouPlay(card, game.CardInMiddle);

                if (canYouPlay)
                {
                    Console.Write("It appears that the card you picked is a ");
                    Console.ForegroundColor = game.GetCardColor(card);
                    Console.WriteLine(card);

                    Console.ForegroundColor = ConsoleColor.White;

                    Console.Write("Would you like to play it? (y/n): ");
                    string answer = AnswerValidation();

                    if (answer.ToLower() == "y")
                    {
                        game.PlayCard(card, playerHand, game.PlayerHands[nextPlayerNum]);
                    }

                }
            }
            else
            {
                List<Card> validCards = new List<Card>();
                CheckValidCards(playerHand.Cards, game.CardInMiddle, ref validCards);

                Console.WriteLine($"\nYou can play {count} cards. Select the # of the one you'd like to play.");

                for (int i = 0; i < validCards.Count; i++)
                {
                    Console.Write($"{i + 1} = ");
                    Console.ForegroundColor = game.GetCardColor(validCards[i]);
                    Console.Write($"{validCards[i]}   ");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine();

                Console.Write("Choice: ");
                byte cardNumInput = CardNumValidation(count);

                game.PlayCard(validCards[cardNumInput - 1], playerHand, game.PlayerHands[nextPlayerNum]);

            }

            Thread.Sleep(1000);
        }

        static bool CanYouPlay(List<Card> hand, Card cardInMiddle, ref int count)
        {

            for(int i = 0; i < hand.Count; i++)
            {
                if (cardInMiddle.Color == hand[i].Color)
                    count++;
                else if (cardInMiddle.Rank == hand[i].Rank)
                    count++;
                else if (hand[i].Color == Color.Gray)
                    count++;
            }

            if(count > 0)
                return true;

            return false;
        } 

        static bool CanYouPlay(Card cardDrawn, Card cardInMiddle)
        {
            if (cardInMiddle.Color == cardDrawn.Color)
                return true;
            else if (cardInMiddle.Rank == cardDrawn.Rank)
                return true;
            else if (cardDrawn.Color == Color.Gray)
                return true;

            return false;
        } 

        static void CheckValidCards(List<Card> hand, Card cardInMiddle, ref List<Card> validCards)
        {
            for(int i = 0;i < hand.Count;i++)
            {
                if (cardInMiddle.Color == hand[i].Color)
                    validCards.Add(hand[i]);
                else if (cardInMiddle.Rank == hand[i].Rank)
                    validCards.Add(hand[i]);
                else if (hand[i].Color == Color.Gray)
                    validCards.Add(hand[i]);
            }
        } 

        static int CalculateFinalScore(List<Card> hand)
        {
            Rank[] numberedRanks = { Rank.Zero, Rank.One, Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six,
                            Rank.Seven, Rank.Eight, Rank.Nine};
            int total = 0;

            for(int i = 0; i < hand.Count; i++)
            {
                if (hand[i].Rank == Rank.Skip || hand[i].Rank == Rank.Reverse || hand[i].Rank == Rank.PlusTwo)
                    total -= 20;
                if (hand[i].Rank == Rank.PlusFour || hand[i].Rank == Rank.ChangeColor)
                    total -= 50;
                if (numberedRanks.Contains(hand[i].Rank))
                    total -= (int)hand[i].Rank;
            }

            return total;
        } 


        static void Display(GameState game, int playerNum)
        {
            Console.Clear();
            game.DisplayGame(playerNum);
        }

        static int GetNextPlayerNum(int playerNum)
        {
            return playerNum % 4;
        }

        static int GetLastPlayerNum(int playerNum)
        {
            return (playerNum + 4) % 4;
        }

        //static void Reverse(GameState game, bool isClockwise, int playerNum, bool gameLoop)
        //{
        //    if(playerNum == 1)
        //    {
        //        while(gameLoop == true || isClockwise == false)
        //        {
        //            Display(game, playerNum+3);
        //            PlayerTurn(game, game.PlayerHands[3], playerNum+3, ref isClockwise);

        //            Display(game, playerNum+2);
        //            PlayerTurn(game, game.PlayerHands[2], playerNum+2, ref isClockwise);

        //            Display(game, playerNum+1);
        //            PlayerTurn(game, game.PlayerHands[1], playerNum+1, ref isClockwise);

        //            Display(game, playerNum);
        //            PlayerTurn(game, game.PlayerHands[0], playerNum, ref isClockwise);
        //        }
        //    }
        //    if (playerNum == 2)
        //    {
        //        while (gameLoop == true || isClockwise == false)
        //        {
        //            Display(game, playerNum-1);
        //            PlayerTurn(game, game.PlayerHands[0], playerNum-1, ref isClockwise);

        //            Display(game, playerNum + 2);
        //            PlayerTurn(game, game.PlayerHands[3], playerNum + 2, ref isClockwise);

        //            Display(game, playerNum + 1);
        //            PlayerTurn(game, game.PlayerHands[2], playerNum + 1, ref isClockwise);

        //            Display(game, playerNum);
        //            PlayerTurn(game, game.PlayerHands[1], playerNum, ref isClockwise);
        //        }
        //    }
        //    if (playerNum == 3)
        //    {
        //        while (gameLoop == true || isClockwise == false)
        //        {
        //            Display(game, playerNum - 1);
        //            PlayerTurn(game, game.PlayerHands[1], playerNum-1, ref isClockwise);

        //            Display(game, playerNum - 2);
        //            PlayerTurn(game, game.PlayerHands[0], playerNum-2, ref isClockwise);

        //            Display(game, playerNum + 1);
        //            PlayerTurn(game, game.PlayerHands[3], playerNum + 1, ref isClockwise);

        //            Display(game, playerNum);
        //            PlayerTurn(game, game.PlayerHands[2], playerNum, ref isClockwise);
        //        }
        //    }
        //    if (playerNum == 4)
        //    {
        //        while (gameLoop == true || isClockwise == false)
        //        {
        //            Display(game, playerNum-1);
        //            PlayerTurn(game, game.PlayerHands[2], playerNum-1, ref isClockwise);

        //            Display(game, playerNum-2);
        //            PlayerTurn(game, game.PlayerHands[1], playerNum-2, ref isClockwise);

        //            Display(game, playerNum-3);
        //            PlayerTurn(game, game.PlayerHands[0], playerNum-3, ref isClockwise);

        //            Display(game, playerNum);
        //            PlayerTurn(game, game.PlayerHands[3], playerNum, ref isClockwise);
        //        }
        //    }
        //}


        static byte ChoiceValidation(bool canYouPlay)
        {
            const byte MIN_NUM = 1;
            byte max_num = 2;

            if(canYouPlay == false)
                max_num = MIN_NUM;

            byte userInput;
            bool successfulConversion;

            successfulConversion = byte.TryParse(Console.ReadLine(), out userInput);

            while (successfulConversion == false || userInput < MIN_NUM || userInput > max_num)
            {
                Console.Write($"What you entered wasn't valid. Try again: ");
                successfulConversion = byte.TryParse(Console.ReadLine(), out userInput);
            }

            return userInput;
        }
        static byte CardNumValidation(int count)
        {
            const int MIN_NUM = 1;

            byte userInput;
            bool successfulConversion;

            successfulConversion = byte.TryParse(Console.ReadLine(), out userInput);

            while (successfulConversion == false || userInput < MIN_NUM || userInput > count)
            {
                Console.Write($"What you entered wasn't valid. Try again: ");
                successfulConversion = byte.TryParse(Console.ReadLine(), out userInput);
            }

            return userInput;
        }
        static string AnswerValidation()
        {
            string? userInput = Console.ReadLine();

            while(userInput.ToLower() != "y" && userInput.ToLower() != "n")
            {
                Console.Write($"What you entered wasn't valid. Try again: ");
                userInput = Console.ReadLine();
            }

            return userInput;
        }
    }
}
