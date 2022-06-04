using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Excel;
using Excel.Core;
using Excel.Exceptions;
using System.IO;
using Farme.Net;
using System.Data;
using OfficeOpenXml;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //FileInfo fileInfo = new FileInfo(Application.dataPath + "/面试表.xlsx");
        //if (fileInfo.Exists)
        //{
        //    fileInfo.Delete();
        //    fileInfo = new FileInfo(Application.dataPath + "/面试表.xlsx");

        //    using (ExcelPackage ep = new ExcelPackage(fileInfo))
        //    {
        //        ExcelWorksheet worksheet = ep.Workbook.Worksheets.Add("我的Excel");
        //        worksheet.Cells[1, 1].Value = "序号";
        //        worksheet.Cells[1, 2].Value = "姓名";
        //        worksheet.Cells[1, 3].Value = "电话";
        //        ep.Save();
        //        Debug.Log("导出Excel成功");
        //    }

        //}
        string path = @"http://36a62990f3.zicp.vip/AngryBirds.mp4";
        WebDownloadTool.DownloadFile(path, (data) =>
        {
            //using (MemoryStream ms = new MemoryStream(data))
            //{
            //    using (IExcelDataReader edr = ExcelReaderFactory.CreateOpenXmlReader(ms))
            //    {
            //        using (DataSet dataSet = edr.AsDataSet())
            //        {
            //            int columns = dataSet.Tables[0].Columns.Count;
            //            int rows = dataSet.Tables[0].Rows.Count;
            //            Debug.Log(rows);
            //            for (int i = 0; i < rows; i++)
            //            {

            //                //InterviewJson ij = new InterviewJson();
            //                //for (int j = 0; j < columns; j++)
            //                //{
            //                //    string gg = dataSet.Tables[0].Rows[i][j].ToString();
            //                //    if (string.IsNullOrEmpty(gg)&&(i>=2))
            //                //    {
            //                //        continue;
            //                //    }
            //                //    BuilderJsonData(i, j, gg);
            //                //}
            //            }
            //        }
            //    }
            //}
        }, (pro) => { Debug.Log(pro);  });

    }
    private Person person = new Person();
    private void BuilderJsonData(int row,int column, string info)
    {
        InterviewJson ij = person.IJ[person.IJ.Count - 1];
        if (person.IJ.Count<(row/2))
        {
            ij = new InterviewJson();
            person.IJ.Add(ij);
        }
      

    }
}

public class Person
{
    public List<InterviewJson> IJ = new List<InterviewJson>();
}

public class InterviewJson
{
    /// <summary>
    /// 公司名称
    /// </summary>
    public string Company;
    /// <summary>
    /// 岗位
    /// </summary>
    public string Jobs;
    /// <summary>
    /// 地址
    /// </summary>
    public string Address;
    /// <summary>
    /// 面试时间
    /// </summary>
    public string InterviewTime;
}

