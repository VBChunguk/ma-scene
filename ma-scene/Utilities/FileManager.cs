using System.IO;

namespace Vbc.MA.Scenario.Utilities
{
    /// <summary>
    /// 밀리언아서 파일을 관리하는 정적 클래스입니다.
    /// </summary>
    public static class FileManager
    {
        private static string mBasePath = string.Empty;
        
        /// <summary>
        /// 밀리언아서 데이터 디렉터리의 경로를 가져오거나 설정합니다.
        /// </summary>
        public static string BasePath
        {
            get { return mBasePath; }
            set { mBasePath = value; }
        }

        /// <summary>
        /// adv 디렉터리의 경로를 가져옵니다.
        /// </summary>
        private static string AdvPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(BasePath)) return string.Empty;
                return Path.Combine(mBasePath, @"download\image\adv");
            }
        }

        /// <summary>
        /// 설정한 데이터 디렉터리가 유효한지의 여부를 가져옵니다.
        /// </summary>
        public static bool IsValidPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(mBasePath)) return false;
                if (!Directory.Exists(mBasePath)) return false;
                if (!Directory.Exists(AdvPath)) return false;
                return true;
            }
        }

        /// <summary>
        /// 캐릭터 이미지 ID로부터 복호화된 파일 스트림을 가져옵니다.
        /// </summary>
        /// <param name="id">캐릭터 이미지 ID입니다.</param>
        /// <returns>복호화된 파일 스트림입니다. 존재하지 않을 경우 null을 반환합니다.</returns>
        public static Stream GetCharacterStreamById(string id)
        {
            string path = Path.Combine(AdvPath, string.Format("adv_chara{0}", id));
            if (!File.Exists(path)) return null;
            return new DecryptStream(File.OpenRead(path));
        }

        /// <summary>
        /// 배경 이미지 ID로부터 복호화된 파일 스트림을 가져옵니다.
        /// </summary>
        /// <param name="id">배경 이미지 ID입니다.</param>
        /// <returns>복호화된 파일 스트림입니다. 존재하지 않을 경우 null을 반환합니다.</returns>
        public static Stream GetBackgroundStreamById(int id)
        {
            string path = Path.Combine(AdvPath, string.Format("adv_bg{0}", id));
            if (!File.Exists(path)) return null;
            return new DecryptStream(File.OpenRead(path));
        }
    }
}
