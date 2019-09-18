using G1Tool.Formats;
using System;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK.Platform;
using System.IO;
using System.Windows.Media.Imaging;

namespace G1Tool
{
    public partial class Form1 : Form
    {


        ContextMenu g1tMenu = new ContextMenu();
        ContextMenu textureMenu = new ContextMenu();

        private G1T currentG1T;

        public Form1()
        {
            InitializeComponent();
            CreateOpenGLContext();
            SetUpContextMenu();
            PopulateCompressionComboBox();
            
        }

        public class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public ComboBoxItem(string text, int value = 0)
            {
                Text = text;
                Value = value;
            }

            public override string ToString()
            {
                return Text;
            }
        }
        private void PopulateCompressionComboBox()
        {
            comboBoxCompression.Items.Add(new ComboBoxItem("RGBA8", 0x1));
            comboBoxCompression.Items.Add(new ComboBoxItem("RGBA8 (Warriors Orochi 4)", 0x2));
            comboBoxCompression.Items.Add(new ComboBoxItem("DXT1 (FEW)", 0x6));
            comboBoxCompression.Items.Add(new ComboBoxItem("DXT5 (FEW)", 0x8));
            comboBoxCompression.Items.Add(new ComboBoxItem("DXT1 (3H)", 0x59));
            comboBoxCompression.Items.Add(new ComboBoxItem("DXT5 (3H)", 0x5B));
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
            SetUpG1TMenu();
            SetUpTextureContextMenu();
        }

        private void SetUpG1TMenu()
        {
            MenuItem add = new MenuItem("Add");
            add.Click += Add_Click;
            g1tMenu.MenuItems.Add(add);
        }

        private void Add_Click(object sender, EventArgs e)
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

                    if (temp.MipMapCount >= 10)
                    {
                        MessageBox.Show("The amount of mipmaps for this texture is too big. Reducing it to 9.");
                        temp.MipMapCount = 9;
                    }

                    G1Texture texture = new G1Texture();
                    texture.Replace(temp);
                    textureListBox.Items.Add(texture);
                    textureListBox.SelectedItem = texture;
                }
            }
            
        }

        private void SetUpTextureContextMenu()
        {
            MenuItem replace = new MenuItem("Replace");
            MenuItem remove = new MenuItem("Remove");
            MenuItem exportPNG = new MenuItem("Export to PNG");
            replace.Click += Replace_Click;
            remove.Click += Remove_Click;
            exportPNG.Click += ExportPNG_Click;
            textureMenu.MenuItems.Add(replace);
            textureMenu.MenuItems.Add(remove);
            textureMenu.MenuItems.Add(exportPNG);
        }

        private void ExportPNG_Click(object sender, EventArgs e)
        {
            using (var opendialog = new SaveFileDialog())
            {
                opendialog.Filter =
                    "Portable Network Graphics|*.png";

                if (opendialog.ShowDialog() == DialogResult.OK)
                {
                    switch (Path.GetExtension(opendialog.FileName))
                    {
                        case ".png":
                            {
                                pictureBox1.Image.Save(opendialog.FileName, ImageFormat.Png);
                            }
                            break;
                    }

                }
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            textureListBox.Items.Remove(textureListBox.SelectedItem);
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
                    G1Texture texture = (G1Texture)textureListBox.SelectedItem;

                    switch (Path.GetExtension(opendialog.FileName))
                    {
                        case ".dds":
                            {
                                DDS temp = new DDS();
                                temp.Read(opendialog.FileName);

                                if (temp.MipMapCount >= 10)
                                {
                                    MessageBox.Show("The amount of mipmaps for this texture is too big. Reducing it to 9.");
                                    temp.MipMapCount = 9;
                                }

                                texture.Replace(temp);
                            }
                            break;
                    }

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

                comboBoxCompression.SelectedItem = GetComboBoxItemByValue(tex.compression_format);

                numericUpDownMipMap.Enabled = true;
                checkBoxNormalMap.Enabled = true;
            }
            else
            {
                numericUpDownMipMap.Enabled = false;
                checkBoxNormalMap.Enabled = false;
            }
        }

        private object GetComboBoxItemByValue(byte internalFormat)
        {
            foreach(ComboBoxItem item in comboBoxCompression.Items)
            {
                if (item.Value == internalFormat)
                    return item;
            }

            return null;
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
                    g1tMenu.Show(this, new System.Drawing.Point(e.X, e.Y));
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
                    currentG1T.Textures.Clear();

                    foreach(G1Texture texture in textureListBox.Items)
                    {
                        currentG1T.AddTexture(texture);
                    }

                    currentG1T.Write(savedialog.FileName);
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

        private void ComboBoxCompression_SelectedIndexChanged(object sender, EventArgs e)
        {
            G1Texture tex = (G1Texture)textureListBox.SelectedItem;
            tex.pixelInternalFormat = G1Texture.GetPixelInternalFormatForTextures((byte)((ComboBoxItem)comboBoxCompression.SelectedItem).Value);
        }
    }
}
