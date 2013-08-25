using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Vbc.MA.Scenario.Core;

namespace Vbc.MA.Scenario.UI
{
    public partial class DisplayForm : Form
    {
        private Font mNameFont;
        private Font mContextFont;

        private ScenarioCommand[] mCommandList;
        private int mCursor;
        private string mTitle;

        private string[] mIds;
        private int[] mIdLastUsed;
        private int mBG;
        private Color mBGColor;
        private Image[] mCrtBase;
        private Image[] mCrtAdditional;
        private Image mBGImage;

        private string mName;
        private string mContext;

        private int BG
        {
            get { return mBG; }
            set
            {
                mBG = value;
                if (mBGImage != null)
                {
                    mBGImage.Dispose();
                    mBGImage = null;
                }
                if (mBG != -1) mBGImage = new Bitmap(Utilities.FileManager.GetBackgroundStreamById(mBG));
            }
        }

        private void SetCharacterId(int position, string id)
        {
            if (mCrtBase[position] != null) mCrtBase[position].Dispose();
            if (mCrtAdditional[position] != null) mCrtAdditional[position].Dispose();
            mCrtBase[position] = null;
            mCrtAdditional[position] = null;
            mIds[position] = id;
            for (int i = 0; i < 8; i++) mIdLastUsed[i]++;
            mIdLastUsed[position] = 0;
            if (id == null) return;

            string[] idSplit = id.Split('_');
            string baseId = idSplit[0];
            if (idSplit.Length > 1)
            {
                if (idSplit[1] != "1") baseId = string.Format("{0}_{1}_1", idSplit[0], idSplit[1]);
                if (idSplit[1] == "1" && idSplit[2] == "1") id = baseId;
            }
            try
            {
                mCrtBase[position] = new Bitmap(Utilities.FileManager.GetCharacterStreamById(baseId));
            }
            catch (Exception)
            {
                mCrtBase[position] = null;
            }
            if (baseId != id)
            {
                try
                {
                    mCrtAdditional[position] = new Bitmap(Utilities.FileManager.GetCharacterStreamById(id));
                }
                catch (Exception)
                {
                    mCrtBase[position] = null;
                }
            }
        }

        public string Title
        {
            get { return mTitle; }
            private set
            {
                mTitle = value;
                if (string.IsNullOrWhiteSpace(mTitle)) this.Text = "<제목 없음>";
                else this.Text = mTitle;
            }
        }

        public DisplayForm()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "(SD 카드)/Android/data/com.square_enix.million_kr/files/save/ 디렉터리를 골라 주세요.";
            string[] roots = Directory.GetLogicalDrives();
            foreach (string root in roots)
            {
                string tpath = Path.Combine(root, @"Android\data\com.square_enix.million_kr\files\save");
                if (Directory.Exists(tpath))
                {
                    fbd.SelectedPath = tpath;
                    break;
                }
            }
            fbd.ShowDialog();
            Utilities.FileManager.BasePath = fbd.SelectedPath;
            if (!Utilities.FileManager.IsValidPath)
            {
            }

            InitializeComponent();
            mNameFont = new Font("나눔고딕", 20);
            mContextFont = new Font("나눔고딕", 28);
            mCommandList = null;
            mCursor = -1;
            Title = null;
            mIds = new string[8];
            mIdLastUsed = new int[8];
            mCrtBase = new Image[8];
            mCrtAdditional = new Image[8];
            mBG = -1;
            mBGImage = null;

            StartNew();
        }

        public void StartNew()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "모든 파일|*";
            ofd.Multiselect = false;
            ofd.InitialDirectory = Path.Combine(Utilities.FileManager.BasePath, @"download\scenario");
            ofd.ShowDialog();
            StartNew(ofd.FileName);
        }

        public void StartNew(string path)
        {
            Stream stream = new Utilities.DecryptStream(File.OpenRead(path));
            mCommandList = ScenarioParser.Parse(stream);
            mCursor = 0;
            mName = string.Empty;
            mContext = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                mIds[i] = null;
                if (mCrtBase[i] != null) mCrtBase[i].Dispose();
                if (mCrtAdditional[i] != null) mCrtAdditional[i].Dispose();
                mCrtBase[i] = null;
                mCrtAdditional[i] = null;
            }
            mBG = -1;
            mBGColor = Color.Black;
            ScenarioCommand title = mCommandList[mCursor++];
            if (!(title is TitleCommand)) // no title
            {
                mCursor = 0;
                Title = null;
                return;
            }
            Title = (title as TitleCommand).Title;
            Invalidate();
        }

        private ScenarioCommand ExecuteNext()
        {
            if (mCommandList.Length <= mCursor) return null;
            ScenarioCommand command = mCommandList[mCursor++];
            if (command is ConversationCommand)
            {
                ConversationCommand conv = command as ConversationCommand;
                mName = conv.Name;
                mContext = conv.Context;
            }
            else if (command is CharacterCommand)
            {
                CharacterCommand crt = command as CharacterCommand;
                SetCharacterId(crt.Position, crt.Id);
            }
            else if (command is BackgroundCommand)
            {
                BackgroundCommand bg = command as BackgroundCommand;
                BG = bg.Id;
                mBGColor = bg.BackgroundColor;
            }
            return command;
        }

        public ScenarioCommand NextConversation()
        {
            while (true)
            {
                ScenarioCommand cmd = ExecuteNext();
                if (cmd == null) break;
                if (cmd is ConversationCommand) return cmd;
            }
            return null;
        }

        private void Background_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush contextAreaBrush = new SolidBrush(Color.FromArgb(192, Color.Black));
            g.Clear(Color.White);
            if (mCursor > 1)
            {
                if (mBG != -1)
                {
                    g.DrawImageUnscaled(mBGImage, 0, 0); // bg draw
                }
                else // colored bg
                {
                    Brush backgroundColorBrush = new SolidBrush(mBGColor);
                    g.FillRectangle(backgroundColorBrush, new Rectangle(0, 0, 960, 640));
                    backgroundColorBrush.Dispose();
                }
            }
            else
            {
                g.DrawString(mTitle, mContextFont, Brushes.Black, new RectangleF(.0f, .0f, 960.0f, 320.0f),
                new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far });
                g.DrawString("클릭해서 넘기세요", mNameFont, Brushes.Black, new RectangleF(.0f, 320.0f, 960.0f, 320.0f),
                    new StringFormat() { Alignment = StringAlignment.Center });
            }
            // sorting
            int[] renderRank = new int[8];
            for (int i = 1; i < 8; i++)
            {
                renderRank[i] = i;
                int p = i;
                while (p > 1)
                {
                    int front = renderRank[p - 1];
                    if (mIdLastUsed[front] < mIdLastUsed[i])
                    {
                        int t = renderRank[p - 1];
                        renderRank[p - 1] = renderRank[p];
                        renderRank[p] = t;
                        p--;
                    }
                    else break;
                }
            }
            for (int i = 1; i < 8; i++)
            {
                int n = renderRank[i];
                int xpos = -480 + n * 120;
                if (mIds[n] == null) continue;
                if (mCrtBase[n] != null) g.DrawImage(mCrtBase[n], new Rectangle(xpos, 0, 960, 640));
                if (mCrtAdditional[n] != null) g.DrawImage(mCrtAdditional[n], new Rectangle(xpos, 0, 960, 640));
            }
            if (!(string.IsNullOrWhiteSpace(mName) && string.IsNullOrWhiteSpace(mContext)))
            {
                g.FillRectangle(contextAreaBrush, 0, 420, 960, 40);
                g.DrawString(mName, mNameFont, Brushes.White, 12.0f, 423.0f);
                g.FillRectangle(contextAreaBrush, 0, 460, 960, 180);
                g.DrawLine(Pens.Gold, 0, 460, 960, 460);
                string[] contexts = mContext.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                int ypos = 470;
                foreach (string context in contexts)
                {
                    g.DrawString(context, mContextFont, Brushes.White, 12.0f, ypos);
                    ypos += 60;
                }
            }
            contextAreaBrush.Dispose();
        }

        private void DisplayForm_MouseDown(object sender, MouseEventArgs e)
        {
            object ret = NextConversation();
            Invalidate();
            if (ret == null) StartNew();
        }
    }
}
