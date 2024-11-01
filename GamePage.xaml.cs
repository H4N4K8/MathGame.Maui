using Windows.Security.Cryptography.Core;

namespace MathGame.Maui;

public partial class GamePage : ContentPage
{
	public string GameType { get; set; }
	int firstNumber = 0;
	int secondNumber = 0;
	int score = 0;
	const int totalQuestions = 2;
	int gamesLeft = totalQuestions;
	public GamePage(string gameType)
	{
		GameType = gameType;
		BindingContext = this;
	}

    private void InitializeComponent()
    {
        throw new NotImplementedException();
    }

    private void CreateNewQuestion()
	{
		var gameOperand = GameType switch
		{
            "Addition" => "+",
            "Subtraction" => "-",
            "Multiplication" => "*",
            "Division" => "/",
			_ => ""

        };

		var random = new Random();
		//short way to if/else:
		firstNumber = GameType != "Division" ? random.Next(1, 9) : random.Next(1, 99);
        secondNumber = GameType != "Division" ? random.Next(1, 9) : random.Next(1, 99);

		if (GameType == "Division")
		{
			while (firstNumber < secondNumber || firstNumber % secondNumber != 0)
			{
				firstNumber = random.Next(1, 99);
                secondNumber = random.Next(1, 99);
            }
		}

        QuestionLabel.Text = $"{firstNumber} {gameOperand} {secondNumber}";
    }

	private void OnAnswerSubmitted(object sender, EventArgs e)
	{
		var answer = Int32.Parse(AnswerEntry.Text);
		var isCorrect = false;
		switch (GameType)
		{
			case "Addition":
				isCorrect = answer == firstNumber + secondNumber;
					break;
            case "Subtraction":
                isCorrect = answer == firstNumber - secondNumber;
                break;
            case "Multiplication":
                isCorrect = answer == firstNumber * secondNumber;
                break;
            case "Division":
                isCorrect = answer == firstNumber / secondNumber;
                break;
        }
		ProcessAnswer(isCorrect);
		gamesLeft--;
		AnswerEntry.Text = "";

		if (gamesLeft > 0)
		{
			CreateNewQuestion();
		}
		else
		{
			GameOver();
		}
		
	}
	private void GameOver()
	{
		QuestionArea.IsVisible = false;
		BackToMenuBtn.IsVisible = true;
		GameOverLabel.Text = $"Game over! you got {score} out of {totalQuestions}";
	}
	private void ProcessAnswer(bool isCorrect)
		{
		if (isCorrect)
			score += 1;

		AnswerLabel.Text = isCorrect ? "Correct!" : "incorrect";
		}
	private void OnBackToMenu(object sender, EventArgs e)
	{
		Navigation.PushAsync(new MainPage());
	}
}