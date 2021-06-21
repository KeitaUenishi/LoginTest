using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;


namespace LoginTest.Pages
{
    public partial class Chat : ComponentBase, IDisposable
    {
        #region フィールド
        private List<string> _messages = new List<string>();
        private string _messageInput;
        private HubConnection _hubConnection;
        #endregion

        #region プロパティ
        [Inject]
        public NavigationManager Navigation { get; set; }
        public bool IsConnected => _hubConnection.State == HubConnectionState.Connected;
        private async Task SendAsync()
        {
            await _hubConnection.SendAsync("SendMessageClientsAll", _messageInput);
            _messageInput = string.Empty;
        }
        #endregion

        #region メソッド
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("/chathub")) //startupのMapHubで指定したHubのURLを指定する.
                .WithAutomaticReconnect(new RandomRetryPolicy()) //自動接続
                .Build();

            _hubConnection.On<string>("ReceiveMessage", (message) =>
            {
                if (string.IsNullOrEmpty(message)) return;

                //入力文字列の中にURLが存在するかどうかを判定だけする
                var urlPattern = new Regex(@"(https?|ftp)(:\/\/[-_.!~*\'()a-zA-Z0-9;\/?:\@&=+\$,%#]+)");
                var urlPatternMatch = urlPattern.Match(message);

                //入力文字列をサニタイズする
                message = HttpUtility.HtmlEncode(message);

                //入力文字列の中にURLが存在する場合はアンカータグに変換する
                if (urlPatternMatch.Success)
                {
                    message = message.Replace(urlPatternMatch.Value, $"<a href=\"{urlPatternMatch}\">{urlPatternMatch}</a>");
                }

                _messages.Insert(0, message);
                StateHasChanged();
            });

            _hubConnection.Reconnected += async connectionId =>
            {
                await _hubConnection.StartAsync();
            };

            //画面の更新を行う
            await _hubConnection.StartAsync();
        }

        /// <summary>
        /// enterキー押下時のイベント
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task KeyDownAsync(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SendAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _ = _hubConnection.DisposeAsync();
        }
        #endregion

        #region クラス
        public class RandomRetryPolicy : IRetryPolicy
        {
            private readonly Random _random = new Random();

            public TimeSpan? NextRetryDelay(RetryContext retryContext)
            {
                //2～5秒の間でランダムに再接続を試みる
                return TimeSpan.FromSeconds(_random.Next(2, 5));
            }
        }
        #endregion

    }
}
