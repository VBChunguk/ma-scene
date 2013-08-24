namespace Vbc.MA.Scenario.Core
{
    /// <summary>
    /// 캐릭터 이미지 커맨드의 내용을 담는 클래스입니다.
    /// </summary>
    public class CharacterCommand : ScenarioCommand
    {
        private int mPosition;
        private string mId;

        /// <summary>
        /// 캐릭터를 표시할 위치를 가져옵니다.
        /// </summary>
        public int Position
        {
            get { return mPosition; }
        }
        /// <summary>
        /// 캐릭터 이미지의 ID를 가져옵니다.
        /// </summary>
        public string Id
        {
            get { return mId; }
        }

        /// <summary>
        /// 캐릭터 위치와 ID로 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="position">캐릭터를 표시할 위치입니다.</param>
        /// <param name="id">캐릭터 이미지의 ID입니다.</param>
        public CharacterCommand(int position, string id)
            : base(CommandType.Character)
        {
            mPosition = position;
            mId = id;
        }

        public override string ToString()
        {
            return string.Format("Character ID {0} at position #{1}", mId, mPosition);
        }
    }
}
