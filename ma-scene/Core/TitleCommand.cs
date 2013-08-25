namespace Vbc.MA.Scenario.Core
{
    /// <summary>
    /// 제목 커맨드의 내용을 담는 클래스입니다.
    /// </summary>
    public class TitleCommand : ScenarioCommand
    {
        private string mTitle;

        /// <summary>
        /// 제목을 가져옵니다.
        /// </summary>
        public string Title
        {
            get { return mTitle; }
        }

        /// <summary>
        /// 제목으로 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="title">설정할 제목입니다.</param>
        public TitleCommand(string title)
            : base(CommandType.Title)
        {
            mTitle = title;
        }

        public override string ToString()
        {
            return mTitle;
        }

        public override string Raw
        {
            get { return string.Format("TI {0}", mTitle); }
        }
    }
}
