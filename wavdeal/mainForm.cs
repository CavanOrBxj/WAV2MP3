using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace wavdeal
{
    public partial class mainForm : Form
    {

        string sWavFolder = "";//定义存放Wav的文件夹路径
        string sMp3Folder = "";//定义存放转换成mp3文件的文件夹路径
        private IniFiles ini;
        FileSystemWatcher fswWavFileForder = null;//文件监控器。
        public Object olLock;  //文件列表list操作锁
        Thread thDealCompressor = null;
        List<string> lwavfiles = null;//初始化文件列表

        //private delegate void setLogTextDelegate(FileSystemEventArgs e);

        //private delegate void renamedDelegate(RenamedEventArgs e);

        public mainForm()
        {
            InitializeComponent();
        }

        #region 文件转换
        private static string mp3CoderWavToMp3(string wavFile, string outmp3File)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = @Application.StartupPath + "\\mp3Coder\\mp3Coder.exe";
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            if (wavFile.IndexOf(' ') > 0)
            {
                wavFile = "\"" + wavFile + "\"";
            }
            if (outmp3File.IndexOf(' ') > 0)
            {
                outmp3File = "\"" + outmp3File + "\"";
            }
            psi.Arguments = wavFile + " " + outmp3File;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(psi);
            p.StartInfo.RedirectStandardOutput = true;
            string strRst = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            //获取结果
            //Log.Instance.LogWrite("48行-转换文件:" + wavFile);
            if (strRst.Contains("Done: 100.00%"))
            {
                //MessageBox.Show("完成转换！");
                return "OK";
            }
            else
            {
                return "NO";
            }
        }

        private static string LameWavToMp3(string wavFile, string outmp3File)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = @Application.StartupPath + "\\lame.exe";
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            if (wavFile.IndexOf(' ') > 0)
            {
                wavFile = "\"" + wavFile + "\"";
            }
            if (outmp3File.IndexOf(' ') > 0)
            {
                outmp3File = "\"" + outmp3File + "\"";
            }
            psi.Arguments = "-V2 " + wavFile + " " + outmp3File;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process p = Process.Start(psi);
            p.StartInfo.RedirectStandardOutput = true;
            string strRst = p.StandardOutput.ReadToEnd();
            p.WaitForExit(500);
            return strRst;
        }

        private static string shellwavtomp3(string wavFile, string outmp3File)
        {
            try
            {
                //实例一个process类
                Process process = new Process();//设定程序名
                process.StartInfo.FileName = "cmd.exe";//使用cmd
                process.StartInfo.UseShellExecute = false;//关闭Shell的使用
                //重新定向标准输入，输入，错误输出
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;//设置cmd窗口不显示
                process.Start();//开始
                //输入命令，退出
                if (wavFile.IndexOf(' ') > 0)
                {
                    wavFile = "\"" + wavFile + "\"";
                }
                if (outmp3File.IndexOf(' ') > 0)
                {
                    outmp3File = "\"" + outmp3File + "\"";
                }
                string sShell = "mp3Coder " + wavFile + " " + outmp3File;
                process.StandardInput.WriteLine(sShell);
                process.StandardInput.WriteLine("exit");
                Thread.Sleep(500);
                string strRst = process.StandardOutput.ReadToEnd();
                //获取结果
                if (strRst.Contains("Done: 100.00%"))
                {
                    MessageBox.Show("完成转换！");
                    return "OK";
                }
                else
                {
                    return "NO";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "NO";
            }
            //process.WaitForExit();
        }
        #endregion End

        private void button1_Click(object sender, EventArgs e)
        {
            string sWavfilepath = @Application.StartupPath + "\\rec_2015_8_24_16_12_1_727.wav";
            string sMp3FilePath = @Application.StartupPath + "\\mp3files\\" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".mp3";
            string msg = mp3CoderWavToMp3(sWavfilepath, sMp3FilePath);
            MessageBox.Show(msg);
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {

            if (btnConvert.Text == "启动转换")
            {
                #region 检查文件夹设置
                if (sWavFolder == "")
                {
                    MessageBox.Show("请设置Wav文件存放的文件夹！");
                    return;
                }
                else
                {
                    if (!Directory.Exists(sWavFolder))
                    {
                        MessageBox.Show("设置的Wav文件夹路径不存在！");
                        return;
                    }
                }
                if (sMp3Folder == "")
                {
                    MessageBox.Show("请设置Mp3文件存放的文件夹！");
                }
                else
                {
                    if (!Directory.Exists(sMp3Folder))
                    {
                        MessageBox.Show("设置的mp3文件夹路径不存在！");
                        return;
                    }
                }
                #endregion End

                //先把wav文件夹下已经有的文件载入到
                DirectoryInfo theFolder = new DirectoryInfo(sWavFolder);
                FileInfo[] wavfiles = theFolder.GetFiles("*.wav");
                if (wavfiles.Length > 0)
                {
                    lwavfiles.Clear();//清除掉残余转换文件列表，重新加入转换列表。
                    for (int i = 0; i < wavfiles.Length; i++)
                    {
                        //int wfs = wavfiles[i].Name.Length;
                        //string wavfilesuffix = wavfiles[i].Name.Substring(wfs -3,3).ToLower();
                        //if (wavfilesuffix == "wav")
                        //{
                        //    lwavfiles.Add(sWavFolder + "\\" + wavfiles[i].Name);
                        //}
                        lwavfiles.Add(sWavFolder + "\\" + wavfiles[i].Name);
                    }
                }
                thDealCompressor = new Thread(DealCompressor);
                thDealCompressor.Name = "";
                thDealCompressor.IsBackground = true;
                thDealCompressor.Start();
                fswWavFileForder.EnableRaisingEvents = true;//启用该组件
                btnConvert.Text = "停止转换";
            }
            else
            {
                fswWavFileForder.EnableRaisingEvents = false;//启用该组件
                btnConvert.Text = "启动转换";
                if (thDealCompressor != null)
                {
                    thDealCompressor.Abort();
                    thDealCompressor = null;
                }
            }
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            //初始化文件夹路径
            lwavfiles = new List<string>();//初始化
            olLock = new Object();

            ini = new IniFiles(@Application.StartupPath + "\\Config.ini");//

            sWavFolder = ini.ReadValue("FilesPath", "WavFilesPath"); // @Application.StartupPath + "\\wavfiles";
            sMp3Folder = ini.ReadValue("FilesPath", "Mp3FilesPath"); //@Application.StartupPath + "\\mp3files";
            txtMp3Forlder.Text = sMp3Folder;
            txtWavForlder.Text = sWavFolder;
            if (!Directory.Exists(sMp3Folder))
            {
                MessageBox.Show("设置的Mp3文件夹路径不存在,请在配置文件下配置好在运行！");
                this.Close();
                return;
            }
            if (!Directory.Exists(sWavFolder))
            {
                MessageBox.Show("设置的Wav文件夹路径不存在,请在配置文件下配置好在运行！");
                this.Close();
                return;
            }

            fswWavFileForder = new FileSystemWatcher();
            fswWavFileForder.Filter = "*.wav";
            fswWavFileForder.IncludeSubdirectories = false;//不监视子路径
            fswWavFileForder.Path = sWavFolder;
            fswWavFileForder.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size | NotifyFilters.CreationTime;//
            fswWavFileForder.Created += new FileSystemEventHandler(this.fswWavFileForder_EventHandle);
            //fswWavFileForder.EnableRaisingEvents = true;//启用该组件

            //
            string sLogPath = Application.StartupPath + "\\Log";
            if (!System.IO.Directory.Exists(sLogPath))
                System.IO.Directory.CreateDirectory(sLogPath);
            Log.Instance.LogDirectory = sLogPath + "\\";
            Log.Instance.FileNamePrefix = "WavToMp3";
            Log.Instance.CurrentMsgType = MsgLevel.Debug;
            Log.Instance.logFileSplit = LogFileSplit.Daily;
            Log.Instance.MaxFileSize = 2;
            Log.Instance.InitParam();




            btnConvert_Click(null, null);
        }

        private void fswWavFileForder_EventHandle(object sender, FileSystemEventArgs e)
        {
            string[] strArr = e.FullPath.Split(new char[] { '\\' });
            if (!strArr[1].Equals("$RECYCLE.BIN"))  //回收站
            {
                //(e.ChangeType.ToString(); WatcherChangeTypes.Created等
                //strArr[strArr.Length - 1]); //文件名
                //e.FullPath;
                //Log.Instance.LogWrite("243行:" + e.FullPath);
                lock (olLock)
                {
                    switch (e.ChangeType)
                    {
                        case WatcherChangeTypes.Created:
                            lwavfiles.Add(e.FullPath);//
                            //lwavfiles.Add(strArr[strArr.Length - 1]);//文件名列表
                            break;
                        case WatcherChangeTypes.Deleted:
                            //lwavfiles.Remove(e.FullPath);//删除了的wav文件就移除
                            break;
                        case WatcherChangeTypes.Changed:

                            break;
                    }
                }

            }
        }
        //处理解压缩转换过程
        private void DealCompressor()
        {
            List<string> lftmp = new List<string>();//临时文件列表
            //处理增加文件列表
            try
            {
                while (true)
                {
                    try
                    {
                        lock (olLock)
                        {
                            DirectoryInfo theLineFolder = new DirectoryInfo(sWavFolder);
                            FileInfo[] linewavfiles = theLineFolder.GetFiles("*.wav");
                            if (linewavfiles.Length > 0)
                            {
                                //lwavfiles.Clear();//清除掉残余转换文件列表，重新加入转换列表。
                                for (int i = 0; i < linewavfiles.Length; i++)
                                {
                                    if (!lwavfiles.Contains(sWavFolder + "\\" + linewavfiles[i].Name))
                                    {
                                        lwavfiles.Add(sWavFolder + "\\" + linewavfiles[i].Name);
                                    }
                                }
                            }
                            if (lwavfiles.Count == 0)
                            {
                                Thread.Sleep(500);
                                continue;
                            }
                            lftmp.InsertRange(0, lwavfiles);
                            lwavfiles.Clear();
                        }
                    }
                    catch (Exception exs)
                    {
                        Console.WriteLine(exs.Message);
                        Log.Instance.LogWrite("330行-更新转换文件列表:" + exs.Message);
                    }
                    //处理文件列表
                    for (int i = 0; i < lftmp.Count; i++)
                    {
                        //处理转换
                        string[] strArr = lftmp[i].Split(new char[] { '\\' });
                        string fname = strArr[strArr.Length - 1];//获取文件名
                        fname = fname.Substring(0, fname.Length - 4);
                        string sMp3FilePath = sMp3Folder + "\\" + fname + ".mp3";//以时间生成System.DateTime.Now.ToString("yyyyMMddHHmmssfff")
                       
                        if (File.Exists(sMp3FilePath))
                        {
                            File.Delete(lftmp[i]);//已经存在转换文件就删除原文件
                            lftmp.Remove(lftmp[i]);
                            i = i - 1;
                            continue;
                        }

                        if (mp3CoderWavToMp3(lftmp[i], sMp3FilePath) == "OK")
                        {
                            //保存记录数据到数据库
                            //Console.WriteLine("文件：" + lftmp[i] + " 已经转换！");
                            //删除原wav文件
                            try
                            {
                                if (File.Exists(lftmp[i]))
                                {
                                    File.Delete(lftmp[i]);
                                    lftmp.Remove(lftmp[i]);
                                }
                                //File.Delete(@"" + lftmp[i]);
                                i--;
                            }
                            catch(Exception ex)
                            {
                                //Log.Instance.LogWrite(ex.Message);
                                Console.WriteLine(ex.Message);
                                i--;
                            }
                            //进行下一步操作
                            //i = i - 1;
                        }
                        else
                        {
                            Log.Instance.LogWrite("375行:" + sMp3FilePath);
                            i--;
                            continue;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DealArry:" + ex.Message);
                Log.Instance.LogWrite("385行－总的错误:" + ex.Message);
            }
        }

        private void btnSelectWav_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                sWavFolder = dialog.SelectedPath;
                txtWavForlder.Text = dialog.SelectedPath;
                //MessageBox.Show("已选择文件夹:" + foldPath, "选择文件夹提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fswWavFileForder.Path = sWavFolder;//设置监控文件夹
            }
        }

        private void btnSelectMp3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                sMp3Folder = dialog.SelectedPath;
                txtMp3Forlder.Text = dialog.SelectedPath;
            }

        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (thDealCompressor != null)
            {
                thDealCompressor.Abort();
                thDealCompressor = null;
            }

        }
    }
}
