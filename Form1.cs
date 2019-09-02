using G1TConverter.Formats;
using System;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK.Platform;
using System.IO;

namespace G1TConverter
{
    public partial class Form1 : Form
    {
        

        ContextMenu textureMenu = new ContextMenu();

        private G1T currentG1T;

        public Form1()
        {
            InitializeComponent();
            CreateOpenGLContext();
            SetUpContextMenu();
        }

        private void CreateOpenGLContext()
        {
            IWindowInfo window = Utilities.CreateWindowsWindowInfo(this.Handle);
            IGraphicsContext context = new GraphicsContext(GraphicsMode.Default, window);
            context.MakeCurrent(window);
            context.LoadAll();
        }

        private void SetUpContextMenu()
        {
            SetUpTextureContextMenu();
        }

        private void SetUpTextureContextMenu()
        {
            MenuItem replace = new MenuItem("Replace");
            replace.Click += Replace_Click;
            textureMenu.MenuItems.Add(replace);
        }

        private void Replace_Click(object sender, EventArgs e)
        {
            using (var opendialog = new OpenFileDialog())
            {
                opendialog.Filter =
                    "DirectDraw Surface|*.dds";

                opendialog.Multiselect = false; //For now at least, only one file at a time

                if (opendialog.ShowDialog() == DialogResult.OK)
                {
                    DDS temp = new DDS();
                    temp.Read(opendialog.FileName);

                    G1Texture texture = (G1Texture)textureListBox.SelectedItem;
                    texture.Replace(temp);
                    pictureBox1.Image = texture.Mipmap.GetBitmap();
                }
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var opendialog = new OpenFileDialog())
            {
                opendialog.Filter =
                    "Koei Tecmo Texture Archive|*.g1t|" +
                    "Binary file|*.bin|" +
                    "All files(*.*)|*.*";

                opendialog.Multiselect = false; //For now at least, only one file at a time

                if (opendialog.ShowDialog() == DialogResult.OK)
                {
                    textureListBox.Items.Clear();
                    OpenFile(opendialog.FileName);
                }
            }
        }

        private void OpenFile(string filename)
        {
            switch(Path.GetExtension(filename))
            {
                case ".bin":
                case ".g1t":
                    {
                        currentG1T = new G1T();
                        currentG1T.Read(filename);

                        foreach (G1Texture tex in currentG1T.Textures)
                        {
                            textureListBox.Items.Add(tex);
                        }
                    }
                    break;
                case ".dds":
                    {
                        DDS temp = new DDS();
                        temp.Read(filename);
                        pictureBox1.Image = temp.Texture.GetBitmap();
                    }
                    break;
                default:
                    {
                        MessageBox.Show("This file format is not supported yet.", "Unsupported file format");
                    }
                    break;
            }
            
        }

        private void TextureListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textureListBox.SelectedIndex >= 0)
            {
                G1Texture tex = (G1Texture)textureListBox.SelectedItem;
                tex.Mipmap.Bind();
                widthLabel.Text = $"Width: {tex.Mipmap.Width}";
                heightLabel.Text = $"Height: {tex.Mipmap.Height}";
                numericUpDownMipMap.Value = tex.MipMapCount;

                if (tex.NormalMapFlags != 0)
                    checkBoxNormalMap.Checked = true;
                else
                    checkBoxNormalMap.Checked = false;

                checkBoxExHeader.Checked = tex.UsesExtraHeader;
                checkBoxNormalMap.Text = $"Normal map ({tex.NormalMapFlags:X})";
                pictureBox1.Image = tex.Mipmap.GetBitmap();

                numericUpDownMipMap.Enabled = true;
                checkBoxNormalMap.Enabled = true;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using (var savedialog = new SaveFileDialog())
            {
                savedialog.DefaultExt = ".png";
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image.Save(savedialog.FileName, ImageFormat.Png);
                }
            }
        }

        private void TextureListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int indexitem = textureListBox.IndexFromPoint(e.X, e.Y);
                if (indexitem == -1)
                {
                }
                else if (textureListBox.Items[indexitem] is G1Texture)
                {
                    textureListBox.SelectedIndex = indexitem;
                    textureMenu.Show(this, new System.Drawing.Point(e.X, e.Y));
                }
            }
        }

        

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var savedialog = new SaveFileDialog())
            {
                savedialog.DefaultExt = ".g1t";
                savedialog.Filter = "Koei Tecmo Texture Archive|*.g1t";

                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    G1T file = new G1T();

                    foreach(G1Texture texture in textureListBox.Items)
                    {
                        file.AddTexture(texture);
                    }

                    file.Write(savedialog.FileName);
                }
            }
        }

        private void NumericUpDownMipMap_ValueChanged(object sender, EventArgs e)
        {
            G1Texture tex = (G1Texture)textureListBox.SelectedItem;
            tex.MipMapCount = (byte)numericUpDownMipMap.Value;
        }

        private void CheckBoxNormalMap_CheckedChanged(object sender, EventArgs e)
        {
            G1Texture tex = (G1Texture)textureListBox.SelectedItem;
            if (checkBoxNormalMap.Checked)
                tex.NormalMapFlags = 3;
            else
                tex.NormalMapFlags = 0;
        }
    }
}
