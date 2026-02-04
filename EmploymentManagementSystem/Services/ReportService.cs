using ClosedXML.Excel;
using EmploymentManagementSystem.DTOS;
using EmploymentManagementSystem.Repositories;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EmploymentManagementSystem.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        #region Employee Directory Reports

        public async Task<byte[]> GenerateEmployeeDirectoryPdfAsync()
        {
            var employees = await _reportRepository.GetEmployeeDirectoryAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text("Employee Directory Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(60);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                                columns.ConstantColumn(80);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.5f);
                                columns.ConstantColumn(70);
                                columns.ConstantColumn(70);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCellStyle).Text("Code").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Full Name").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Email").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Phone").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Department").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Position").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Salary").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Status").Bold();

                                static IContainer HeaderCellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold())
                                        .PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                }
                            });

                            foreach (var emp in employees)
                            {
                                table.Cell().Element(RowCellStyle).Text(emp.EmployeeCode);
                                table.Cell().Element(RowCellStyle).Text(emp.FullName);
                                table.Cell().Element(RowCellStyle).Text(emp.Email).FontSize(8);
                                table.Cell().Element(RowCellStyle).Text(emp.PhoneNumber);
                                table.Cell().Element(RowCellStyle).Text(emp.DepartmentName);
                                table.Cell().Element(RowCellStyle).Text(emp.Position);
                                table.Cell().Element(RowCellStyle).Text($"₹{emp.Salary:N0}");
                                table.Cell().Element(RowCellStyle).Text(emp.Status);

                                static IContainer RowCellStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                        .PaddingVertical(5);
                                }
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                            x.Span($" | Generated on {DateTime.Now:dd-MMM-yyyy HH:mm}");
                        });
                });
            });

            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateEmployeeDirectoryExcelAsync()
        {
            var employees = await _reportRepository.GetEmployeeDirectoryAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employee Directory");

            worksheet.Cell(1, 1).Value = "Employee Code";
            worksheet.Cell(1, 2).Value = "Full Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Phone Number";
            worksheet.Cell(1, 5).Value = "Department";
            worksheet.Cell(1, 6).Value = "Position";
            worksheet.Cell(1, 7).Value = "Salary";
            worksheet.Cell(1, 8).Value = "Joining Date";
            worksheet.Cell(1, 9).Value = "Status";

            var headerRange = worksheet.Range(1, 1, 1, 9);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            foreach (var emp in employees)
            {
                worksheet.Cell(row, 1).Value = emp.EmployeeCode;
                worksheet.Cell(row, 2).Value = emp.FullName;
                worksheet.Cell(row, 3).Value = emp.Email;
                worksheet.Cell(row, 4).Value = emp.PhoneNumber;
                worksheet.Cell(row, 5).Value = emp.DepartmentName;
                worksheet.Cell(row, 6).Value = emp.Position;
                worksheet.Cell(row, 7).Value = emp.Salary;
                worksheet.Cell(row, 7).Style.NumberFormat.Format = "₹#,##0";
                worksheet.Cell(row, 8).Value = emp.JoiningDate;
                worksheet.Cell(row, 8).Style.DateFormat.Format = "dd-MMM-yyyy";
                worksheet.Cell(row, 9).Value = emp.Status;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        #endregion

        #region Department Reports

        public async Task<byte[]> GenerateDepartmentReportPdfAsync()
        {
            var departments = await _reportRepository.GetDepartmentReportAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header()
                        .Text("Department Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1.5f);
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(70);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCellStyle).Text("Department").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Description").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Manager").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Employees").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Status").Bold();

                                static IContainer HeaderCellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold())
                                        .PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                }
                            });

                            foreach (var dept in departments)
                            {
                                table.Cell().Element(RowCellStyle).Text(dept.DepartmentName);
                                table.Cell().Element(RowCellStyle).Text(dept.Description);
                                table.Cell().Element(RowCellStyle).Text(dept.ManagerName);
                                table.Cell().Element(RowCellStyle).Text(dept.TotalEmployees.ToString());
                                table.Cell().Element(RowCellStyle).Text(dept.Status);

                                static IContainer RowCellStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                        .PaddingVertical(5);
                                }
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                            x.Span($" | Generated on {DateTime.Now:dd-MMM-yyyy HH:mm}");
                        });
                });
            });

            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateDepartmentReportExcelAsync()
        {
            var departments = await _reportRepository.GetDepartmentReportAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Departments");

            worksheet.Cell(1, 1).Value = "Department Name";
            worksheet.Cell(1, 2).Value = "Description";
            worksheet.Cell(1, 3).Value = "Manager Name";
            worksheet.Cell(1, 4).Value = "Total Employees";
            worksheet.Cell(1, 5).Value = "Status";

            var headerRange = worksheet.Range(1, 1, 1, 5);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            foreach (var dept in departments)
            {
                worksheet.Cell(row, 1).Value = dept.DepartmentName;
                worksheet.Cell(row, 2).Value = dept.Description;
                worksheet.Cell(row, 3).Value = dept.ManagerName;
                worksheet.Cell(row, 4).Value = dept.TotalEmployees;
                worksheet.Cell(row, 5).Value = dept.Status;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        #endregion

        #region Attendance Reports

        public async Task<byte[]> GenerateAttendanceReportPdfAsync(DateTime? startDate, DateTime? endDate)
        {
            var attendances = await _reportRepository.GetAttendanceReportAsync(startDate, endDate);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header()
                        .Column(column =>
                        {
                            column.Item().Text("Attendance Report").SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);
                            if (startDate.HasValue || endDate.HasValue)
                            {
                                column.Item().Text($"Period: {startDate?.ToString("dd-MMM-yyyy") ?? "Start"} to {endDate?.ToString("dd-MMM-yyyy") ?? "End"}")
                                    .FontSize(12);
                            }
                        });

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(70);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1.5f);
                                columns.ConstantColumn(80);
                                columns.ConstantColumn(70);
                                columns.ConstantColumn(70);
                                columns.ConstantColumn(60);
                                columns.ConstantColumn(60);
                                columns.RelativeColumn(1.5f);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCellStyle).Text("Emp Code").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Employee Name").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Department").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Date").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Check In").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Check Out").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Status").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Hours").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Remarks").Bold();

                                static IContainer HeaderCellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold())
                                        .PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                }
                            });

                            foreach (var att in attendances)
                            {
                                table.Cell().Element(RowCellStyle).Text(att.EmployeeCode);
                                table.Cell().Element(RowCellStyle).Text(att.EmployeeName);
                                table.Cell().Element(RowCellStyle).Text(att.DepartmentName);
                                table.Cell().Element(RowCellStyle).Text(att.Date.ToString("dd-MMM-yyyy"));
                                table.Cell().Element(RowCellStyle).Text(att.CheckInTime ?? "N/A");
                                table.Cell().Element(RowCellStyle).Text(att.CheckOutTime ?? "N/A");
                                table.Cell().Element(RowCellStyle).Text(att.Status);
                                table.Cell().Element(RowCellStyle).Text(att.WorkHours.ToString("0.00"));
                                table.Cell().Element(RowCellStyle).Text(att.Remarks ?? "");

                                static IContainer RowCellStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                        .PaddingVertical(5);
                                }
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                            x.Span($" | Generated on {DateTime.Now:dd-MMM-yyyy HH:mm}");
                        });
                });
            });

            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateAttendanceReportExcelAsync(DateTime? startDate, DateTime? endDate)
        {
            var attendances = await _reportRepository.GetAttendanceReportAsync(startDate, endDate);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Attendance");

            worksheet.Cell(1, 1).Value = "Employee Code";
            worksheet.Cell(1, 2).Value = "Employee Name";
            worksheet.Cell(1, 3).Value = "Department";
            worksheet.Cell(1, 4).Value = "Date";
            worksheet.Cell(1, 5).Value = "Check In Time";
            worksheet.Cell(1, 6).Value = "Check Out Time";
            worksheet.Cell(1, 7).Value = "Status";
            worksheet.Cell(1, 8).Value = "Work Hours";
            worksheet.Cell(1, 9).Value = "Remarks";

            var headerRange = worksheet.Range(1, 1, 1, 9);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            foreach (var att in attendances)
            {
                worksheet.Cell(row, 1).Value = att.EmployeeCode;
                worksheet.Cell(row, 2).Value = att.EmployeeName;
                worksheet.Cell(row, 3).Value = att.DepartmentName;
                worksheet.Cell(row, 4).Value = att.Date;
                worksheet.Cell(row, 4).Style.DateFormat.Format = "dd-MMM-yyyy";
                worksheet.Cell(row, 5).Value = att.CheckInTime;
                worksheet.Cell(row, 6).Value = att.CheckOutTime;
                worksheet.Cell(row, 7).Value = att.Status;
                worksheet.Cell(row, 8).Value = att.WorkHours;
                worksheet.Cell(row, 9).Value = att.Remarks;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        #endregion

        #region Salary Reports

        public async Task<byte[]> GenerateSalaryReportPdfAsync()
        {
            var salaries = await _reportRepository.GetSalaryReportAsync();

            var document = Document.Create(container =>
            {
                _ = container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header()
                        .Text("Salary Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(70);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(1.5f);
                                columns.RelativeColumn(1.5f);
                                columns.ConstantColumn(90);
                                columns.ConstantColumn(90);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCellStyle).Text("Emp Code").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Employee Name").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Department").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Position").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Salary").Bold();
                                header.Cell().Element(HeaderCellStyle).Text("Joining Date").Bold();

                                static IContainer HeaderCellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold())
                                        .PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                }
                            });

                            decimal totalSalary = 0;
                            foreach (var sal in salaries)
                            {
                                table.Cell().Element(RowCellStyle).Text(sal.EmployeeCode);
                                table.Cell().Element(RowCellStyle).Text(sal.EmployeeName);
                                table.Cell().Element(RowCellStyle).Text(sal.DepartmentName);
                                table.Cell().Element(RowCellStyle).Text(sal.Position);
                                table.Cell().Element(RowCellStyle).Text($"₹{sal.Salary:N0}");
                                table.Cell().Element(RowCellStyle).Text(sal.JoiningDate.ToString("dd-MMM-yyyy"));

                                totalSalary += sal.Salary;

                                static IContainer RowCellStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                        .PaddingVertical(5);
                                }
                            }

                            table.Cell().ColumnSpan(4).Element(TotalCellStyle).Text("Total Salary:").Bold();
                            table.Cell().Element(TotalCellStyle).Text($"₹{totalSalary:N0}").Bold();
                            table.Cell();

                            static IContainer TotalCellStyle(IContainer container)
                            {
                                return container.BorderTop(2).BorderColor(Colors.Black)
                                    .PaddingVertical(5);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                            x.Span($" | Generated on {DateTime.Now:dd-MMM-yyyy HH:mm}");
                        });
                });
            });

            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateSalaryReportExcelAsync()
        {
            var salaries = await _reportRepository.GetSalaryReportAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Salary Report");

            worksheet.Cell(1, 1).Value = "Employee Code";
            worksheet.Cell(1, 2).Value = "Employee Name";
            worksheet.Cell(1, 3).Value = "Department";
            worksheet.Cell(1, 4).Value = "Position";
            worksheet.Cell(1, 5).Value = "Salary";
            worksheet.Cell(1, 6).Value = "Joining Date";

            var headerRange = worksheet.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;
            decimal totalSalary = 0;
            foreach (var sal in salaries)
            {
                worksheet.Cell(row, 1).Value = sal.EmployeeCode;
                worksheet.Cell(row, 2).Value = sal.EmployeeName;
                worksheet.Cell(row, 3).Value = sal.DepartmentName;
                worksheet.Cell(row, 4).Value = sal.Position;
                worksheet.Cell(row, 5).Value = sal.Salary;
                worksheet.Cell(row, 5).Style.NumberFormat.Format = "₹#,##0";
                worksheet.Cell(row, 6).Value = sal.JoiningDate;
                worksheet.Cell(row, 6).Style.DateFormat.Format = "dd-MMM-yyyy";
                totalSalary += sal.Salary;
                row++;
            }

            worksheet.Cell(row, 4).Value = "Total:";
            worksheet.Cell(row, 4).Style.Font.Bold = true;
            worksheet.Cell(row, 5).Value = totalSalary;
            worksheet.Cell(row, 5).Style.Font.Bold = true;
            worksheet.Cell(row, 5).Style.NumberFormat.Format = "₹#,##0";

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        #endregion
    }
}
