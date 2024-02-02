using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testApp.DataModels;
using testApp.Models;

namespace testApp.Mappers
{
    public class MappersQuestion : Profile
    {
        public MappersQuestion()
        {
            CreateMap<TestQuestion, TestQuestionDataModel>().ReverseMap();
        }
    }
}
