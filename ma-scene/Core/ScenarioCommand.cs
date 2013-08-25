namespace Vbc.MA.Scenario.Core
{
    /// <summary>
    /// 시나리오 커맨드의 종류를 나타냅니다.
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// 제목을 나타냅니다.
        /// </summary>
        Title,
        /// <summary>
        /// 배경 이미지를 나타냅니다.
        /// </summary>
        Background,
        /// <summary>
        /// 음악을 나타냅니다.
        /// </summary>
        Music,
        /// <summary>
        /// 사운드 이펙트를 나타냅니다.
        /// </summary>
        SoundEffect,
        /// <summary>
        /// 이펙트를 나타냅니다.
        /// </summary>
        Effect,
        /// <summary>
        /// 대화를 나타냅니다.
        /// </summary>
        Conversation,
        /// <summary>
        /// 캐릭터 이미지를 나타냅니다.
        /// </summary>
        Character,
        /// <summary>
        /// 보이스를 나타냅니다.
        /// </summary>
        Voice,
        /// <summary>
        /// 알 수 없는 명령을 나타냅니다.
        /// </summary>
        Unknown,
    }

    /// <summary>
    /// 시나리오 커맨드의 기본 클래스입니다.
    /// </summary>
    public abstract class ScenarioCommand
    {
        protected CommandType mCommandType;

        /// <summary>
        /// 사용되지 않습니다.
        /// </summary>
        private ScenarioCommand()
        {
            mCommandType = CommandType.Unknown;
        }

        /// <summary>
        /// 커맨드의 종류로 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="type">시나리오 커맨드의 종류입니다.</param>
        protected ScenarioCommand(CommandType type)
        {
            mCommandType = type;
        }

        /// <summary>
        /// 처리되지 않은 데이터 원본을 가져옵니다.
        /// </summary>
        public abstract string Raw
        {
            get;
        }

        /// <summary>
        /// 커맨드의 종류를 가져옵니다.
        /// </summary>
        public CommandType Type
        {
            get { return mCommandType; }
        }

        public override string ToString()
        {
            return Raw;
        }
    }
}
