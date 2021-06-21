using System;
using LoginTest.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace LoginTest.Pages
{
    public partial class Index
    {
        #region プロパティ
        /// <summary>
        /// Inject属性を指定することで、NavigationManagerのサービスの依存関係を挿入します。
        /// </summary>
        [Inject]
        public NavigationManager Navigation { get; set; }
        /// <summary>
        /// ログイン情報を保持
        /// </summary>
        public LoginData LoginData { get; set; }
        #endregion

        #region コンストラクタ
        public Index()
        {
            //Index.razorから参照するのでインスタンス生成をしておかないとエラーとなる
            LoginData = new LoginData();
        }
        #endregion

        #region メソッド
        /// <summary>
        /// Validate処理成功時に処理
        /// </summary>
        /// <param name="context"></param>
        public void OnValidSubmit(EditContext context)
        {
            Console.WriteLine($"OnValidSubmit()");
            Navigation.NavigateTo("Chat", false);
        }
        /// <summary>
        /// Validate処理失敗時に処理
        /// </summary>
        /// <param name="context"></param>
        public void OnInvalidSubmit(EditContext context)
        {
            Console.WriteLine($"OnInvalidSubmit()");
        }
        #endregion
    }
}
