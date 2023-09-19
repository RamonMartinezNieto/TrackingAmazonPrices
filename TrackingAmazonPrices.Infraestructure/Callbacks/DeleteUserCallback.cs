using System.Text;
using Telegram.Bot.Types;
using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Application.Callbacks;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Infraestructure.Callbacks
{
    internal class DeleteUserCallback : ICallback
    {
        private readonly ILogger<DeleteUserCallback> _logger;
        private readonly IMessageHandler _handlerMessage;
        private readonly IDatabaseUserService _userService;
        private readonly ILiteralsService _literalsService;

        public DeleteUserCallback(
            ILogger<DeleteUserCallback> logger,
            IMessageHandler handlerMessage,
            IDatabaseUserService userService,
            ILiteralsService literalsService)
        {
            _logger = logger;
            _handlerMessage = handlerMessage;
            _userService = userService;
            _literalsService = literalsService;
        }

        public async Task<bool> ExecuteAsync(object objectMessage, string dataCallback)
        {
            var user = await _handlerMessage.GetUser(objectMessage);
            var userLanguage = user.Language.LanguageCode;

            if (dataCallback.Equals("no"))
            {
                _ = await _handlerMessage.AnswerdCallback(
                    objectMessage,
                    await _literalsService.GetAsync(userLanguage, Literals.GoodDay));

                return true;
            }

            if (!await _userService.UserExists(user.UserId))
            {
                _ = await _handlerMessage.AnswerdCallback(
                    objectMessage,
                    await _literalsService.GetAsync(Literals.NoUser));
            }

            _logger.LogInformation("Start deleting user {id}", user.UserId);

            bool result = await _userService.DeleteUser(user.UserId);

            if (result) { 
                _logger.LogInformation("User deleted {id}", user.UserId);

                string userDeleted = await _literalsService.GetAsync(userLanguage, Literals.UserDeleted);
                string goodDay = await _literalsService.GetAsync(userLanguage, Literals.GoodDay);
                string message = $"{userDeleted} {goodDay}";

                _ = await _handlerMessage.AnswerdCallback(objectMessage, message);
                _ = await _handlerMessage.SentMessageAsync(objectMessage, message);
            }

            return result;
        }
    }
}