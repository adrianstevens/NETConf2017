using System.Collections.Generic;

namespace Radio.ViewModels
{
    public class LessonsPageViewModel
    {
        public List<Lesson> Lessons { get; private set; }

        public LessonsPageViewModel(List<Lesson> lessons)
        {
            Lessons = lessons;
        }
    }
}