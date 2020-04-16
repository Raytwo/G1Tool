using G1Tool.Formats;
using System;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Platform;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

namespace G1Tool
{
    public partial class Form1 : Form
    {


        ContextMenu g1tMenu = new ContextMenu();
        ContextMenu textureMenu = new ContextMenu();

        private G1T currentG1T;
        private KTBin BinFile { get; set; }
        private int FormatIndex { get; set; }
        private ContextMenu ContextMenuDDS { get; set; }
        private ContextMenu ContextMenuG1T { get; set; }
        private new List<List<G1T>> BinFileList { get; set; }   // I hate KT
        private List<List<G1Texture>> G1TFileList { get; set; }
        private List<string> FilePathBinGZList { get; set; }
        private List<string> FilePathBinList { get; set; }
        private List<string> FilePathG1TList { get; set; }
        private int NodeIndexBin { get; set; }
        private int NodeIndexG1T { get; set; }

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
            /* using (var opendialog = new openFileDialog())
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
             */
        }

        private void SetUpTextureContextMenu()
        {
            MenuItem replaceDDS = new MenuItem("Replace DDS");
            replaceDDS.Click += ReplaceDDS_Click;
            ContextMenuDDS = new ContextMenu();
            ContextMenuDDS.MenuItems.Add(replaceDDS);

            MenuItem replaceG1T = new MenuItem("Replace G1T");
            replaceG1T.Click += ReplaceG1T_Click;
            ContextMenuG1T = new ContextMenu();
            ContextMenuG1T.MenuItems.Add(replaceG1T);
            //     MenuItem remove = new MenuItem("Remove");
            //    MenuItem exportPNG = new MenuItem("Export to PNG");

            //     remove.Click += Remove_Click;
            //     exportPNG.Click += ExportPNG_Click;

            //  cmDatabase.MenuItems.Add(replaceG1T);
            //  treeView1.ContextMenu = cmDatabase;
            //   treeView1.ContextMenu.MenuItems.Add(remove);
            // treeView1.ContextMenu.MenuItems.Add(exportPNG);
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
            //    textureListBox.Items.Remove(textureListBox.SelectedItem);
        }

        private void ReplaceDDS_Click(object sender, EventArgs e)
        {
            using (var opendialog = new OpenFileDialog())
            {
                opendialog.Filter =
                    "DirectDraw Surface|*.dds";

                if (opendialog.ShowDialog() == DialogResult.OK)
                {
                    G1Texture texture = new G1Texture();
                    DDS newDDS = new DDS();
                    newDDS.Read(opendialog.FileName);

                    if (newDDS.MipMapCount >= 10)
                    {
                        MessageBox.Show("The amount of mipmaps for this texture is too big. Reducing it to 9.");
                        newDDS.MipMapCount = 9;
                    }

                    if (FormatIndex == NodeIndexBin && treeView1.Nodes.ContainsKey("BIN"))
                    {
                        //        texture = BinFileList[treeView1.SelectedNode.Parent.Parent.Index][treeView1.SelectedNode.Parent.Index][treeView1.SelectedNode.Index];
                        texture.Replace(newDDS);
                    }
                    if (FormatIndex == NodeIndexG1T && treeView1.Nodes.ContainsKey("G1T"))
                    {
                        texture = G1TFileList[treeView1.SelectedNode.Parent.Index][treeView1.SelectedNode.Index];
                        texture.Replace(newDDS);
                    }
                    pictureBox1.Image = texture.Mipmap.GetBitmap();
                }
            }
        }

        private void ReplaceG1T_Click(object sender, EventArgs e)
        {
            using (var opendialog = new OpenFileDialog())
            {
                opendialog.Filter =
                    "Koei Tecmo texture archive|*.g1t";

                if (opendialog.ShowDialog() == DialogResult.OK)
                {
                    using (var fs = new FileStream(opendialog.FileName, FileMode.Open))
                    {
                        int fileSize = (int)new FileInfo(opendialog.FileName).Length;
                        byte[] buffer = new byte[fileSize];
                        fs.Read(buffer, 0, fileSize);

                        var newG1T = new G1T();
                        newG1T.Read(buffer);

                        treeView1.SelectedNode.Nodes.Clear();
                        for (int i = 0; i < newG1T.Textures.Count; i++)
                        {
                            treeView1.SelectedNode.Nodes.Add(i.ToString() + ".dds");
                        }
                        if (FormatIndex == NodeIndexBin && treeView1.Nodes.ContainsKey("BIN"))
                        {
                            //           BinFileList[treeView1.SelectedNode.Parent.Index][treeView1.SelectedNode.Index] = newG1T.Textures;
                            LoadImage(BinFileList[treeView1.SelectedNode.Parent.Index][treeView1.SelectedNode.Index].Textures[treeView1.SelectedNode.FirstNode.Index]);
                        }
                        if (FormatIndex == NodeIndexG1T && treeView1.Nodes.ContainsKey("G1T"))
                        {
                            G1TFileList[treeView1.SelectedNode.Parent.Index] = newG1T.Textures;
                            LoadImage(G1TFileList[treeView1.SelectedNode.Index][treeView1.SelectedNode.FirstNode.Index]);
                        }
                    }
                }
            }
        }
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView1.SelectedNode = e.Node;
            }
            if (FormatIndex == NodeIndexBin && treeView1.Nodes.ContainsKey("BIN"))
            {

                if (e.Node.Level == 2)  // G1T
                {
                    e.Node.ContextMenu = ContextMenuG1T;
                }
                if (e.Node.Level == 3)  // DDS
                {
                    e.Node.ContextMenu = ContextMenuDDS;
                }
            }
            if (FormatIndex == NodeIndexG1T && treeView1.Nodes.ContainsKey("G1T"))
            {
                if (e.Node.Level == 1)  // G1T
                {
                    e.Node.ContextMenu = ContextMenuG1T;
                }
                if (e.Node.Level == 2)  // DDS
                {
                    e.Node.ContextMenu = ContextMenuDDS;
                }
            }

        }
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter =
                    "Koei Tecmo texture archive|*.g1t|" +
                    "Koei Tecmo binary archive|*.bin|" +
                    "Compressed Koei Tecmo texture archive|*.g1t.gz|" +
                    "DirectDraw Surface|*.dds|" +
                    "All supported files|*.g1t*;*.bin;*.g1t.gz;*.dds";
                openFileDialog.FilterIndex = 5;
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FilePathBinGZList = new List<string>();
                    FilePathBinList = new List<string>();
                    FilePathG1TList = new List<string>();
                    treeView1.Nodes.Clear();
                    pictureBox1.Image = null;
                    for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                    {
                        switch (Path.GetExtension(openFileDialog.FileNames[i]))
                        {
                            case (".bin"):
                                {
                                    if (!treeView1.Nodes.ContainsKey("BIN"))
                                    {
                                        treeView1.Nodes.Add(new TreeNode("BIN") { Name = "BIN" });
                                        NodeIndexBin = treeView1.Nodes.IndexOfKey("BIN");
                                    }
                                    FilePathBinGZList.Add(openFileDialog.FileNames[i]);
                                }
                                break;
                            case (".g1t"):
                                {
                                    if (!treeView1.Nodes.ContainsKey("G1T"))
                                    {
                                        treeView1.Nodes.Add(new TreeNode("G1T") { Name = "G1T" });
                                        NodeIndexG1T = treeView1.Nodes.IndexOfKey("G1T");
                                    }
                                    FilePathG1TList.Add(openFileDialog.FileNames[i]);
                                }
                                break;
                            case (".dds"):
                                {

                                }
                                break;
                        }
                    }
                    OpenFile();
                }
            }
        }

        private void OpenFile()
        {
            BinFileList = new List<List<G1T>>();
            G1TFileList = new List<List<G1Texture>>();
            var imageList = new ImageList();
            imageList.ColorDepth = ColorDepth.Depth32Bit;

            #region BIN
            for (int i = 0; i < FilePathBinGZList.Count; i++)
            {
                imageList.Images.Add(i.ToString(), Icon.ExtractAssociatedIcon(FilePathBinGZList[i]));
                var node = new TreeNode()
                {
                    Text = Path.GetFileName(FilePathBinGZList[i]),
                    ImageKey = i.ToString(),
                    SelectedImageKey = i.ToString(),
                };

                treeView1.Nodes[NodeIndexBin].Nodes.Add(node);
                BinFile = new KTBin();
                var binEntries = new List<G1T>();
                var ktGZip = new KTGZip();
                var dummyEntry = new G1T();

                BinFile.ReadKTBin(FilePathBinGZList[i]);
                for (int ii = 0; ii < BinFile.FileList.Count; ii++)
                {
                    if (BinFile.FileList[ii].Length != 0)
                    {
                        #region Compressed Entries
                        try
                        {
                            int magicID = BitConverter.ToInt32(ktGZip.Decompress(BinFile.FileList[ii]), 0x0);
                            switch (magicID)
                            {
                                case (0x47314D5F):  // G1M_
                                    {
                                        treeView1.Nodes[NodeIndexBin].Nodes[i].Nodes.Add(new TreeNode(ii.ToString() + ".g1m"));
                                        binEntries.Add(dummyEntry);
                                    }
                                    break;
                                case (0x47315447):  // G1TG
                                    {
                                        treeView1.Nodes[NodeIndexBin].Nodes[i].Nodes.Add(new TreeNode(ii.ToString() + ".g1t"));

                                        var g1t = new G1T();
                                        g1t.Read(ktGZip.Decompress(BinFile.FileList[ii]));
                                        for (int iii = 0; iii < g1t.Textures.Count; iii++)
                                        {
                                            treeView1.Nodes[NodeIndexBin].Nodes[i].Nodes[ii].Nodes.Add(iii.ToString() + ".dds");
                                        }
                                        binEntries.Add(g1t);
                                    }
                                    break;
                                default:
                                    {
                                        treeView1.Nodes[NodeIndexBin].Nodes[i].Nodes.Add(new TreeNode(ii.ToString() + ".bin"));
                                        binEntries.Add(dummyEntry);
                                    }
                                    break;
                            }
                        }
                        #endregion
                        // Need a new list to make sure to not compress when saving
                        #region Uncompressed Entries
                        catch (Exception)
                        {
                            int magicID = BitConverter.ToInt32(BinFile.FileList[ii], 0x0);
                            switch (magicID)
                            {
                                case (0x47314D5F):  // G1M_
                                    {
                                        treeView1.Nodes[NodeIndexBin].Nodes[i].Nodes.Add(new TreeNode(ii.ToString() + ".g1m"));
                                        binEntries.Add(dummyEntry);
                                    }
                                    break;
                                case (0x47315447):  // G1TG
                                    {
                                        var g1tList = new List<List<G1Texture>>();
                                        treeView1.Nodes[NodeIndexBin].Nodes[i].Nodes.Add(new TreeNode(ii.ToString() + ".g1t"));

                                        var g1t = new G1T();
                                        g1t.Read(BinFile.FileList[ii]);
                                        for (int iii = 0; iii < g1t.Textures.Count; iii++)
                                        {
                                            treeView1.Nodes[NodeIndexBin].Nodes[i].Nodes[ii].Nodes.Add(iii.ToString() + ".dds");
                                        }
                                        binEntries.Add(g1t);
                                    }
                                    break;
                                default:
                                    {
                                        treeView1.Nodes[NodeIndexBin].Nodes[i].Nodes.Add(new TreeNode(ii.ToString() + ".bin"));
                                        binEntries.Add(dummyEntry);
                                    }
                                    break;
                            }
                            // New List for bin with uncompressed files
                            if (!FilePathBinList.Contains(FilePathBinGZList[i]))
                            {
                                FilePathBinList.Add(FilePathBinGZList[i]);
                            }
                        }
                        #endregion
                    }
                    #region Dummy Entries
                    else
                    {
                        treeView1.Nodes[NodeIndexBin].Nodes[i].Nodes.Add(new TreeNode(ii.ToString() + ".bin"));
                        binEntries.Add(dummyEntry);
                    }
                }
                #endregion
                BinFileList.Add(binEntries);
            }
            // Remove bin with uncompressed files from binGZ
            for (int i = 0; i < FilePathBinList.Count; i++)
            {
                FilePathBinGZList.RemoveAt(FilePathBinGZList.IndexOf(FilePathBinList[i]));
            }
            #endregion
            #region G1T
            for (int i = 0; i < FilePathG1TList.Count; i++)
            {
                imageList.Images.Add(i.ToString(), Icon.ExtractAssociatedIcon(FilePathG1TList[i]));
                var node = new TreeNode()
                {
                    Text = Path.GetFileName(FilePathG1TList[i]),
                    ImageKey = i.ToString(),
                    SelectedImageKey = i.ToString(),
                };

                treeView1.Nodes[NodeIndexG1T].Nodes.Add(node);
                using (var fs = new FileStream(FilePathG1TList[i], FileMode.Open))
                {
                    int fileSize = (int)new FileInfo(FilePathG1TList[i]).Length;
                    byte[] buffer = new byte[fileSize];
                    fs.Read(buffer, 0, fileSize);

                    currentG1T = new G1T();
                    currentG1T.Read(buffer);

                    for (int ii = 0; ii < currentG1T.Textures.Count; ii++)
                    {
                        treeView1.Nodes[NodeIndexG1T].Nodes[i].Nodes.Add(new TreeNode(ii.ToString() + ".dds"));
                    }
                }
                G1TFileList.Add(currentG1T.Textures);
            }
            #endregion
            /*
            switch (Path.GetExtension(openFileDialog.FileNames[i]))
            {
                #region GZ
                case ".gz":
                    {
                        using (var fs = new FileStream(openFileDialog.FileNames[i], FileMode.Open))
                        {
                            var ktGZip = new KTGZip();
                            int fileSize = (int)new FileInfo(openFileDialog.FileNames[i]).Length;
                            byte[] buffer = new byte[fileSize];
                            fs.Read(buffer, 0, fileSize);

                            currentG1T = new G1T();
                            currentG1T.Read(ktGZip.Decompress(buffer));
                        }
                        var list = new List<string>();
                        list.Add(Path.GetFileNameWithoutExtension(openFileDialog.FileNames[i]));

                    }
                    break;
                #endregion
                #region DDS
                case ".dds":
                    {
                        treeView1.Nodes[3].Nodes.Add(node);

                        DDS temp = new DDS();
                        temp.Read(openFileDialog.FileNames[i]);
                        pictureBox1.Image = temp.Texture.GetBitmap();
                    }
                    break;
            }
            #endregion
            */
            treeView1.ImageList = imageList;
        }
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private object GetComboBoxItemByValue(byte internalFormat)
        {
            foreach (ComboBoxItem item in comboBoxCompression.Items)
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
            /*
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
            */
        }



        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (var savedialog = new SaveFileDialog())
            {
                savedialog.DefaultExt = ".bin";
                savedialog.Filter = "Koei Tecmo Texture Archive|*.bin";

                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    var fileList = new List<byte[]>();
                    var ktGZIP = new KTGZip();
                    for (int i = 0; i < BinFileList[treeView1.SelectedNode.Index].Count; i++)    // For Each G1T file of the selected bin file
                    {
                        fileList.Add(ktGZIP.Compress(BinFileList[treeView1.SelectedNode.Index][i].Write()));
                    }
                    BinFile.Write(fileList, FilePathBinGZList[treeView1.SelectedNode.Index]);
                }
            }
        }

        private void NumericUpDownMipMap_ValueChanged(object sender, EventArgs e)
        {
            /*
            G1Texture tex = currentG1T.Textures[textureListBox.SelectedIndex];
            tex.MipMapCount = (byte)numericUpDownMipMap.Value;
            */
        }

        private void CheckBoxNormalMap_CheckedChanged(object sender, EventArgs e)
        {
            /*
            G1Texture tex = currentG1T.Textures[textureListBox.SelectedIndex];
            if (checkBoxNormalMap.Checked)
                tex.NormalMapFlags = 3;
            else
                tex.NormalMapFlags = 0;
                */
        }

        private void ComboBoxCompression_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
                G1Texture tex = currentG1T.Textures[textureListBox.SelectedIndex];
                tex.pixelInternalFormat = G1Texture.GetPixelInternalFormatForTextures((byte)((ComboBoxItem)comboBoxCompression.SelectedItem).Value);
            */
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var nodeRoot = treeView1.SelectedNode;

            while (nodeRoot.Parent != null)
            {
                nodeRoot = nodeRoot.Parent;
            }
            if (FormatIndex != nodeRoot.Index)
            {
                FormatIndex = nodeRoot.Index;
            }
            if (treeView1.SelectedNode.Level == 0)
            {
                pictureBox1.Image = null;
            }

            if (FormatIndex == NodeIndexBin && treeView1.Nodes.ContainsKey("BIN"))
            {
                if (treeView1.SelectedNode.Level == 1)
                {
                    pictureBox1.Image = null;
                }
                if (treeView1.SelectedNode.Level == 2)
                {
                    if (Path.GetExtension(treeView1.SelectedNode.Text) == ".g1t")
                    {
                        LoadImage(BinFileList[treeView1.SelectedNode.Parent.Index][treeView1.SelectedNode.Index].Textures[treeView1.SelectedNode.FirstNode.Index]);
                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }
                }
                if (treeView1.SelectedNode.Level == 3)
                {
                    LoadImage(BinFileList[treeView1.SelectedNode.Parent.Parent.Index][treeView1.SelectedNode.Parent.Index].Textures[treeView1.SelectedNode.Index]);
                }
            }

            if (FormatIndex == NodeIndexG1T && treeView1.Nodes.ContainsKey("G1T"))
            {
                if (treeView1.SelectedNode.Level == 1)
                {
                    LoadImage(G1TFileList[treeView1.SelectedNode.Index][treeView1.SelectedNode.FirstNode.Index]);

                }
                if (treeView1.SelectedNode.Level == 2)
                {
                    LoadImage(G1TFileList[treeView1.SelectedNode.Parent.Index][treeView1.SelectedNode.Index]);
                }
            }
        }
        /*
        switch (Path.GetExtension(openFileDialog.FileNames[FormatIndex])) // FileIndex
        {
            case ".bin":
                {
                    if (Path.GetExtension(treeView1.SelectedNode.Text) == ".g1t")
                    {
                        if (treeView1.SelectedNode.Level == 1)
                        {
                            LoadImage(BinEntries[FormatIndex][treeView1.SelectedNode.Index][treeView1.SelectedNode.FirstNode.Index][treeView1.SelectedNode.FirstNode.Index]);
                        }
                    }
                    if (treeView1.SelectedNode.Level == 2)
                    {
                        var node = treeView1.SelectedNode;
                        while (node.Parent.Level == 1)
                        {
                            node = node.Parent;
                        }
                        LoadImage(BinEntries[FormatIndex][node.Index][treeView1.SelectedNode.Index][treeView1.SelectedNode.Index]);
                    }
                }
                break;
            case ".g1t":
                {
                    if (treeView1.SelectedNode.Level == 0)
                    {
                        LoadImage(G1TEntries[FormatIndex][treeView1.SelectedNode.FirstNode.Index]);
                    }
                    if (treeView1.SelectedNode.Level == 1)
                    {
                        LoadImage(G1TEntries[FormatIndex][treeView1.SelectedNode.Index]);
                    }
                }
                break;
        } */

        private void LoadImage(G1Texture tex)
        {
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

        private void button1_Click(object sender, EventArgs e)
        {
            using (var colorDialog = new ColorDialog())
            {
                colorDialog.FullOpen = true;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    panel1.BackColor = colorDialog.Color;
                    pictureBox1.BackColor = colorDialog.Color;
                }
            }
        }
    }
}