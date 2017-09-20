using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;

namespace Radio
{
	public enum GameMode
	{
		Basic,
		BasicPractice,
		Advanced,
		AdvancedPractice, 
		count
	}

	public class GameManager
	{
		Game currentGame;

		public Game LoadGame(GameMode mode)
		{
			switch (mode)
			{
				case GameMode.Basic:
					return currentGame = LoadGame("BasicQuestions", GameMode.Basic);
				case GameMode.BasicPractice:
					return currentGame = LoadGame("BasicQuestions", GameMode.BasicPractice);
					case GameMode.Advanced:
					return currentGame = LoadGame("AdvancedQuestions", GameMode.Advanced);
				case GameMode.AdvancedPractice:
					return currentGame = LoadGame("AdvancedQuestions", GameMode.AdvancedPractice);
			}
			return null;
		}

		public Game GetCurrentGame()
		{
			return currentGame;
		}

		static Game LoadGame(string questionFile, GameMode mode)
		{
			var questionsText = String.Empty;

			if (string.IsNullOrWhiteSpace(questionsText))
			{
				var assembly = typeof(App).GetTypeInfo().Assembly;
				using (var stream = assembly.GetManifestResourceStream("Radio.Data." + questionFile + ".json"))
				{
					questionsText = new StreamReader(stream).ReadToEnd();
				}
			}

			var lessons = JsonConvert.DeserializeObject<List<Lesson>>(questionsText);

			if (mode == GameMode.AdvancedPractice || mode == GameMode.BasicPractice)
				return new Game(GetPracticeExam(lessons), 0, mode);

			return new Game(lessons);
		}

		static List<Lesson> GetPracticeExam(List<Lesson> lessons)
		{
			int count = 80;

			var rand = new Random();

			var exam = new Lesson();

			exam.Title = "Practice Exam";

			while (exam.NumberOfQuestions < count)
			{
				int lesson = rand.Next() % lessons.Count;

				int question = rand.Next() % lessons[lesson].NumberOfQuestions;

				if (exam.Questions.Contains(lessons[lesson].Questions[question]))
					continue;

				exam.Questions.Add(lessons[lesson].Questions[question]);
			}

			return new List<Lesson> { exam }; 
		}
	}
}