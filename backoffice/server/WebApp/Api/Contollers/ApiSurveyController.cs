using ECCSA.BackOffice.Server.Model.Common;
using ECCSA.BackOffice.Server.Model.Security;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common;
using ECCSA.BackOffice.Server.Persistence.Repositories.Security;
using ECCSA.BackOffice.Server.WebApp.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECCSA.BackOffice.Server.Persistence.Repositories.Common.Notification;

namespace ECCSA.BackOffice.Server.WebApp.Api.Contollers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ApiSurveyController : BaseController
    {
        private readonly ILogger<ApiSurveyController> _logger;
        private readonly SurveyRepository _repository;
        public ApiSurveyController(ILogger<ApiSurveyController> logger,
            UserManager<User> userManager, SurveyRepository repository,
            PublicUserRepository userRepository, NotificationRepository notificationRepository) : base(userManager, userRepository, notificationRepository)
        {
            _logger = logger;
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetSurveyList()
        {
            var serviceRequests = await _repository.PublicSurveys(TheUser.Id);

            return Ok(serviceRequests);
        }
        [HttpGet]
        public async Task<IActionResult> UserNotFinishedSurveys()
        {
            var serviceRequests = await _repository.UserNotFinishedSurveys(TheUser.Id);

            return Ok(serviceRequests);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserSurvey(Guid id)
        {
            try
            {
                var result = await _repository.GetSurveySettings(id, TheUser.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> SaveResponse(Guid id, IFormCollection formCollection)
        {
            var result = await _repository.GetSurveySettings(id, TheUser.Id);
            var respondant = await _repository.GetSurveyRespondant(id, TheUser.Id);
            foreach (var q in result.Questions)
            {
                var key = Guid2String(q.Id);
                var val = formCollection[key];
                if (q.Response == null || q.Response.Id == Guid.Empty)
                {
                    q.Response = new SurveyResponse
                    {
                        Id = Guid.NewGuid(),
                        QuestionId = q.Id,
                        SurveyRespondentId = respondant.Id,
                        SurveyId = respondant.SurveyId,
                        TextualResponse = ""
                    };
                } 
                else if (q.Response == null || q.Response.SurveyId == Guid.Empty)
                {
                    q.Response.SurveyId = id;
                }
                
                switch (q.QuestionType)
                {
                    case QuestionType.Numeric:
                        if (int.TryParse(val, out var number))
                            q.Response.NumericResponse = number;
                        else
                        {
                            q.Response.NumericResponse = 0;
                        }
                        break;
                    case QuestionType.Text:
                        q.Response.TextualResponse = val;
                        break;
                    case QuestionType.SingleChoice:
                        if (Guid.TryParse(val, out var gVal))
                        {
                            q.Response.Choices = new List<SelectedChoice>
                                {
                                    new SelectedChoice
                                    {
                                        ChoiceId = gVal,
                                        Rank = 0,
                                        ResponseId = q.Response.Id
                                    }
                                };
                        }
                        else
                        {
                            q.Response.Choices = new List<SelectedChoice>();
                        }
                        break;
                    case QuestionType.MultipleChoice:
                        q.Response.Choices = val.ToString().Split(",").Where(v => Guid.TryParse(v, out var gVal)).Select(v => new SelectedChoice
                        {
                            ChoiceId = Guid.Parse(v),
                            Rank = 0,
                            ResponseId = q.Response.Id
                        }
                        ).ToList();
                        break;
                }

                try
                {
                    await _repository.SaveResponses(q.Response);
                }
                catch (Exception ex)
                {
                    LogError(_logger, ex, "ApiSurvey", "SaveResponse", formCollection);
                    return Ok(new
                    {
                        saved = false,
                        message = ex.Message,
                    });
                }
            }
            return Ok(new
            {
                saved = true
            });
        }
    }
}
