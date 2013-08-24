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
            }
            mCrtBase[position] = new Bitmap(Utilities.FileManager.GetCharacterStreamById(baseId));
            if (baseId != id)
            {
                mCrtAdditional[position] = new Bitmap(Utilities.FileManager.GetCharacterStreamById(id));
            }
        }

        public string Title
        {
            get { return mTitle; }
            private set
            {
                mTitle = value;
                if (string.IsNullOrWhiteSpace(mTitle)) this.Text = "Display";
                else this.Text = mTitle;
            }
        }

        public DisplayForm()
        {
            Utilities.FileManager.BasePath = @"E:\Document-related\research";
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

            StartNew(@"E:\Document-related\research\download\scenario\scsc_50090501");
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
            ScenarioCommand title = mCommandList[mCursor++];
            if (!(title is TitleCommand)) // invalid
            {
                mCommandList = null;
                mCursor = -1;
                Title = null;
                return;
            }
            Title = (title as TitleCommand).Title;
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
            g.Clear(Color.Black);
            g.DrawString(mTitle, mContextFont, Brushes.White, new RectangleF(.0f, .0f, 960.0f, 320.0f),
                new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far });
            g.DrawString("클릭해서 넘기세요", mNameFont, Brushes.White, new RectangleF(.0f, 320.0f, 960.0f, 320.0f),
                new StringFormat() { Alignment = StringAlignment.Center });
            if (mBG != -1)
            {
                g.DrawImageUnscaled(mBGImage, 0, 0); // bg draw
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
            NextConversation();
            Invalidate();
        }
    }
}
