Linebot-Demo-TranslatorKing (翻譯王 Bot)
===
此Line bot範例為使用 LineBotSDK 建立的 『多國語言即時翻譯 Bot 』<br><br>
用戶可以跟 bot 說 任何文字，它會翻譯成你需要的語言。 <br>
目前支援十多種語言...中文,英文,日文,西班牙文,韓文,印度文,希伯來文,馬來文,越南文,菲律賓文,泰文,德文,法文,義大利文,俄文,烏克蘭文...還有...克林貢文... <br><br>
把它放到群組裡，可以輕鬆跟國外友人對話，幫你把國外朋友說的話都翻成中文，而你說的話則翻成對方熟悉的語言...<br>

使用畫面
===
 ![](https://imgur.com/LlWsuJu.png)

測試 - 想要試玩看看?
===
您可以用LINE 搜尋 @ejn1954w 將其加入好友即可測試 <br>
或用手機點選底下連結: <br>
https://line.me/R/ti/p/%40ejn1954w <br>

如何佈署專案
===
* 請 clone 之後，修改 web.config 中的 ChannelAccessToken
```xml
  <appSettings>
    <add key="ChannelAccessToken" value="請改成你自己的channel access token"/>
  </appSettings>
```
* 若為了便於除錯，可修改 LineWebHookSampleController.cs 中的 Admin User Id，將發生Exception時候的錯誤轉給自己
```csharp
   catch (Exception ex)
            {
                //回覆訊息
                this.PushMessage("請改成你自己的Admin User Id", "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
```
* 建議使用Ngrok進行測試 <br/>
(可參考 https://youtu.be/kCga1_E-ijs ) 
* LINE Bot後台的WebHook設定，其位置為 Http://你的domain/api/TranslatorKing

資料庫 或 其他相依需求
===
* 本範例沒有使用資料庫
* 本範例使用到了MS Cognitivi Services進行線上即時翻譯，請申請MS translator API並在Web.Config中填入Key
```xml
  <appSettings>
    <!--請換成你自己的key-->
    <add key="MSTranslatorTextKey" value=""/>
  </appSettings>
```

注意事項
===
由於這只是一個範例，我們盡可能用最簡單的方式來開發。 <br/>
範例中記錄狀態的方式採用了Application[]變數，但實務上建議您調整成資料庫或其他storage。

相關資源 
===
<br/>LineBotSDK：https://www.nuget.org/packages/LineBotSDK
<br/>相關課程：http://www.studyhost.tw/NewCourses/LineBot
<br/>線上課程：https://www.udemy.com/line-bot/
<br/>更多內容，請參考電子書：https://www.pubu.com.tw/ebook/103305
<br/>LINE Bot實體書籍：https://www.tenlong.com.tw/products/9789865020354

