namespace TesterMaster.Slack
{
    using SlackBotMessages;
    using SlackBotMessages.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Net;
    using System.Text;

    public class TestSlackMessage
    {
        public void UseWrapper()
        {
            var service = new SlackWrapper("https://hooks.slack.com/services/");

            var message = service.ConfigureMessage("#test-channel-for-post-messages-to-slack", "Custom name");

            service.AddAttachment(ref message, SlackMessageAttachmentEnum.Success, "pre text", "Test name", "Field");

            service.SendMessage(message);
        }

        public void GetResponse()
        {
            NameValueCollection data = new NameValueCollection
            {
                ["token"] = "xoxb-here-must-be-token",
                ["channel"] = "#channel name",
                ["username"] = "botname", // You can specify it yourself
                ["icon_emoji"] = ":chart_with_upwards_trend:", //icon

                ["text"] = "test message again"  //text
            };

            using (WebClient client = new WebClient())
            {
                byte[] response = client.UploadValues("https://slack.com/api/chat.postMessage", "POST", data);
                string responseInString = Encoding.UTF8.GetString(response);
                Console.WriteLine(responseInString);
            }
        }

        public void SendMessageWithSlackBot()
        {
            SbmClient client = new SbmClient("https://hooks.slack.com/services/");

            Message message = new Message
            {
                Text = "You are using Slack Bot Messages by Paul Seal from codeshare.co.uk",
                Channel = "general",
                Username = "SlackBotMessages",
                IconEmoji = ":party_parrot:",
                Attachments = new List<Attachment>
    {
        new Attachment
        {
            Fallback = "This is for slack clients which choose not to display the attachment.",
            Pretext = "This line appears before the attachment",
            Text = "This line of text appears inside the attachment",
            Color = "good",
            Fields = new List<Field>
            {
                new Field
                {
                    Title = Emoji.HeavyCheckMark + " Success",
                    Value = "You can add multiple lines to an attachment\n\nLike this\n\nAnd this."
                }
            }
        }
                }
            };

            client.Send(message);
            //or send it fully async like this:
            //await client.SendAsync(message).ConfigureAwait(false);
        }

    }

}