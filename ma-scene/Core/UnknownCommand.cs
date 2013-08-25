namespace Vbc.MA.Scenario.Core
{
    /// <summary>
    /// 알 수 없는 커맨드의 내용을 담는 클래스입니다.
    /// </summary>
    public class UnknownCommand : ScenarioCommand
    {
        private string mRaw;

        /// <summary>
        /// 처리되지 않은 데이터로 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="raw">처리되지 않은 데이터입니다.</param>
        public UnknownCommand(string raw)
            : base(CommandType.Unknown)
        {
            mRaw = raw;
        }

        public override string Raw
        {
            get { return mRaw; }
        }

        public override string ToString()
        {
            return mRaw;
        }
    }
}
