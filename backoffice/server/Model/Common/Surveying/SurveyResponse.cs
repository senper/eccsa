using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECCSA.BackOffice.Server.Model.Common
{
    public class SurveyResponse
    {
        public Guid Id { get; set; }
        public Guid? SurveyRespondentId { get; set; }

        public Guid SurveyId { get; set; }

        public Guid QuestionId { get; set; }
        public string RespondentName { get; set; }



        public ICollection<SelectedChoice> Choices { get; set; }

        public decimal NumericResponse { get; set; }

        public String TextualResponse { get; set; }

        public virtual SurveyRespondent Respondent { get; set; }

        public virtual Survey Survey { get; set; }

        public string Response()
        {
            if (Choices != null)
            {
                var sb = new StringBuilder();
                foreach (var choice in Choices.OrderBy(x => x.Rank))
                {
                    if (sb.Length > 0)
                    {
                        //sb.Append("U+002CU+0020");
                        sb.Append(", ");
                        //sb.Append("U+002CU+0020");
                    }
                    sb.Append($"{choice.Choice.OrderNo}.{choice.Choice.ChoiceText}");
                }
                return sb.ToString();
            }
            if (!string.IsNullOrEmpty(TextualResponse))
                return TextualResponse;
            return NumericResponse.ToString();
        }

        public string Response(QuestionType questionType)
        {
            if ((questionType == QuestionType.MultipleChoice || questionType == QuestionType.SingleChoice) &&
                Choices != null)
            {
                var sb = new StringBuilder();
                foreach (var choice in Choices.OrderBy(x => x.Rank))
                {
                    if (sb.Length > 0)
                    {
                        //sb.Append("U+002CU+0020");
                        sb.Append(", ");
                        //sb.Append("U+002CU+0020");
                    }
                    sb.Append($"{choice.Choice.OrderNo}.{choice.Choice.ChoiceText}");
                }
                return sb.ToString();
            }
            else if (questionType == QuestionType.Text && !string.IsNullOrEmpty(TextualResponse))
                return TextualResponse;
            else if (questionType == QuestionType.Numeric)
                return NumericResponse.ToString();
            else
                return "";
        }
    }
}