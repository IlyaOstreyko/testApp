using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testApp.Interfaces
{
    public interface IQuestionRepository<T> where T : class
    {
        List<T> GetAll();
        List<T> GetQuestionsInTheme(string nameTheme);
        List<T> GetRndQuestionsInTheme(string nameTheme, int number);
        List<string> GetThemes();
        List<string> GetThemesInSpeciality(string nameSpeciality);
        int GetNumberQuestionInTheme(string nameTheme);
        bool CheckQuestionsOnNameQuestion(string nameQuestion);
        bool CheckQuestions(T question);
        int? GetIdQuestions(T question);
        T GetQuestionsOnNameQuestion(string nameQuestion);
        T Get(int id);
        void Create(T item);
        int Create(IEnumerable<T> items);
        void Update(T item);
        void Delete(int id);
    }
}
