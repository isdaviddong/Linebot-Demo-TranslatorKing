using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using isRock.LineBot;

namespace TranslatorKingMain.Controllers
{
    public class LineBotWebHookController : isRock.LineBot.LineWebHookControllerBase
    {
        //const string channelAccessToken = "!!!!! 改成自己的ChannelAccessToken !!!!!";
        string helpMsg = "Hi, 我是翻譯助理, 你輸入任何文字，我都可以翻譯成您指定的語言，目前預設是英文\n\n如果你想將預設翻譯語言改成其他的，可以試試用手機按下底下選單，或是輸入 \n/翻成英文   \n/翻成日文  \n/翻成西班牙文 \n/翻成克林貢文\n...\n其實，我可以翻譯十數種語言，包含中文,英文,日文,西班牙文,韓文,印度文,希伯來文,克林貢文,馬來文,越南文,菲律賓文,泰文,德文,法文,義大利文,俄文,烏克蘭文...\n\n試試看囉~";

        private string GetState(string UserID)
        {
            if (System.Web.HttpContext.Current.Application[UserID] == null)
                System.Web.HttpContext.Current.Application[UserID] = "en";
            return System.Web.HttpContext.Current.Application[UserID].ToString();
        }

        private void SetState(string UserID, string LanguageCode)
        {
            System.Web.HttpContext.Current.Application[UserID] = LanguageCode;
        }

        [Route("api/TranslatorKing")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            //設定ChannelAccessToken(或抓取Web.Config)
            //this.ChannelAccessToken = channelAccessToken;
            //取得Line Event(這是範例，因此只取第一個)
            var LineEvent = this.ReceivedMessage.events.FirstOrDefault();

            try
            {
                //配合Line verify 
                if (LineEvent.replyToken == "00000000000000000000000000000000") return Ok();

                //回覆訊息
                if (LineEvent.type == "message")
                {
                    if (LineEvent.message.type == "text") //收到文字
                    {
                        if (LineEvent.message.text.Trim() == "/help" || LineEvent.message.text.Trim() == "/說明")
                        {
                            this.ReplyMessage(LineEvent.replyToken, helpMsg);
                            return Ok();
                        }

                        //如果是command
                        if (LineEvent.message.text.Trim().StartsWith("/"))
                        {
                            var response = ProcessCommand(LineEvent);
                            if (response != null)
                                this.ReplyMessage(LineEvent.replyToken, response);
                            else
                                this.ReplyMessage(LineEvent.replyToken, helpMsg);
                            return Ok();
                        }

                        //翻譯成目標語言
                        var result = Models.MSTranslatorUtility.Translate(LineEvent.message.text.Trim(), GetState(LineEvent.source.userId));
                        this.ReplyMessage(LineEvent.replyToken, result.FirstOrDefault().translations.FirstOrDefault().text);
                        //if (LineEvent.message.type == "sticker") //收到貼圖，回覆耶圖
                        //    this.ReplyMessage(LineEvent.replyToken, 1, 2);
                    }
                    else
                        this.ReplyMessage(LineEvent.replyToken, helpMsg);
                }
                else
                {
                    this.ReplyMessage(LineEvent.replyToken, helpMsg);
                }
                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //如果發生錯誤
                this.ReplyMessage(LineEvent.replyToken, $"準備將\n{LineEvent.message.text} \n翻譯為 {  System.Web.HttpContext.Current.Application[LineEvent.source.userId].ToString()} 時, \n發生了點差錯: {ex.Message} 請截圖後回報開發團隊，謝謝。 \n");
                //response OK
                return Ok();
            }
        }

        private isRock.LineBot.MessageBase ProcessCommand(Event LineEvent)
        {
            var language = LineEvent.message.text.Trim().Replace("/", "").Replace("翻成", "");
            var SupportLanguage = new Dictionary<string, string>();
            SupportLanguage.Add("中文", "zh-Hant");
            SupportLanguage.Add("英文", "en");
            SupportLanguage.Add("日文", "ja");
            SupportLanguage.Add("西班牙文", "es");
            SupportLanguage.Add("韓文", "ko");
            SupportLanguage.Add("印度文", "hi");
            SupportLanguage.Add("希伯來文", "he");
            SupportLanguage.Add("克林貢文", "tlh");
            SupportLanguage.Add("馬來文", "ms");
            SupportLanguage.Add("越南文", "vi");
            SupportLanguage.Add("菲律賓文", "fil");
            SupportLanguage.Add("泰文", "th");
            SupportLanguage.Add("德文", "de");
            SupportLanguage.Add("法文", "fr");
            SupportLanguage.Add("義大利文", "it");
            SupportLanguage.Add("俄文", "ru");
            SupportLanguage.Add("烏克蘭文", "uk");

            var ret = from c in SupportLanguage
                      where c.Key == language
                      select c;
            if (ret.Count() > 0)
            {
                SetState(LineEvent.source.userId, ret.FirstOrDefault().Value);
                var ResponseMsg = new TextMessage($"好的，後面你輸入的句子我會開始翻譯成{ret.FirstOrDefault().Key}");
                return ResponseMsg;
            }
            else
            {
                var ResponseMsg = new TextMessage($"喔喔，我還認不得你要的語言 - {language}");
                return ResponseMsg;
            }
        }
    }
}
