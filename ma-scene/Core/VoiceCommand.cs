namespace Vbc.MA.Scenario.Core
{
    /// <summary>
    /// 보이스 커맨드의 내용을 담는 클래스입니다.
    /// </summary>
    public class VoiceCommand : ScenarioCommand
    {
        private string mId;

        /// <summary>
        /// 보이스 파일 ID를 가져옵니다.
        /// </summary>
        public string Id
        {
            get { return mId; }
        }

        /// <summary>
        /// 보이스 파일 ID로 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="id">보이스 파일 ID입니다.</param>
        public VoiceCommand(string id)
            : base(CommandType.Voice)
        {
            mId = id;
        }

        public override string Raw
        {
            get { return string.Format("VO {0}", mId); }
        }

        public override string ToString()
        {
            return string.Format("Play voice {0}", mId);
        }
    }
}
