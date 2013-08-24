namespace Vbc.MA.Scenario.Core
{
    /// <summary>
    /// 대화 커맨드의 내용을 담는 클래스입니다.
    /// </summary>
    public class ConversationCommand : ScenarioCommand
    {
        private string mName;
        private string mContext;

        /// <summary>
        /// 캐릭터의 이름을 가져옵니다.
        /// </summary>
        public string Name
        {
            get { return mName; }
        }
        /// <summary>
        /// 캐릭터의 대사를 가져옵니다.
        /// </summary>
        public string Context
        {
            get { return mContext; }
        }

        /// <summary>
        /// 캐릭터의 이름과 대사로 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="name">캐릭터의 이름입니다.</param>
        /// <param name="context">캐릭터의 대사입니다.</param>
        public ConversationCommand(string name, string context)
            : base(CommandType.Conversation)
        {
            mName = name;
            mContext = context;
        }
    }
}
