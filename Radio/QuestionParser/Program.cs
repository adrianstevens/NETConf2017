using eXam;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace QuestionParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string basicText, advancedText;

            var assembly = typeof(App).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("eXam.Data.QuestionsBasic.txt"))
                basicText = new StreamReader(stream).ReadToEnd();

            using (var stream = assembly.GetManifestResourceStream("eXam.Data.QuestionsAdvanced.txt"))
                advancedText = new StreamReader(stream).ReadToEnd();

            var basicLessons = OpenQuestions(basicText);
            var advLessons = OpenQuestions(advancedText);

            var basicJson = JsonConvert.SerializeObject(basicLessons);
            var advJson = JsonConvert.SerializeObject(advLessons);

            File.WriteAllText(@"BasicQuestions.json", basicJson);
            File.WriteAllText(@"AdvancedQuestions.json", advJson);

            return;
            
        }

        static List<Lesson> OpenQuestions(string questionsText)
        {
            if (string.IsNullOrWhiteSpace(questionsText))
                return null;

            var lessons = new List<Lesson>();

            var separator = new string[] { "\r\n", "\n" };//\r\n for Windows

            var lines = questionsText.Split(separator, new StringSplitOptions());

            int count = 0;

            Lesson lesson = null;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                //time to parse
                if (string.IsNullOrWhiteSpace(line) || line[0] == '\'' || line[0] == '^' || line[0] == '\r')
                    continue;

                if (line[0] == '{') //new lession
                {
                    lesson = CreateLesson(line);
                    lessons.Add(lesson);
                }
                else //process question, answers and explanation
                {
                    if (line[0] != 'B' && line[0] != 'A') //think this is just basic/advanced indicator
                        continue;

                    var index = line.IndexOf('-');
                    var lessonIndex = int.Parse(line.Substring(index + 1, 1));
                    index = line.IndexOf('-', index + 1);
                    var sectionIndex = int.Parse(line.Substring(index + 1, 1));//can be 2 digits 
                    index = line.IndexOf('-', index + 1);
                    var questionIndex = int.Parse(line.Substring(index + 1, 1));

                    index = line.IndexOf('(', index);  //open paren 
                    var answerText = line.Substring(index + 1, 1);
                    var answerIndex = answerText[0] - 'A';

                    index = line.IndexOf(' ', index);  //space preceeding 
                    var questionText = line.Substring(index + 1);

                    var question = new QuizQuestion()
                    {
                        Lesson = lessonIndex,
                        Section = sectionIndex,
                        Index = questionIndex,
                        CorrectAnswer = answerIndex,
                        Question = questionText,
                    };

                    //now look for answers
                    while(lines[++i][0] != '>') //move the index of the for loop
                    {
                        var ans = lines[i].Substring(2); //appears to be a tab ... try this
                        question.Answers.Add(ans);
                    }

                    //check for keyword
                    int exIndex = 2;

					if (lines[i].Contains("key words"))
					{
						question.Keyword = lines[i].Substring(14, lines[i].IndexOf('.') - 13);
						exIndex = lines[i].IndexOf('.') + 2;
					}

                    else if(lines[i].Contains("key word"))
                    {
                        question.Keyword = lines[i].Substring(13, lines[i].IndexOf('.') - 13);
                        exIndex = lines[i].IndexOf('.') + 2;//we've corrected
                    }

                    //and finally, the explanation
                    question.Explanation = lines[i].Substring(exIndex);

                    lesson.Questions.Add(question);

                    Debug.WriteLine($"Added question {sectionIndex}:{questionIndex} with {question.Answers.Count} answers");
                }

                //  Debug.WriteLine(lines[i]);

                //  count++;
            }

            Debug.WriteLine($"Count: {count}");

            return lessons;
        }

        static Lesson CreateLesson(string lessonText)
        {
            var id = int.Parse(lessonText.Substring(2, 2));
            var title = lessonText.Substring(6);

            var lesson = new Lesson()
            {
                ID = id,
                Title = title
            };

            Debug.WriteLine($"Created lesson {id}: {title}");

            return lesson;
        }
    }
}
