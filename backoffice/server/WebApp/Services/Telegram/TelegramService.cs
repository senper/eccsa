using ECCSA.BackOffice.Server.Model.Common;
using MailService.WebApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Types = Telegram.Bot.Types;

namespace telegramtest
{

    public interface ITelegramService
    {


        Task<int> PublishToTelegramChannelAsync(string post, string picture);


        public bool SendMessage(string msg, string sendTo);

        bool GetUpdates(string from);


        bool SendPicture(string picture, string sendTo);

        Task<bool> SendPicture2Async(string sendTo);
        Task<bool> GetPosts(string from);

        Task<bool> GetPost(string from);

        Task<bool> SendPolls(string sendTo);
        Task<List<String>> SendSurveyPolls(SurveySetting setting, string groupId = null);
        Task<PollUpdateResult> GetPollResults(SurveySetting survey, Guid surveyId, List<string> pollIds);
        Task<bool> ConfirmPollUpdates(int lastUpdateId);

    }
    public class TelegramService : ITelegramService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        private readonly string _telegramAPI;
        private readonly string _botToken;
        private readonly string _channel;

        readonly TelegramBotClient botClient;
        public TelegramService(IConfiguration configuration/*, ILogger logger*/)
        {
            try
            {
                //Microsoft.Extensions.Logging.Logger
                _configuration = configuration;
                //_logger = logger;
                _telegramAPI = _configuration.GetSection("Telegram:API").Value;
                _telegramAPI = "https://api.telegram.org/bot"; // _configuration.GetSection("Telegram:API").Value;
                _botToken = _configuration.GetSection("Telegram:BotToken").Value;
                _channel = _configuration.GetSection("Telegram:Channel").Value;

                botClient = new TelegramBotClient(_botToken);
                var me = botClient.GetMeAsync().Result;
                Console.WriteLine(
                    $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
                );
            }
            catch
            {
                botClient = null;
            }
        }

        /// <summary>
        /// Publish to Telegram channel.
        /// </summary>
        /// <returns>0 - successful posting, 1 - published only picture, 2 - nothing has been published</returns>
        /// <param name="post">post to publish</param>
        /// <param name="picture">URL of the image to publish</param>
        public async Task<int> PublishToTelegramChannelAsync(string post, string picture)
        {
            //if (!SendPicture(picture, _channel))
            //{
            //    //return 2;
            //}

            // await SendPicture2Async(_channel);

            //   await GetPosts(_channel);
            //await GetPost(_channel);

            //if (!SendMessage(post, _channel))
            //{
            //    return 1;
            //}

            //   GetUpdates(_channel);

            // await  SendPolls(_channel);

            SendMessage(@"<div style='background:#eeeeee; border:1px solid #cccccc; padding:5px 10px'>
<p>TTTT &ouml;</p>

<hr /><a id='yyy' name='yyy'><tt>TTTTT</tt></a> <strong><s>DDDD</s></strong> <em>FFFF</em></div>

<ol>
	<li>1</li>
	<li>2</li>
	<li>
	<h2 style='font-style:italic'><var><span class='marker'>3</span></var></h2>
	</li>
</ol>", _channel);

            return 0;
        }

        /// <summary>
        /// Send a message to a Telegram chat/channel
        /// </summary>
        /// <param name="msg">Message text</param>
        /// <param name="sendTo">Recepient</param>
        public bool SendMessage(string msg, string sendTo)
        {
            if(botClient == null) return false;
            try
            {
                msg = WebUtility.UrlEncode(msg);

                using var httpClient = new HttpClient();
                var res = httpClient.GetAsync($"{_telegramAPI}{_botToken}/sendMessage?chat_id={sendTo}&text={msg}&parse_mode=HTML&disable_web_page_preview=true").Result;
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    //string content = res.Content.ReadAsStringAsync().Result;
                    //string status = res.StatusCode.ToString();
                    throw new Exception($"Couldn't send a message via Telegram. Response from Telegram API: {res.Content.ReadAsStringAsync().Result}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("SendMessage", ex);
                return false;
            }
            return true;
        }

        public bool GetUpdates(string from)
        {
            if (botClient == null) return false;
            try
            {

                using var httpClient = new HttpClient();
                var res = httpClient.GetAsync($"{_telegramAPI}{_botToken}/getUpdates?chat_id={from}&offset={-100000000000}").Result;
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    //string content = res.Content.ReadAsStringAsync().Result;
                    //string status = res.StatusCode.ToString();
                    throw new Exception($"Couldn't send a message via Telegram. Response from Telegram API: {res.Content.ReadAsStringAsync().Result}");
                }
                string content = res.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetUpdates", ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Send a picture to a Telegram chat/channel
        /// </summary>
        /// <param name="picture">URL of the image to send</param>
        /// <param name="sendTo">Recepient</param>
        public bool SendPicture(string picture, string sendTo)
        {
            if (botClient == null) return false;
            try
            {
                picture = WebUtility.UrlEncode(picture);

                using var httpClient = new HttpClient();
                var res = httpClient.GetAsync($"{_telegramAPI}{_botToken}/sendPhoto?chat_id={sendTo}&photo={picture}").Result;
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    string content = res.Content.ReadAsStringAsync().Result;
                    //string status = res.StatusCode.ToString();
                    throw new Exception($"Couldn't send a picture to Telegram. Response from Telegram API: {res.Content.ReadAsStringAsync().Result}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("SendPicture", ex);
                return false;
            }
            return true;
        }

        public async Task<bool> SendPicture2Async(string sendTo)
        {
            if (botClient == null) return false;
            try
            {
                var FileUrl = @"E:\\exper\\telegramtest\\bin\\Debug\\netcoreapp3.1\\pp1.jpg";
                using var stream = System.IO.File.Open(FileUrl, FileMode.Open);
                //  FileToSend fts = new FileToSend();
                // fts.Content = stream;
                //  fts.Filename = FileUrl.Split('\\').Last();
                var test = await botClient.SendPhotoAsync(sendTo, stream, "My Text");


            }
            catch (Exception ex)
            {
                _logger.LogError("SendPicture2", ex);
                return false;
            }
            return true;
        }
        public async Task<bool> GetPosts(string from)
        {
            if (botClient == null) return false;
            try
            {
                var resutl = await botClient.GetUpdatesAsync();
                var polls = resutl.Where(m => m.Type == Types.Enums.UpdateType.Poll).ToList();
                // ( polls[0] as Poll).
            }
            catch (Exception ex)
            {
                _logger.LogError("GetPosts", ex);
                return false;
            }
            return true;
        }

        public async Task<bool> GetPost(string from)
        {
            if (botClient == null) return false;
            try
            {
                var resutl = await botClient.ForwardMessageAsync(from, from, 46863810);

            }
            catch (Exception ex)
            {
                _logger.LogError("GetPost", ex);
                return false;
            }
            return true;
        }

        public async Task<bool> SendPolls(string sendTo)
        {
            if (botClient == null) return false;
            try
            {
                var resutl = await botClient.SendPollAsync(sendTo, "Is it popular?", Array.Empty<string>());
            }
            catch (Exception ex)
            {
                _logger.LogError("SendPolls", ex);
                return false;
            }
            return true;
        }

        public async Task<List<String>> SendSurveyPolls(SurveySetting setting, string groupId = null)
        {
            if (botClient == null) return new List<string>();
            groupId = groupId?.Trim();
            var chatId = string.IsNullOrEmpty(groupId) ? _channel : groupId;
            var list = new List<string>();

            foreach (var q in setting.Questions.OrderBy(s => s.OrderNo))
            {
                var resutl = await botClient.SendPollAsync(chatId: chatId, $"{q.OrderNo}. {q.Question}",
                    q.Choices.OrderBy(s => s.OrderNo).Select(c => $"{c.OrderNo}. {c.ChoiceText}").ToArray(),
                    allowsMultipleAnswers: q.QuestionType == ECCSA.BackOffice.Server.Model.Common.QuestionType.MultipleChoice,
                    isAnonymous: string.IsNullOrEmpty(groupId),
                    type: Types.Enums.PollType.Regular, openPeriod: setting.OpenDays * 60 * 60 * 24);
                list.Add(resutl.Poll.Id);
            }
            return list;
        }

        public async Task<PollUpdateResult> GetPollResults(SurveySetting survey, Guid surveyId, List<string> pollIds)
        {
            if (botClient == null) { return new PollUpdateResult(); }
            var responses = new List<SurveyResponse>();
            var votes = new List<SurveyVote>();
            int lastupdateId = 0;
            try
            {
                var resutl = await botClient.GetUpdatesAsync(
                    allowedUpdates: new List<Types.Enums.UpdateType> { Types.Enums.UpdateType.Poll, Types.Enums.UpdateType.PollAnswer }
                    );
                var polls = resutl.Where(m => pollIds.Any(p => p == m.Poll?.Id || p == m.PollAnswer?.PollId)).ToList();
                foreach (var p in polls)
                {
                    if (p.Poll == null && p.PollAnswer == null) continue;
                    var poll = p.Poll ?? (polls.FirstOrDefault(l => l.Poll?.Id == p.PollAnswer?.PollId)?.Poll);
                    if (poll == null) continue;
                    var question = survey.Questions.FirstOrDefault(q => $"{ q.OrderNo}. { q.Question}" == poll.Question);
                    if (question == null) continue;
                    if (p.Type == Types.Enums.UpdateType.Poll && p.Poll != null)// (string.IsNullOrEmpty(survey.GroupId))
                    {
                        foreach (var op in poll.Options)
                        {
                            var choice = question.Choices.FirstOrDefault(c => $"{c.OrderNo}. {c.ChoiceText}" == op.Text);
                            var oldVote = votes.FirstOrDefault(v => v.QuestionId == question.Id && v.ChoiceId == choice.Id);
                            if (oldVote == null)
                            {
                                votes.Add(new SurveyVote
                                {
                                    Id = Guid.NewGuid(),
                                    SurveyId = surveyId,
                                    QuestionId = question.Id,
                                    ChoiceId = choice.Id,
                                    Votes = op.VoterCount
                                });
                            }
                            else
                            {
                                oldVote.Votes = op.VoterCount;
                            }
                        }
                    }
                    else if (p.Type == Types.Enums.UpdateType.PollAnswer && p.PollAnswer != null)
                    {
                        var res = new SurveyResponse
                        {
                            Id = Guid.NewGuid(),
                            QuestionId = question.Id,
                            SurveyId = surveyId,
                            Choices = new List<SelectedChoice>(),
                            SurveyRespondentId = null,
                            TextualResponse = "",
                            RespondentName = $"{p.PollAnswer.User.FirstName} {p.PollAnswer.User.LastName}"
                        };
                        if (p.PollAnswer == null) continue;
                        foreach (var opId in p.PollAnswer.OptionIds)
                        {
                            var option = poll.Options[opId];
                            var choice = question.Choices.FirstOrDefault(c => $"{c.OrderNo}. {c.ChoiceText}" == option.Text);
                            res.Choices.Add(new SelectedChoice { ChoiceId = choice.Id, ResponseId = res.Id });
                        }
                        responses.Add(res);
                    }
                }
                if (resutl.Any())
                {
                    lastupdateId = resutl.LastOrDefault().Id;
                }
            }
            catch 
            {
                //_logger.LogError("GetPosts", ex);
                throw;
            }

            return new PollUpdateResult
            {
                LastPollId = lastupdateId,
                Survey = new Survey { Votes = votes, Responses = responses }
            };
        }

        public async Task<bool> ConfirmPollUpdates(int lastUpdateId)
        {
            if (botClient == null) return false;
            if (lastUpdateId > 0)
            {
                await botClient.GetUpdatesAsync(offset: lastUpdateId + 1,
                            allowedUpdates: new List<Types.Enums.UpdateType> { Types.Enums.UpdateType.Poll, Types.Enums.UpdateType.PollAnswer });
                return true;
            }
            return false;
        }
    }
    public class PollUpdateResult
    {
        public int LastPollId { get; set; }
        public Survey Survey { get; set; }
    }
}
