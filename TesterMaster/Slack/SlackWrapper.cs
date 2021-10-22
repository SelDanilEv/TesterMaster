namespace TesterMaster.Slack
{
    using SlackBotMessages;
    using SlackBotMessages.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public enum SlackMessageAttachmentEnum
    {
        Success,
        Warning,
        Error
    }

    public class SlackWrapper
    {
        private readonly SbmClient _client;

        public SlackWrapper(string webHookUrl)
        {
            _client = new SbmClient(webHookUrl);
        }

        public Message ConfigureMessage(string channel, string botName = "Adjustments Bot", string icon = Emoji.Notebook, string mainText = "")
        {
            var newMessage = new Message()
            {
                Channel = channel,
                Username = botName,
                IconEmoji = icon,
                Text = mainText,
                Attachments = new List<Attachment>()
            };

            return newMessage;
        }

        public void AddAttachment(ref Message message, SlackMessageAttachmentEnum attachmentType, string preText = "", string text = "", string fieldValue = "")
        {
            string color = "", title = "";
            switch (attachmentType)
            {
                case SlackMessageAttachmentEnum.Success:
                    color = "good";
                    title = Emoji.HeavyCheckMark + " Success";
                    break;
                case SlackMessageAttachmentEnum.Warning:
                    color = "warning";
                    title = Emoji.Warning + " Warning";
                    break;
                case SlackMessageAttachmentEnum.Error:
                    color = "danger";
                    title = Emoji.X + " Error";
                    break;
            }

            message.Attachments.Add(
                new Attachment
                {
                    Fallback = string.Empty,
                    Pretext = preText,
                    Text = text,
                    Color = color,
                    Fields = new List<Field>
                        {
                        new Field
                            {
                            Title = title,
                            Value = fieldValue
                            }
                        }
                }
            );
        }

        public async Task SendMessageAsync(Message message)
        {
            try
            {
                await _client.SendAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendMessage(Message message)
        {
            try
            {
                _client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
