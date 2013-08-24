namespace Vbc.MA.Scenario.Core
{
    /// <summary>
    /// 배경 이미지 커맨드의 내용을 담는 클래스입니다.
    /// </summary>
    public class BackgroundCommand : ScenarioCommand
    {
        private int mId;

        /// <summary>
        /// 배경 이미지 ID를 가져옵니다.
        /// </summary>
        public int Id
        {
            get { return mId; }
        }

        /// <summary>
        /// 빈 배경으로 새 인스턴스를 초기화합니다.
        /// </summary>
        public BackgroundCommand()
            : base(CommandType.Background)
        {
            mId = -1;
        }

        /// <summary>
        /// 배경 이미지 ID로 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="id">배경 이미지 ID입니다.</param>
        public BackgroundCommand(int id)
            : base(CommandType.Background)
        {
            mId = id;
        }

        public override string ToString()
        {
            return "BG " + mId;
        }
    }
}
