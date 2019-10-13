using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpertSystems
{
    public class Journal
    {
        public int Id { get; set; }
        //Суммарное число цитирований журнала в РИНЦ
        public int SumNumbersOfCitations { get; set; }
        //Среднее число статей в выпуске
        public double AverageNumbersOfArticles { get; set; }
        //Средняя оценка по результатам общественной экспертизы
        public double AverageMarkOfPublicExpertise { get; set; }
        //Показатель журнала в рейтинге SCIENCE INDEX
        public double IndicatorInRating {get;set;}
        //Средний индекс Хирша авторов
        public double AverageIndexHirsh { get; set; }
    }
}
