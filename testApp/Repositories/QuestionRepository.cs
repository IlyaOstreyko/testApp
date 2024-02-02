using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testApp.Context;
using testApp.DataModels;
using testApp.Interfaces;
using testApp.Models;

namespace testApp.Repositories
{
    public class QuestionRepository : IQuestionRepository<TestQuestion>
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IMapper _mapper;

        public QuestionRepository()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            _applicationContext = new ApplicationContext();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TestQuestion, TestQuestionDataModel>().ReverseMap()
            .ForMember("FullImageQuestion", opt => opt.MapFrom(c => currentDirectory + "\\" + c.ImageQuestion)));
            _mapper = new Mapper(config);
        }

        public void Create(TestQuestion itemQuestion)
        {
            TestQuestionDataModel testQuestionDM = _mapper.Map<TestQuestionDataModel>(itemQuestion);
            _applicationContext.TestQuestionDataModels.Add(testQuestionDM);
            _applicationContext.SaveChanges();
            //int returnValue = (int)this.ad.InsertCommand.ExecuteScalar();
        }

        public int Create(IEnumerable<TestQuestion> questions)
        {
            int countQuestions = 0;
            try
            {
                IEnumerable<TestQuestionDataModel> QuestionsDM = _mapper.Map<IEnumerable<TestQuestionDataModel>>(questions);
                _applicationContext.TestQuestionDataModels.AddRange(QuestionsDM);
                _applicationContext.SaveChanges();
                countQuestions = QuestionsDM.Count();
            }
            catch
            {

            }
            return countQuestions;
        }

        public void Delete(int id)
        {
            TestQuestionDataModel questionDM = _applicationContext.TestQuestionDataModels.FirstOrDefault(u => u.QuestionId == id);
            //TestQuestionDataModel questionDM = _applicationContext.TestQuestionDataModels.Find(id);
            if (questionDM != null)
            {
                _applicationContext.TestQuestionDataModels.Remove(questionDM);
                _applicationContext.SaveChanges();
            }
        }

        public TestQuestion Get(int id)
        {
            TestQuestionDataModel questionDM = _applicationContext.TestQuestionDataModels.FirstOrDefault(u => u.QuestionId == id);

            TestQuestion testQuestion = new TestQuestion();

            if (questionDM == null)
            {
                return testQuestion;
            }
            testQuestion = _mapper.Map<TestQuestion>(questionDM);

            return testQuestion;
        }

        public List<TestQuestion> GetAll()
        {
            List<TestQuestionDataModel> questionsDM = _applicationContext.TestQuestionDataModels.ToList();

            List<TestQuestion> testQuestions = new List<TestQuestion>();

            if (questionsDM is null)
            {
                return testQuestions;
            }

            testQuestions = _mapper.Map<IEnumerable<TestQuestion>>(questionsDM).ToList();

            return testQuestions;
        }

        public List<TestQuestion> GetQuestionsInTheme(string nameTheme)
        {
            List<TestQuestionDataModel> questionsDM = _applicationContext.TestQuestionDataModels.Where(u => u.NameTheme == nameTheme).ToList();

            List<TestQuestion> testQuestions = new List<TestQuestion>();

            if (questionsDM is null)
            {
                return testQuestions;
            }
            testQuestions = _mapper.Map<IEnumerable<TestQuestion>>(questionsDM).ToList();

            return testQuestions;
        }

        public List<TestQuestion> GetRndQuestionsInTheme(string nameTheme, int number)
        {
            List<TestQuestionDataModel> questionsDM = new List<TestQuestionDataModel>();
            questionsDM = _applicationContext.TestQuestionDataModels.Where(u => u.NameTheme == nameTheme).ToList();

            List<TestQuestion> testQuestions = new List<TestQuestion>();

            if (questionsDM is null)
            {
                return testQuestions;
            }

            testQuestions = _mapper.Map<IEnumerable<TestQuestion>>(questionsDM).ToList();

            Shuffle(testQuestions);

            return testQuestions.GetRange(0, number);
        }

        #region randomShafle

        private Random rng = new Random();

        public void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        #endregion

        public bool CheckQuestionsOnNameQuestion(string nameQuestion)
        {
            var question = _applicationContext.TestQuestionDataModels.FirstOrDefault(u => u.NameQuestion == nameQuestion);

            if (question is null)
            {
                return false;
            }

            return true;
        }

        public bool CheckQuestions(TestQuestion question)
        {
            var questionInBase = _applicationContext.TestQuestionDataModels.FirstOrDefault(u => u.NameQuestion == question.NameQuestion && u.NameTheme == question.NameTheme && u.ImageQuestion == question.ImageQuestion && u.NameAnswerCorrect1 == question.NameAnswerCorrect1 && u.NameAnswerIncorrect1 == question.NameAnswerIncorrect1 && u.NameAnswerIncorrect2 == question.NameAnswerIncorrect2 && u.NameAnswerIncorrect3 == question.NameAnswerIncorrect3);

            if (questionInBase is null)
            {
                return false;
            }

            return true;
        }

        public int? GetIdQuestions(TestQuestion question)
        {
            var questionInBase = _applicationContext.TestQuestionDataModels.FirstOrDefault(u => u.NameQuestion == question.NameQuestion && u.NameTheme == question.NameTheme && u.NameAnswerCorrect1 == question.NameAnswerCorrect1 && u.NameAnswerIncorrect1 == question.NameAnswerIncorrect1 && u.NameAnswerIncorrect2 == question.NameAnswerIncorrect2 && u.NameAnswerIncorrect3 == question.NameAnswerIncorrect3);

            if (questionInBase is null)
            {
                return null;
            }

            return questionInBase.QuestionId;
        }

        public TestQuestion GetQuestionsOnNameQuestion(string nameQuestion)
        {
            var questionDM = _applicationContext.TestQuestionDataModels.FirstOrDefault(u => u.NameQuestion == nameQuestion);

            var testQuestions = new TestQuestion();


            if (questionDM is null)
            {

                return testQuestions;
            }

            testQuestions = _mapper.Map<TestQuestion>(questionDM);

            return testQuestions;
        }

        public List<string> GetThemesInSpeciality(string nameSpeciality)
        {
            var themes = _applicationContext.TestQuestionDataModels.Where(u => u.NameSpeciality == nameSpeciality).Select(n => n.NameTheme);

            if (themes is null)
            {
                var nullThemes = new List<string>();
                return nullThemes;
            }

            return themes.ToList();
        }

        public List<string> GetThemes()
        {
            var themes = new List<string>();

            themes = _applicationContext.TestQuestionDataModels.Select(n => n.NameTheme).Distinct().ToList();

            return themes.ToList();
        }

        public void Update(TestQuestion itemQuestion)
        {
            TestQuestionDataModel testQuestionDM = _mapper.Map<TestQuestionDataModel>(itemQuestion);
            var test = _applicationContext.TestQuestionDataModels.Where(x => x.QuestionId == itemQuestion.QuestionId).FirstOrDefault();
            _applicationContext.TestQuestionDataModels.Remove(test);
            _applicationContext.SaveChanges();
            _applicationContext.TestQuestionDataModels.Add(testQuestionDM);
            //_applicationContext.TestQuestionDataModels.AddRange(testQuestionDM);
            _applicationContext.SaveChanges();
        }

        public int GetNumberQuestionInTheme(string nameTheme)
        {
            var numberQuestionInTheme = _applicationContext.TestQuestionDataModels.Where(u => u.NameTheme == nameTheme).Select(n => n.NameTheme).Count();
            return numberQuestionInTheme;
        }
    }
}
