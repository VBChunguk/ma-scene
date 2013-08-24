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
        private ScenarioCommand[] mCommandList;
        private int mCursor;
        private string mTitle;

        private string[] mIds;
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
            if (id == null) return;

            string baseId = id.Split('_')[0];
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
            InitializeComponent();
            mCommandList = null;
            mCursor = -1;
            Title = null;
            mIds = new string[8];
            mCrtBase = new Image[8];
            mCrtAdditional = new Image[8];
            mBG = -1;
            mBGImage = null;
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
            if (mCommandList.Length >= mCursor) return null;
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
            g.FillRegion(new SolidBrush(Color.Black), Region);
            if (mBG != -1)
            {
                g.DrawImageUnscaled(mBGImage, 0, 0); // bg draw
            }
            for (int i = 1; i < 8; i++)
            {
                int xpos = -480 + (i - 1) * 160;
                if (mIds[i] == null) continue;
                g.DrawImageUnscaled(mCrtBase[i], xpos, 0);
                g.DrawImageUnscaled(mCrtAdditional[i], xpos, 0);
            }
        }
    }
}
