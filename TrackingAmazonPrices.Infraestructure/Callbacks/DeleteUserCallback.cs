using System.Text;
using TrackingAmazonPrices.Application;
using TrackingAmazonPrices.Application.Callbacks;
using TrackingAmazonPrices.Application.Handlers;
using TrackingAmazonPrices.Application.Services;
using TrackingAmazonPrices.Domain.Enums;

namespace TrackingAmazonPrices.Infraestructure.Callbacks
{
    public class DeleteUserCallback : ICallback
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
                return await _handlerMessage.AnswerdCallback(
                    objectMessage,
                    await _literalsService.GetAsync(userLanguage, Literals.GoodDay));
            }

            if (!await _userService.UserExists(user.UserId))
            {
                return await _handlerMessage.AnswerdCallback(
                    objectMessage,
                    await _literalsService.GetAsync(Literals.NoUser));
            }

            _logger.LogInformation("Start deleting user {id}", user.UserId);

            bool result = await _userService.DeleteUser(user.UserId);

            if (result)
            {
                _logger.LogInformation("User deleted {id}", user.UserId);

                StringBuilder sb = new();
                sb.Append(await _literalsService.GetAsync(userLanguage, Literals.UserDeleted));
                sb.Append(' ');
                sb.Append(await _literalsService.GetAsync(userLanguage, Literals.GoodDay));

                result = await _handlerMessage.AnswerdCallback(objectMessage, sb.ToString());
            }

            return result;
        }
    }
}