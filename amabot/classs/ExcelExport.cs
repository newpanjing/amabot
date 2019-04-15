using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace amabot.classs
{
    class ExcelExport
    {

        public static void export(List<Comment> comments)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("users");

            // 第一列
            NPOI.SS.UserModel.IRow row = sheet.CreateRow(0);

            string[] headers = new string[]{
            "昵称","邮箱","评价星级","排名","个人主页"
            };
            for (int i=0;i<headers.Length;i++)
            {
                row.CreateCell(i).SetCellValue(headers[i]);
            }

            for (int i = 0; i < comments.Count; i++)
            {
                Comment c = comments[i];
                // 第二列
                NPOI.SS.UserModel.IRow row2 = sheet.CreateRow(i+1);

                row2.CreateCell(0).SetCellValue(c.NickName);
                row2.CreateCell(1).SetCellValue(c.Email);
                row2.CreateCell(2).SetCellValue(c.Star);
                row2.CreateCell(3).SetCellValue(c.Rank);
                row2.CreateCell(4).SetCellValue(c.Profile);
            }

         

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel文件(*.xls)|*.xls";

            if (dialog.ShowDialog() == DialogResult.OK) {

                string fileName= dialog.FileName;
                try
                {
                    
                    FileStream stream = new FileStream(fileName, FileMode.Create);
                    using (stream)
                    {
                        book.Write(stream);
                    }
                }
                catch (Exception e) {
                    MessageBox.Show("保存文件失败！");
                }
            }
            


        }
    }
}
