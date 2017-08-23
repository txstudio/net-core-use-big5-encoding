using System;
using System.Text;
using System.Net.Http;

namespace net_core_use_big5_encoding
{
    class Program
    {
        /*
            需額外安裝套件 "System.Text.Encoding.CodePages" 
            透過 dotnet clr 安裝套件指令
            dotnet add package System.Text.Encoding.CodePages

            需呼叫 Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
            才可以使用額外的 Encoding 名稱 (ex: big5)


            範例程式碼為透過 HttpClient 類別呼叫
            http://isin.twse.com.tw/isin/C_public.jsp?strMode=2
            取得有價證卷編碼清單（網頁編碼格式為 "big5"）
        */

        static void Main(string[] args)
        {
            Byte[] _htmlBuffer;
            string _html;

            StockListedProxy _proxy;
            Encoding _encoding;

            _proxy = new StockListedProxy();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _encoding = Encoding.GetEncoding("big5");

            //取得網頁的 byte[] 資料
            _htmlBuffer = _proxy.GetHtml();

            //使用 big5 編碼取得文字內容
            _html = _encoding.GetString(_htmlBuffer);
            
            //輸出文字內容
            Console.WriteLine(_html);

            Console.WriteLine("--------");
            Console.WriteLine("執行完畢");
            Console.WriteLine("--------");
        }
    }

    internal class StockListedProxy
    {
        const string baseAddress = "http://isin.twse.com.tw";
        const string relativeUri = "/isin/C_public.jsp?strMode=2";

        private HttpClient _client;

        public StockListedProxy()
        {
            this._client = new HttpClient();
            this._client.BaseAddress = new Uri(baseAddress);
        }

        ///<summary>取得網頁內容</summary>
        public Byte[] GetHtml()
        {
            var _response = this._client.GetAsync(relativeUri).Result;

            if(_response.IsSuccessStatusCode == true)
            {
                var _bytes = _response.Content.ReadAsByteArrayAsync().Result;

                return _bytes;
            }

            return null;
        }
    }
}