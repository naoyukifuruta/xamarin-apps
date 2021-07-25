namespace SimpleTimecard.Interfaces
{
    public interface IToast
    {
        /// <summary>
        /// トースト表示
        /// </summary>
        /// <param name="message">メッセージ</param>
        void Show(string message);

        /// <summary>
        /// トースト表示（表示長さ指定版）
        /// </summary>
        /// <example>
        /// Length:0-Short
        /// Length:1-Long
        /// </example>
        /// <param name="message">メッセージ</param>
        /// <param name="length">表示長さ</param>
        void Show(string message, int length);
    }
}
