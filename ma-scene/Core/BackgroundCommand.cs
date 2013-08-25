using System.Drawing;

namespace Vbc.MA.Scenario.Core
{
    /// <summary>
    /// 배경 이미지 커맨드의 내용을 담는 클래스입니다.
    /// </summary>
    public class BackgroundCommand : ScenarioCommand
    {
        private int mId;
        private Color mColor;

        /// <summary>
        /// 배경 이미지 ID를 가져옵니다. 이미지를 사용하지 않는 경우 -1입니다.
        /// </summary>
        public int Id
        {
            get { return mId; }
        }

        /// <summary>
        /// 배경 색을 가져옵니다.
        /// </summary>
        public Color BackgroundColor
        {
            get { return mColor; }
        }

        /// <summary>
        /// 검정색 배경으로 새 인스턴스를 초기화합니다.
        /// </summary>
        public BackgroundCommand()
            : base(CommandType.Background)
        {
            mId = -1;
            mColor = Color.Black;
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

        /// <summary>
        /// 배경 색으로 새 인스턴스를 초기화합니다.
        /// </summary>
        /// <param name="color">배경 색입니다.</param>
        public BackgroundCommand(Color color)
            : base(CommandType.Background)
        {
            mId = -1;
            mColor = color;
        }

        public override string ToString()
        {
            if (mId != -1) return string.Format("Background #{0}", mId);
            else
            {
                if (mColor == Color.Black) return "Empty background";
                else return string.Format("Colored background (a:{0}, r:{1}, g:{2}, b:{3})", mColor.A, mColor.R, mColor.G, mColor.B);
            }
        }

        public override string Raw
        {
            get
            {
                if (mId != -1) return string.Format("BG {0}", mId);
                else
                {
                    if (mColor == Color.Black) return "BG ";
                    else return string.Format("BG color={0},{1},{2},{3}", mColor.A, mColor.R, mColor.G, mColor.B);
                }
            }
        }
    }
}
