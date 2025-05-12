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
            List<Player> playersInRound = new List<Player>();

            bool gameLoop = true;
            int turnCount = 0;
            bool isClockwise = true;

            Console.WriteLine("************************************");
            Console.WriteLine("Welcome to Programming 2 - Final Project - Winter 2025");
            Console.WriteLine("Created by CHRISTEL 6250046 on May 2025");
            Console.WriteLine("************************************\n");

            SetUpGame(game, ref playersInRound);

            while (gameLoop || game.DrawDeck.CardsLeft != 0)
            {
                //Processing turns:
                for(int i = 0; i < playersInRound.Count; i++)
                {
                    Display(game, i+1, playersInRound); //Displaying the gameboard
                    PlayerTurn(game, game.PlayerHands[i], isClockwise, i + 1); //Processing player turn
                    turnCount++;

                    //Checking for winner once the number of total turns becomes 25:
                    if(turnCount >= 25)
                    {
                        gameLoop = CheckForWinner(game.PlayerHands[i]);

                        if (gameLoop == false)
                            break; //Breaking the for loop early if there's a winner
                    }
                        
                }
            }

            Console.WriteLine("RAAAAH");
            

            Console.ReadLine();
        }



        static void SetUpGame(GameState game, ref List<Player> playersInRound)
        {
            const int NUM_PLAYERS = 4;

            Console.WriteLine("WELCOME TO UNO\n");

            for(int i = 0; i < NUM_PLAYERS; i++)
            {
                Player player = new Player(); //Creating a player

                Console.Write($"Name of Player {i+1}: "); 
                player.Name = Console.ReadLine(); //Entering a player
                player.TotalWinnings = 0; //The player starts with a total winning of 0;

                playersInRound.Add(player); //Adding the player to the player list for THIS round
                game.AllPlayers.Add(player); //Adding the player to the TOTAL player list (aka leaderboard)
            }

            //Dealing:
            game.Deal(); 
            Console.WriteLine("\nDealing cards...");
            Thread.Sleep(2000);
        }

        static void PlayerTurn(GameState game, Hand playerHand, bool isClockwise, int playerNum)
        {
            int count = 0; //Useful for keeping track of how many cards the player can play
            int nextPlayerNum = GetNextPlayerNum(playerNum, isClockwise); //Useful for knowing who's the next player (in case of action cards)

            //int previousPlayerNum = GetLastPlayerNum(playerNum); //Will be useful later

            bool canYouPlay = game.CanYouPlay(playerHand.Cards, ref count); //Checking if the player has cards they can play

            if (canYouPlay) 
                Console.Write("Enter the # of your next move (1 = Draw   2 = Play a Card): "); //If yes, they get the choice to play
            else
                Console.Write("Enter the # of your next move (1 = Draw): "); //If no, they are forced to draw

            byte userInput = ChoiceValidation(canYouPlay); //Getting the choice of action 

            if (userInput == 1) //Drawing
            {
                Card card = game.DrawOneCard(); //Drawing a card from the deck
                playerHand.AddCard(card); //Adding the card to the player's hand
                canYouPlay = game.CanYouPlay(card); //Checking if the drawn card is one that can be played

                if (canYouPlay) //If yes, asking the user if they'd like to play it. If the asnwer to that is no, drawn card stays in hand
                {
                    Console.Write("It appears that the card you picked is a ");
                    Console.ForegroundColor = game.GetCardColor(card);
                    Console.WriteLine(card); 

                    Console.ForegroundColor = ConsoleColor.White;

                    Console.Write("Would you like to play it? (y/n): ");
                    string answer = AnswerValidation();

                    if (answer.ToLower() == "y")
                        game.PlayCard(card, playerHand, game.PlayerHands[nextPlayerNum]); //Playing the card if player wants to
                }
            }
            else //Playing
            {
                List<Card> validCards = new List<Card>();
                CheckValidCards(playerHand.Cards, game.CardInMiddle, ref validCards); //Checking how many cards can be played (adding them to a list)

                Console.WriteLine($"\nYou can play {count} cards. Select the # of the one you'd like to play.");

                //Displaying the cards that can be played:
                for (int i = 0; i < validCards.Count; i++) 
                {
                    Console.Write($"{i + 1} = ");
                    Console.ForegroundColor = game.GetCardColor(validCards[i]);
                    Console.Write($"{validCards[i]}   ");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine();

                //Asking the user for which card they'd like to play:
                Console.Write("Choice: ");
                byte cardNumInput = CardNumValidation(count);

                //Playing the card chosen:
                game.PlayCard(validCards[cardNumInput - 1], playerHand, game.PlayerHands[nextPlayerNum]); 

            }

            //Giving a tiny pause for dramatic reasons, idk
            Thread.Sleep(1000);
        }

        static void CheckValidCards(List<Card> hand, Card cardInMiddle, ref List<Card> validCards)
        {
            for(int i = 0;i < hand.Count;i++)
            {
                if (cardInMiddle.Color == hand[i].Color) //If the color matches, it's valid
                    validCards.Add(hand[i]);
                else if (cardInMiddle.Rank == hand[i].Rank) //If the rank matches, it's valid
                    validCards.Add(hand[i]);
                else if (hand[i].Color == Color.Gray) //If the card is a Wild card (+4 or Change Color), it's valid
                    validCards.Add(hand[i]);
            }
        } 

        static void Display(GameState game, int playerNum, List<Player> playersInRound)
        {
            Console.Clear(); //Clearing the console to keep the ouput clean
            game.DisplayGame(playerNum, playersInRound); //Displaying the gameboard
        }

        static int GetNextPlayerNum(int playerNum, bool isClockwise)
        {
            int nextPlayerNum;

            if(isClockwise)
               nextPlayerNum = playerNum % 4;
            else
                nextPlayerNum = (playerNum + 4) % 4;

            return nextPlayerNum;
        }

        static bool CheckForWinner(Hand playerHand)
        {
            if(playerHand.Cards.Count == 0)
                return false;

            return true;
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
