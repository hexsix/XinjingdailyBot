﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using XinjingdailyBot.Infrastructure;
using XinjingdailyBot.Infrastructure.Attribute;
using XinjingdailyBot.Infrastructure.Extensions;
using XinjingdailyBot.Interface.Bot;

namespace XinjingdailyBot.Service.Bot;


[AppService(ServiceType = typeof(IChannelService), ServiceLifetime = LifeTime.Singleton)]
public class ChannelService : IChannelService
{
    private readonly ITelegramBotClient _botClient;
    private readonly OptionsSetting _optionsSetting;
    private readonly ILogger<ChannelService> _logger;

    private Chat _reviewGroup = new();
    private Chat _commentGroup = new();
    private Chat _subGroup = new();
    private Chat _acceptChannel = new();
    private Chat _rejectChannel = new();
    private User _botUser = new();

    Chat IChannelService.ReviewGroup { get => _reviewGroup; }
    Chat IChannelService.CommentGroup { get => _commentGroup; }
    Chat IChannelService.SubGroup { get => _subGroup; }
    Chat IChannelService.AcceptChannel { get => _acceptChannel; }
    Chat IChannelService.RejectChannel { get => _rejectChannel; }
    User IChannelService.BotUser { get => _botUser; }

    public ChannelService(
       ILogger<ChannelService> logger,
        ITelegramBotClient botClient,
        IOptions<OptionsSetting> optionsSetting)
    {
        _logger = logger;
        _botClient = botClient;
        _optionsSetting = optionsSetting.Value;
    }

    public async Task InitChannelInfo()
    {
        _botUser = await _botClient.GetMeAsync();

        _logger.LogInformation("机器人信息: {Id} {nickName} @{userName}", _botUser.Id, _botUser.NickName(), _botUser.Username);

        var channelOption = _optionsSetting.Channel;

        try
        {
            _acceptChannel = await _botClient.GetChatAsync(channelOption.AcceptChannel);
            _logger.LogInformation("稿件发布频道: {chatProfile}", _acceptChannel.ChatProfile());
        }
        catch
        {
            _logger.LogError("未找到指定的稿件发布频道, 请检查拼写是否正确");
            throw;
        }
        try
        {

            _rejectChannel = await _botClient.GetChatAsync(channelOption.RejectChannel);
            _logger.LogInformation("拒稿存档频道: {chatProfile}", _rejectChannel.ChatProfile());
        }
        catch
        {
            _logger.LogError("未找到指定的拒稿存档频道, 请检查拼写是否正确");
            throw;
        }

        try
        {
            if (long.TryParse(channelOption.ReviewGroup, out long groupId))
            {
                _reviewGroup = await _botClient.GetChatAsync(groupId);
            }
            else
            {
                _reviewGroup = await _botClient.GetChatAsync(channelOption.ReviewGroup);
            }
            _logger.LogInformation("审核群组: {chatProfile}", _reviewGroup.ChatProfile());
        }
        catch
        {
            _logger.LogError("未找到指定的审核群组, 可以使用 /groupinfo 命令获取群组信息");
            _reviewGroup = new() { Id = -1 };
        }

        try
        {
            if (long.TryParse(channelOption.CommentGroup, out long subGroupId))
            {
                _commentGroup = await _botClient.GetChatAsync(subGroupId);
            }
            else
            {
                _commentGroup = await _botClient.GetChatAsync(channelOption.CommentGroup);
            }
            _logger.LogInformation("评论区群组: {chatProfile}", _commentGroup.ChatProfile());
        }
        catch
        {
            _logger.LogError("未找到指定的评论区群组, 可以使用 /groupinfo 命令获取群组信息");
            _commentGroup = new() { Id = -1 };
        }

        try
        {
            if (long.TryParse(channelOption.SubGroup, out long subGroupId))
            {
                _subGroup = await _botClient.GetChatAsync(subGroupId);
            }
            else
            {
                _subGroup = await _botClient.GetChatAsync(channelOption.SubGroup);
            }
            _logger.LogInformation("频道子群组: {chatProfile}", _subGroup.ChatProfile());
        }
        catch
        {
            _logger.LogError("未找到指定的闲聊群组, 可以使用 /groupinfo 命令获取群组信息");
            _subGroup = new() { Id = -1 };
        }

        if (_subGroup.Id == -1 && _commentGroup.Id != -1)
        {
            _subGroup = _commentGroup;
        }
        else if (_commentGroup.Id == -1 && _subGroup.Id != -1)
        {
            _commentGroup = _subGroup;
        }



    }

}