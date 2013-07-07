using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MyBookshelf
{
    public partial class Form2 : Form
    {
        private string dbName = "Library";
        private string sql = "select * from Books";
        private static string consqlserver = "server=PC-201209151323;uid=sa;pwd=123;database=Library";
        private SqlConnection con = new SqlConnection(consqlserver);
        //private SqlConnection con0 = new SqlConnection("server=PC-201209151323;uid=sa;pwd=123;database=master..sysprocesses");
        private SqlCommand cmd;
        private DataSet ds;
        private DataTable dt;
        private SqlDataAdapter da;
        private SqlCommandBuilder scb;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            refresh();

            /*
            da = new SqlDataAdapter(sql, con);
            try
            {
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //将获得数据源赋予数据库控件
                    dataGridView1.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                con.Close();
                con.Dispose();
                da.Dispose();
            }
             * */
        }

        private void refresh()                                          //刷新
        {
            //ds = libraryDataSet;
            //dataGridView1.DataSource = null;
            //dataGridView1.DataSource = ds.Tables["Books"];
            //dataGridView1.DataBind();     //不可行 无DataBind方法

            //this.booksTableAdapter1.Update(this.libraryDataSet.Books);    //空值时会出错

            //this.dataGridView1.Refresh();
            dataGridView1.DataSource = booksBindingSource;
            this.booksTableAdapter1.Fill(this.libraryDataSet.Books);
        }

        private void delete()                                           //删除
        {
            DialogResult dlResult = MessageBox.Show(this, "删除记录", "请确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            if (dlResult == DialogResult.Yes)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                    {
                        if (!r.IsNewRow)
                        {
                            dataGridView1.Rows.Remove(r);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选择要删除的行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            //把删除后的DataGridView1的结果更新到数据库中
            /*
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                try
                {
                    con.Open();
                    da = new SqlDataAdapter("select * from Books", con);
                    SqlCommandBuilder scb = new SqlCommandBuilder(da);
                    da.Fill(ds, "Books");
                    da.Update(dt);
                    con.Close();
                }
                catch (DataException de)
                {
                    MessageBox.Show("删除出错！");
                }
            }
             * */
        }

        private void delAll()                                           //清空
        {
            DialogResult dlResult = MessageBox.Show(this, "要删除全部记录", "请确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            if (dlResult == DialogResult.Yes)
            {
                //this.dataGridView1.DataSource = dt;
                //这个for语句比较么效率！！！
                for (int i = dataGridView1.RowCount - 2; i >= 0; i--)
                {
                    dataGridView1.Rows.Remove(dataGridView1.Rows[i]);
                }
            }
            /*
            DialogResult dlResult = MessageBox.Show(this, "要删除全部记录", "请确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign);
            if (dlResult == DialogResult.Yes)
            {
                try
                {
                    con = new SqlConnection(consqlserver);
                    con.Open();
                    cmd = con.CreateCommand();
                    cmd.CommandText = "delete Books";
                    cmd.ExecuteNonQuery();

                    //dataGridView1.Rows.Clear();  //出错，不能清除此列表

                    while (this.dataGridView1.Rows.Count != 1)  //不能=0，提示：无法删除未提交的新行
                    {
                        this.dataGridView1.Rows.RemoveAt(0);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("清空失败", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
             * */

            /*
            DialogResult dlResult = MessageBox.Show(this, "要删除全部记录", "请确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign);
            if (dlResult == DialogResult.Yes)
            {
                int j = dataGridView1.SelectedRows.Count;
                int[] l = new int[j];

                int i;
                for (i = 0; i < j; i++)
                {
                    l[i] = dataGridView1.SelectedRows[i].Index;
                }

                int k = 0;
                while (k < j)
                {
                    this.libraryDataSet.Books.Rows[l[k]].Delete();
                    k++;
                }
            }
             * */
        }

        private void update()                                           //保存
        {
            DialogResult dlResult = MessageBox.Show(this, "要保存修改", "请确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            if (dlResult == DialogResult.Yes)
            {
                this.booksTableAdapter1.Update(this.libraryDataSet.Books);
                /*
                DataTable dt = dataGridView1.DataSource as DataTable;
                if (dt != null)
                {
                    con.Open();
                    sql = "select * from Books";
                    da = new SqlDataAdapter(sql, con);
                    scb = new SqlCommandBuilder(da);
                    try
                    {
                        //da.Fill(ds, "Books");
                        //da.Update(ds);
                        //dataGridView1.Update();
                        MessageBox.Show("保存成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (DataException de)
                    {
                        MessageBox.Show("保存失败", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show(de.ToString());
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                        refresh();
                    }
                }
                 * */
            }
        }

        //*************************选择文件夹*************************//
        private void button1_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog scanDialog = new FolderBrowserDialog();
            scanDialog.Description = "选择文件路径";
            if (scanDialog.ShowDialog() == DialogResult.OK)
            {
                string x, s, y;
                int count = 0;                  // 导入数目
                int cfcount = 0;                // 重复数目
                string wr;                      // 作者名
                string bn;                      // 书名
                string foldPath = scanDialog.SelectedPath;
                textBox1.Text = foldPath;                                                       //文本框显示文件夹路径
                //MessageBox.Show("已选择文件夹:" + foldPath, "选择文件夹提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DirectoryInfo mydir = new DirectoryInfo(foldPath);
                con.Open();
                foreach (FileSystemInfo fsi in mydir.GetFileSystemInfos())
                {
                    if (fsi is FileInfo)
                    {
                        FileInfo fi = (FileInfo)fsi;
                        x = System.IO.Path.GetDirectoryName(fi.FullName);                // 完全的文件名
                        s = System.IO.Path.GetExtension(fi.FullName);                    // 拓展名
                        y = System.IO.Path.GetFileNameWithoutExtension(fi.FullName);     // 无拓展名                                                            
                        if (s == ".txt")
                        {
                            if (y.Contains("--"))
                            {
                                string[] split = y.Split(new Char[] { '-', '-' }, StringSplitOptions.RemoveEmptyEntries);
                                wr = split[0];
                                bn = split[1];
                            }
                            else
                            {
                                wr = "";
                                bn = y;
                            }
                            sql = string.Format("select count(bAllName) FROM Books where bAllName='" + y + "'");
                            //sql = string.Format("select count(bName) FROM Books where bName='" + bn + "' and bWriter='"+ wr +"'");
                            cmd = new SqlCommand(sql, con);
                            int judge = (int)cmd.ExecuteScalar();
                            if (judge >= 1)  //书名重复时
                            {
                                cfcount++;
                                //MessageBox.Show("书名重复："+bn);
                            }
                            else
                            {
                                cmd = con.CreateCommand();
                                //cmd.Connection = con;                                             //  链接
                                cmd.CommandText = "insert into Books(bAllName,bWriter,bName) values(@bAllName,@bWriter,@bName)";
                                //cmd.Parameters.AddWithValue("bName", bn);
                                //cmd.Parameters.AddWithValue("bWriter", wr);
                                cmd.Parameters.Add("@bAllName", SqlDbType.NVarChar);
                                cmd.Parameters["@bAllName"].Value = y;
                                cmd.Parameters.Add("@bWriter", SqlDbType.NVarChar);
                                cmd.Parameters["@bWriter"].Value = wr;
                                cmd.Parameters.Add("@bName", SqlDbType.NVarChar);
                                cmd.Parameters["@bName"].Value = bn;
                                cmd.ExecuteNonQuery();
                                count++; ;
                            }
                        }
                    }
                }
                MessageBox.Show(cfcount + "本书导入重复");
                MessageBox.Show(count + "本书导入书库");

                refresh();
            }
            else
            {
                MessageBox.Show("导入不成功！");
            }
            con.Close();
        }

        //*************************选择文件***************************//
        private void button4_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog scanFileDialog = new OpenFileDialog();
            scanFileDialog.Multiselect = true; //选择多个文件
            scanFileDialog.Title = "请选择文件";
            scanFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";      // 有顺序要求！

            if (scanFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName;                                                        // 文件名 全名
                string wr;                                                              // 作者名
                string bn;                                                              // 书名
                int count = 0;                                                          // 统计导入数目
                int cfcount = 0;                                                        // 统计重复数目

                con.Open();
                foreach (string f in scanFileDialog.FileNames)
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(f);
                    //MessageBox.Show("已选择文件:" + fileName, "选择文件提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (fileName.Contains("--"))
                    {
                        string[] split = fileName.Split(new Char[] { '-', '-' }, StringSplitOptions.RemoveEmptyEntries);
                        wr = split[0];
                        bn = split[1];
                    }
                    else
                    {
                        wr = "";
                        bn = fileName;
                    }
                    sql = string.Format("select count(bAllName) FROM Books where bAllName='" + fileName + "'");
                    //sql = string.Format("select count(bName) FROM Books where bName='" + bn + "' and bWriter='" + wr + "'");
                    cmd = new SqlCommand(sql, con);
                    int judge = (int)cmd.ExecuteScalar();
                    if (judge >= 1)  //书名重复时
                    {
                        cfcount++;
                        //MessageBox.Show("书名重复："+bn);
                    }
                    else        //书名不重复
                    {
                        sql = "insert into Books(bAllName,bWriter,bName) values(@bAllName,@bWriter,@bName)";
                        cmd = con.CreateCommand();
                        //cmd.Connection = con;                                             //  链接
                        cmd.CommandText = sql;
                        //cmd.Parameters.AddWithValue("bName", bn);
                        //cmd.Parameters.AddWithValue("bWriter", wr);
                        cmd.Parameters.Add("@bAllName", SqlDbType.NVarChar);
                        cmd.Parameters["@bAllName"].Value = fileName;
                        cmd.Parameters.Add("@bWriter", SqlDbType.NVarChar);
                        cmd.Parameters["@bWriter"].Value = wr;
                        cmd.Parameters.Add("@bName", SqlDbType.NVarChar);
                        cmd.Parameters["@bName"].Value = bn;
                        cmd.ExecuteNonQuery();
                        count++; ;
                    }
                }
                MessageBox.Show(cfcount + "本书导入重复");
                MessageBox.Show(count + "本书导入书库");

                refresh();
            }
            else
            {
                MessageBox.Show("导入不成功！");
            }
            con.Close();
        }

        //*************************批量添加***************************//
        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog scanFileDialog = new OpenFileDialog();
            scanFileDialog.Multiselect = true; //选择多个文件
            scanFileDialog.Title = "请选择文件";
            scanFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";      // 有顺序要求！

            if (scanFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName;                                                        // 文件名 全名
                string wr;                                                              // 作者名
                string bn;                                                              // 书名
                int count = 0;                                                          // 统计导入数目
                int cfcount = 0;                                                        // 统计重复数目

                con.Open();
                foreach (string f in scanFileDialog.FileNames)
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(f);
                    //MessageBox.Show("已选择文件:" + fileName, "选择文件提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (fileName.Contains("--"))
                    {
                        string[] split = fileName.Split(new Char[] { '-', '-' }, StringSplitOptions.RemoveEmptyEntries);
                        wr = split[0];
                        bn = split[1];
                    }
                    else
                    {
                        wr = "";        //不知道作者名
                        bn = fileName;
                    }
                    sql = string.Format("select count(bAllName) FROM Books where bAllName='" + fileName + "'");
                    //sql = string.Format("select count(bName) FROM Books where bName='" + bn + "' and bWriter='" + wr + "'");
                    cmd = new SqlCommand(sql, con);
                    int judge = (int)cmd.ExecuteScalar();
                    if (judge >= 1)  //书名重复时
                    {
                        cfcount++;
                        //MessageBox.Show("书名重复："+bn);
                    }
                    else        //书名不重复
                    {
                        sql = "insert into Books(bAllName,bWriter,bName,bType,bState,bRead,bStar) values(@bAllName,@bWriter,@bName,@bType,@bState,@bRead,@bStar)";
                        cmd = con.CreateCommand();
                        cmd.CommandText = sql;
                        cmd.Parameters.Add("@bAllName", SqlDbType.NVarChar);
                        cmd.Parameters["@bAllName"].Value = fileName;
                        cmd.Parameters.Add("@bWriter", SqlDbType.NVarChar);
                        cmd.Parameters["@bWriter"].Value = wr;
                        cmd.Parameters.Add("@bName", SqlDbType.NVarChar);
                        cmd.Parameters["@bName"].Value = bn;
                        cmd.Parameters.Add("@bType", SqlDbType.NVarChar);
                        cmd.Parameters["@bType"].Value = comboBox1.Text.Trim();
                        cmd.Parameters.Add("@bState", SqlDbType.NVarChar);
                        cmd.Parameters["@bState"].Value = comboBox2.Text.Trim();
                        cmd.Parameters.Add("@bRead", SqlDbType.NVarChar);
                        cmd.Parameters["@bRead"].Value = comboBox3.Text.Trim();
                        cmd.Parameters.Add("@bStar", SqlDbType.NVarChar);
                        cmd.Parameters["@bStar"].Value = comboBox4.Text.Trim();
                        cmd.ExecuteNonQuery();
                        count++; ;
                    }
                }
                MessageBox.Show(cfcount + "本书导入重复");
                MessageBox.Show(count + "本书导入书库");

                refresh();
            }
            else
            {
                MessageBox.Show("导入不成功！");
            }
            con.Close();
        }

        private void button5_Click_1(object sender, EventArgs e)    // 删除
        {
            delete();
        }

        private void button2_Click_1(object sender, EventArgs e)    // 刷新
        {
            refresh();
        }

        private void button3_Click_1(object sender, EventArgs e)    // 清空
        {
            delAll();
        }

        private void button6_Click(object sender, EventArgs e)      // 保存
        {
            update();
        }

        private void button7_Click(object sender, EventArgs e)      // 添加
        {
            try
            {
                // 首先判断输入信息是否完全
                if (textBox2.Text == "" || textBox3.Text == "")
                {
                    MessageBox.Show("请输入完整的联系人信息", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string allName = textBox2.Text.Trim() + "--" + textBox3.Text.Trim();    //全名
                    // 在此判断保存重复记录问题
                    sql = string.Format("SELECT * FROM Books where bAllName='" + allName + "'");
                    //sql = string.Format("select count(bName) FROM Books where bName='" + textBox3.Text.Trim() + "' and bWriter='" + textBox2.Text.Trim() + "'");
                    //定义SQL Server连接对象
                    da = new SqlDataAdapter(sql, con);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        //提示该联系人编号已经存在
                        MessageBox.Show("该联系人编号已经存在,请您更换联系人的编号", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (MessageBox.Show("确认要保存该联系人信息吗？", "信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {

                            sql = string.Format(@"insert into Books(bName,bWriter) values ('{2}','{0}','{1}')", textBox3.Text.Trim(), textBox2.Text.Trim(), allName);
                            //定义SQL Server连接对象
                            cmd = new SqlCommand(sql, con);
                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();

                            }
                            catch { }
                            finally
                            {
                                con.Close();
                                con.Dispose();
                                cmd.Dispose();
                            }
                            MessageBox.Show("信息保存成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //刷新数据
                            refresh();
                            /*
                            sql = "select * from Books order by bName";
                            SqlDataAdapter da1 = new SqlDataAdapter(sql, con);
                            da1.Fill(ds);
                            if (ds.Tables[0].Rows.Count > 1)
                            {
                                dataGridView1.DataSource = ds.Tables[0];
                            }
                             * */
                        }
                    }
                }
            }
            catch { }
        }

        //*************************增加行号***************************//
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv != null)
            {
                Rectangle rect = new Rectangle(e.RowBounds.Location.X,
                    e.RowBounds.Location.Y,
                    dgv.RowHeadersWidth - 4,
                    e.RowBounds.Height);
                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    dgv.RowHeadersDefaultCellStyle.Font,
                    rect, dgv.RowHeadersDefaultCellStyle.ForeColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            }
        }

        //*************************数据库备份***************************//
        private void button8_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "备份数据库";
            saveFileDialog1.Filter = "书库文件(*.lbk)|*.lbk|所有文件(*.*)|*.*";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.ShowDialog();
            //string saveFilePath = saveFileDialog1.FileName.ToString();                                      //文件路径
            //string saveFilePathWithoutName = saveFilePath.Substring(1, saveFilePath.LastIndexOf("\\"));     //文件路径 不带文件名 
            //string saveFileNameExt = saveFilePath.Substring(saveFilePath.LastIndexOf("\\") + 1);            //文件名 不带路径
            //textBox5.Text = saveFilePathWithoutName + DateTime.Now.ToString("yyyyMMdd") + "-" + saveFileNameExt;
            textBox5.Text = saveFileDialog1.FileName;

            if (textBox5.Text == "")
            {
                MessageBox.Show("请输入要备份数据库的备份路径", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                //若该文件夹下有同名文件存在，则先删掉！ 否则大小会不断叠加上去=
                if (File.Exists(saveFileDialog1.FileName))
                    File.Delete(saveFileDialog1.FileName);

                con = new SqlConnection(consqlserver);
                con.Open();
                sql = "BACKUP DATABASE " + dbName + " TO disk='" + textBox5.Text + "'";
                //sql = "BACKUP DATABASE " + dbName + " TO disk='" + textBox5.Text + "'With Differential";//差异备份
                cmd = new SqlCommand(sql, con);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("数据库备份成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据库备份失败", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        //*************************数据库还原***************************//
        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "还原数据库";
            openFileDialog1.Filter = "书库文件(*.lbk)|*.lbk|所有文件(*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;

            openFileDialog1.ShowDialog();
            textBox7.Text = openFileDialog1.FileName;

            if (textBox7.Text == "")
            {
                MessageBox.Show("请输入或选择要还原数据库的备份路径", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {

                ///杀死原来所有的数据库连接进程
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Data Source=.;Initial Catalog=master;User ID=sa;pwd =123;pooling=false ";
                //conn.ConnectionString = "Data Source=.;Initial Catalog=master;Integrated Security=SSPI";
                conn.Open();
                string sql = "SELECT spid FROM sysprocesses ,sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='" +
                              dbName + "'";
                SqlCommand cmd1 = new SqlCommand(sql, conn);
                SqlDataReader dr;
                ArrayList list = new ArrayList();
                try
                {
                    dr = cmd1.ExecuteReader();
                    while (dr.Read())
                    {
                        list.Add(dr.GetInt16(0));
                    }
                    dr.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
                //MessageBox.Show(list.Count.ToString());
                for (int i = 0; i < list.Count; i++)
                {
                    conn.Open();
                    sql = string.Format("KILL {0}", list[i].ToString());
                    cmd1 = new SqlCommand(sql, conn);
                    cmd1.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("系统已经清除的数据库线程： " + list[i].ToString() + "\r\n正在还原数据库！");
                }

                sql = "use master RESTORE DATABASE " + dbName + " FROM disk= '" + textBox7.Text + "' WITH REPLACE";
                con = new SqlConnection(consqlserver);
                con.Open();
                //cmd.CommandText = @"RESTORE DATABASE " + textBox6.Text + " FROM disk= '" + textBox7.Text + "' WITH REPLACE";
                //cmd.Connection = con;
                cmd = new SqlCommand(sql, con);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("数据库还原成功", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SqlConnection.ClearAllPools();  //加上这个才不会报错
                    refresh();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("数据库还原失败", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dataGridView1.Rows[e.RowIndex].Selected == false)
                    {
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dataGridView1.SelectedRows.Count == 1)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    //弹出操作菜单
                    contextMenuStrip.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)   // 修改
        {
            //dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].ReadOnly = false;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)   // 删除
        {
            delete();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)   // 清空
        {
            delAll();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)   // 刷新
        {
            refresh();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)   // 保存
        {
            update();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //*************************组合查询***************************//
        private void button11_Click(object sender, EventArgs e)
        {
            DialogResult dlResult = MessageBox.Show(this, "进行查询", "请确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            if (dlResult == DialogResult.Yes)
            {
                sql = "select * from Books where 1 = 1 ";
                if (textBox4.Text != "")  //作者名
                {
                    sql += " and bWriter='" + textBox4.Text + "'";
                }
                if (textBox6.Text != "")  //书名
                {
                    sql += " and bName='" + textBox6.Text + "'";
                }
                if (textBox8.Text != "")  //类型
                {
                    sql += " and bType='" + textBox8.Text + "'";
                }
                if (textBox9.Text != "")  //首发站
                {
                    sql += " and bFirstWeb='" + textBox9.Text + "'";
                }
                if (textBox10.Text != "")  //主角
                {
                    sql += " and bMainCharacter='" + textBox10.Text + "'";
                }
                if (textBox11.Text != "")  //标签
                {
                    sql += " and bTag='" + textBox11.Text + "'";
                }
                if (comboBox5.Text != "")  //状态
                {
                    sql += " and bState='" + comboBox5.Text + "'";
                }
                if (comboBox6.Text != "")  //已阅
                {
                    sql += " and bRead='" + comboBox6.Text + "'";
                }
                if (comboBox7.Text != "")  //评价
                {
                    sql += " and bAssessment='" + comboBox7.Text + "'";
                }
                if (comboBox8.Text != "")  //星级
                {
                    sql += " and bStar='" + comboBox8.Text + "'";
                }
                try
                {
                    //con.ConnectionString = consqlserver;
                    /*
                    da = new SqlDataAdapter(sql, con);
                    con.Open();
                    da.Fill(ds);
                    dataGridView1.DataSource=ds.Tables[0];
                     * */
                    //this.booksTableAdapter1.Fill(this.libraryDataSet.Books);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("查询失败", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }



            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)   // 切换模式
        {
            dataGridView1.ReadOnly = !dataGridView1.ReadOnly;
        }


     
       



        
    }
}
