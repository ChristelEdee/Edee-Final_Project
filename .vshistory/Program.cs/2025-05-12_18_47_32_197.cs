using System.Reflection;
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

            bool mainLoop = true;

            Console.WriteLine("************************************");
            Console.WriteLine("Welcome to Programming 2 - Final Project - Winter 2025");
            Console.WriteLine("Created by CHRISTEL 6250046 on May 2025");
            Console.WriteLine("************************************\n");

            SetUpGame(game, ref playersInRound);
            PlayGame(game, playersInRound);

            //while (game.DrawDeck.CardsLeft != 0)
            //{
            //    //Processing turns:
            //    for(int i = 0; i < playersInRound.Count; i++)
            //    {
            //        Display(game, i+1, playersInRound); //Displaying the gameboard
            //        PlayerTurn(game, game.PlayerHands[i], isClockwise, i + 1); //Processing player turn
            //        turnCount++;

            //        //Checking for winner once the number of total turns becomes 25:
            //        if(turnCount >= 25)
            //        {
            //            isThereWinner = CheckForWinner(game.PlayerHands[i]);

            //            if (isThereWinner == true)
            //            {
            //                winner = playersInRound[i].Name;
            //                break; //Breaking the for loop early if there's a winner
            //            }
                            
            //        }    
            //    }

            //    if (isThereWinner == true)
            //        break; //Breaking the while loop if there's a winner
            //}

            //GameOver(game, winner, playersInRound);

            byte menuChoice;

            while (mainLoop)
            {
                Console.Clear();

                Console.WriteLine("What would you like to do next? Enter the # of your choice.\n");
                Console.WriteLine("1. See Leaderboard of Total Winnings");
                Console.WriteLine("2. Play a new game");
                Console.WriteLine("3. Quit the program\n");
                Console.Write("Choice: ");

                menuChoice = MenuChoiceValidation();

                switch (menuChoice)
                {
                    case 1:
                        DisplayLeaderboard(game);
                    break;

                    case 2:
                        GameState newGame = new GameState(game.AllPlayers); //Creating a new game object (this time with the one paramter constructor to keep the leaderboard)
                        List<Player> newPlayersList = new List<Player>(); //Creating a new list of players for the round

                        SetUpGame(newGame, ref newPlayersList); 
                        PlayGame(newGame, newPlayersList);
                    break;

                    case 3:
                        mainLoop = false;
                    break;
                }
            }

            Console.WriteLine("RAAAAAAAAAAAH");
            Console.ReadLine();
        }


        //Important methods:
        static void SetUpGame(GameState game, ref List<Player> playersInRound)
        {
            const int NUM_PLAYERS = 4;

            Console.WriteLine("\nWELCOME TO UNO\n");

            for(int i = 0; i < NUM_PLAYERS; i++)
            {
                Player player = new Player(); //Creating a player

                Console.Write($"Name of Player {i+1}: "); 
                player.Name = Console.ReadLine(); //Entering a player
                player.TotalWinnings = 0; //The player starts with a total winning of 0;
                playersInRound.Add(player); //Adding the player to the player list for THIS round

                //Checking if the player is already in the ALL PLAYERS list (same name)
                for(int j =0; j < game.AllPlayers.Count; j++)
                {
                    //If the same player is playing again, their total wins get transfered
                    if(player.Name == game.AllPlayers[j].Name)
                    {
                        player.TotalWinnings = game.AllPlayers[j].TotalWinnings;
                        game.AllPlayers.RemoveAt(j); //Removing temporarily (Gets added back after this loop ends)
                    }
                }

                game.AllPlayers.Add(player); //Adding the player to the TOTAL player list (aka leaderboard)
            }

            //Dealing:
            game.Deal(); 
            Console.WriteLine("\nDealing cards...");
            Thread.Sleep(2000);
        }

        static void PlayGame(GameState game, List<Player> playersInRound)
        {
            int turnCount = 0;
            bool isClockwise = true;
            bool isThereWinner = true;
            string? winner = null;

            while (game.DrawDeck.CardsLeft != 0)
            {
                //Processing turns:
                for (int i = 0; i < playersInRound.Count; i++)
                {
                    Display(game, i + 1, playersInRound); //Displaying the gameboard
                    PlayerTurn(game, game.PlayerHands[i], isClockwise, i + 1); //Processing player turn
                    turnCount++;

                    //Checking for winner once the number of total turns becomes 25:
                    if (turnCount >= 25)
                    {
                        isThereWinner = CheckForWinner(game.PlayerHands[i]);

                        if (isThereWinner == true)
                        {
                            winner = playersInRound[i].Name;
                            break; //Breaking the for loop early if there's a winner
                        }

                    }
                }

                if (isThereWinner == true)
                    break; //Breaking the while loop if there's a winner
            }

            GameOver(game, winner, playersInRound);
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
                return true;

            return false;
        }

        static void SortPlayersAndScores(ref List<int> handValueList, ref List<Player> playersInRound)
        {
            int max_index;

            for (int i = 0; i < handValueList.Count; i++)
            {
                max_index = i;

                for (int j = i; j < handValueList.Count; j++)
                {
                    if (handValueList[j] > handValueList[max_index])
                        max_index = j;
                }

                if (max_index != i)
                {
                    int temp1 = handValueList[i];
                    handValueList[i] = handValueList[max_index];
                    handValueList[max_index] = temp1;

                    Player temp2 = playersInRound[i];
                    playersInRound[i] = playersInRound[max_index];
                    playersInRound[max_index] = temp2;
                }
            }
        }



        static void GameOver(GameState game, string? winner, List<Player> playersInRound)
        {
            List<int> handValuesList = new List<int>();
        
            //Displaying Game status (Winner or forced Game Over):
            if (winner != null)
                Console.WriteLine($"\n{winner} WINS!!!\n");
            else
                Console.WriteLine("\nNo more cards in the deck. GAME OVER!\n");

            Thread.Sleep(2000); //Pause of 3 seconds for dramatic effect


            //Getting hand values of each player and adding it to a list:
            for(int i = 0; i < playersInRound.Count; i++)
            {
                int playerHandValue = game.CalculateFinalHandValue(game.PlayerHands[i].Cards);
                handValuesList.Add(playerHandValue);
            }

            //Displaying the game over screen
            game.DisplayGameOver(playersInRound, handValuesList);
            

            //Sorting the lists in order (Hand Value closest to 0 - Hand Value furthest to 0)
            SortPlayersAndScores(ref handValuesList, ref playersInRound);

            Console.WriteLine("\nFINAL SCORES\n");
            for (int i = 0; i < handValuesList.Count; i++)
            {
                if(i == 0 || i == 1)
                    Console.ForegroundColor = ConsoleColor.Yellow; //1st and 2nd ranked player gets to be highlighted
                else
                    Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine($"{i + 1}. {playersInRound[i].Name} -> {handValuesList[i]}");
            }

            

            //Processing the total wins (Notes: Only 1st and 2nd ranked players get some wins. 1st place gets 3/4 of the combined handValues of 3rd and 4th place * 2
            //while 2nd place gets 1/4. This scoring algorithm is heavily based on the UNO game app)
            int winsFirstPlace = -(((handValuesList[2] + handValuesList[3]) * 2) * 3) /4;
            int winsSecondPlace = -((handValuesList[2] + handValuesList[3]) * 2) / 4;

            Console.WriteLine("\n\nWINS\n");
            for(int i = 0; i < playersInRound.Count; i++)
            {
                Console.Write($"{playersInRound[i].Name} -> ");

                if(i == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"+{winsFirstPlace}");
                }
                else if(i == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"+{winsSecondPlace}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"+0");
                }

                Console.ForegroundColor= ConsoleColor.White;
            }


            //Updating the winning players' total wins
            for (int i = 0; i < game.AllPlayers.Count; i++)
            {
                if (game.AllPlayers[i].Name == playersInRound[0].Name)
                    game.AllPlayers[i] = new Player { Name = playersInRound[0].Name, TotalWinnings = game.AllPlayers[i].TotalWinnings + winsFirstPlace };

                if (game.AllPlayers[i].Name == playersInRound[1].Name)
                    game.AllPlayers[i] = new Player { Name = playersInRound[1].Name, TotalWinnings = game.AllPlayers[i].TotalWinnings + winsSecondPlace };
            }

            Console.WriteLine("\nPress any key to exit this screen.");
            Console.ReadLine();
        }

        static void DisplayLeaderboard(GameState game)
        {
            Console.Clear();

            Console.WriteLine("LEADERBOARD\n\n");

            Console.WriteLine("PLAYERS\t\t  TOTAL WINS");
            Console.WriteLine("------------------------------");
            for (int i = 0; i < game.AllPlayers.Count; i++)
            {
                Console.WriteLine($"{i+1}. {game.AllPlayers[i].Name}\t\t{game.AllPlayers[i].TotalWinnings}");
                Console.WriteLine("------------------------------");
            }

            Console.WriteLine("\nPress any key to exit this screen.");
            Console.ReadLine();
        }
        


        //Validation methods:
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
        static byte MenuChoiceValidation()
        {
            const byte MAX_MENU_CHOICE = 3; //Last choice for main menu
            const byte MIN_CHOICE_NUM = 1;

            byte userInput;
            bool successfulConversion;

            successfulConversion = byte.TryParse(Console.ReadLine(), out userInput);

            while (successfulConversion == false || userInput > MAX_MENU_CHOICE || userInput < MIN_CHOICE_NUM)
            {
                Console.Write($"What you entered was not a valid choice. Try again: ");
                successfulConversion = byte.TryParse(Console.ReadLine(), out userInput);
            }

            return userInput;
        }
    }
}
