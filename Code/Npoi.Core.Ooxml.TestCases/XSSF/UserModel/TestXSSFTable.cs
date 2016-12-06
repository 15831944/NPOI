﻿/* ====================================================================
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for Additional information regarding copyright ownership.
   The ASF licenses this file to You under the Apache License, Version 2.0
   (the "License"); you may not use this file except in compliance with
   the License.  You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
==================================================================== */

namespace Npoi.Core.XSSF.UserModel
{
    using Npoi.Core.OpenXmlFormats.Spreadsheet;
    using Npoi.Core.SS.UserModel;
    using Npoi.Core.XSSF;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class TestXSSFTable
    {
        [Test]
        public void Bug56274()
        {
            // read sample file
            XSSFWorkbook inputWorkbook = XSSFTestDataSamples.OpenSampleWorkbook("56274.xlsx");

            // read the original sheet header order
            XSSFRow row = inputWorkbook.GetSheetAt(0).GetRow(0) as XSSFRow;
            List<String> headers = new List<String>();
            foreach (ICell cell in row)
            {
                headers.Add(cell.StringCellValue);
            }

            // no SXSSF class
            // save the worksheet as-is using SXSSF
            //File outputFile = File.CreateTempFile("poi-56274", ".xlsx");
            //SXSSFWorkbook outputWorkbook = new Npoi.Core.XSSF.streaming.SXSSFWorkbook(inputWorkbook);
            //outputWorkbook.Write(new FileOutputStream(outputFile));

            // re-read the saved file and make sure headers in the xml are in the original order
            //inputWorkbook = new Npoi.Core.XSSF.UserModel.XSSFWorkbook(new FileStream(outputFile));
            inputWorkbook = XSSFTestDataSamples.WriteOutAndReadBack(inputWorkbook) as XSSFWorkbook;
            CT_Table ctTable = (inputWorkbook.GetSheetAt(0) as XSSFSheet).GetTables()[0].GetCTTable();
            List<CT_TableColumn> ctTableColumnList = ctTable.tableColumns.tableColumn;

            Assert.AreEqual(headers.Count, ctTableColumnList.Count,
                    "number of headers in xml table should match number of header cells in worksheet");
            for (int i = 0; i < headers.Count; i++)
            {
                Assert.AreEqual(headers[i], ctTableColumnList[i].name,
                    "header name in xml table should match number of header cells in worksheet");
            }
            //Assert.IsTrue(outputFile.Delete());
        }
    }
}